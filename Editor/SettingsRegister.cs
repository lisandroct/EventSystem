using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lisandroct.Core.Editor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace lisandroct.EventSystem
{
    public class SettingsRegister {
        private const string Library = "EventSystem";
        private const string Events = "Events";
        private const string Editor = "Editor";
        private const string Listeners = "Listeners";
        
        private static string LibraryPath => $"{CoreFolders.CorePath}/{Library}";
        private static string EventsPath => $"{LibraryPath}/{Events}";
        private static string EventsInspectorsPath => $"{EventsPath}/{Editor}";
        private static string ListenersPath => $"{LibraryPath}/{Listeners}";

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var tabIndex = 0;
            var scrollPosition = Vector2.zero;
            var generator = new CodeGenerator(EventsPath, EventsInspectorsPath, ListenersPath);

            string namespaceFilter = null;
            string classFilter = null;

            string newName = null;
            var newTypes = new List<Type>();

            IEnumerable<Type> types = null;
            IEnumerable<Type> filteredTypes = null;
            
            var serializedObject = Settings.GetSerializedObject();

            var settings = serializedObject.targetObject as Settings;

            var provider = new SettingsProvider("Project/EventSystem", SettingsScope.Project)
            {
                label = "EventSystem",
                guiHandler = (searchContext) =>
                {
                    serializedObject.Update();
                    
                    EditorGUI.BeginChangeCheck();
                    tabIndex = GUILayout.Toolbar(tabIndex, new [] { "Event Types", "Add New" });
                    if (EditorGUI.EndChangeCheck())
                    {
                        scrollPosition = Vector2.zero;
                        if (tabIndex == 1)
                        {
                            scrollPosition = Vector2.zero;
                            namespaceFilter = null;
                            classFilter = null;

                            types = LoadTypes();
                            filteredTypes = null;
                        }
                    }
                    
                    switch (tabIndex) {
                        case 0:
                        {
                            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                            for (int i = 0, n = settings.Definitions.Count; i < n; i++)
                            {
                                var definition = settings.Definitions[i];

                                if (!definition.IsValid)
                                {
                                    continue;
                                }
                                    
                                if(!EditorGUILayout.ToggleLeft(definition.ToString(), true)) {
                                    settings.Definitions.RemoveAt(i);
                                    break;
                                }
                            }

                            EditorGUILayout.EndScrollView();

                            if (GUILayout.Button("Generate"))
                            {
                                CreateFolders();

                                if (settings.Definitions != null)
                                {
                                    foreach (var definition in settings.Definitions)
                                    {
                                        if (!definition.IsValid)
                                        {
                                            continue;
                                        }
                                        
                                        generator.Generate(definition.Name, definition.GetTypes());
                                    }
                                }
                                
                                AssetDatabase.Refresh();
                            }

                            break;
                        }
                        case 1:
                        {
                            if (newTypes.Count > 0)
                            {
                                EditorGUI.BeginChangeCheck();
                                var targetName = EditorGUILayout.TextField("Name",
                                    string.IsNullOrEmpty(newName) ? ConcatenateTypeNames(newTypes) : newName);
                                if (EditorGUI.EndChangeCheck())
                                {
                                    newName = targetName;
                                }

                                GUILayout.BeginHorizontal();
                                for (int i = 0, n = newTypes.Count; i < n; i++)
                                {
                                    var type = newTypes[i];
                                    if (!GUILayout.Toggle(true, type.GetFriendlyName(), EditorStyles.miniButton))
                                    {
                                        newTypes.RemoveAt(i);
                                        break;
                                    }
                                }
                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();
                                
                                if (GUILayout.Button("Add Event Type"))
                                {
                                    var newDefinition = new EventDefinition(targetName, newTypes.ToArray());
                                    settings.Definitions.Add(newDefinition);
                                    
                                    newName = null;
                                    newTypes.Clear();
                                }
                            }

                            if (newTypes.Count < 4)
                            {
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField("Search types", EditorStyles.boldLabel);
                                EditorGUI.BeginChangeCheck();
                                namespaceFilter = EditorGUILayout.TextField("Namespace", namespaceFilter);
                                classFilter = EditorGUILayout.TextField("Class Name", classFilter);
                                if (EditorGUI.EndChangeCheck())
                                {
                                    filteredTypes = FilterTypes(types, classFilter, namespaceFilter);
                                }

                                if (filteredTypes != null)
                                {
                                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                                    string currentNamespace = null;
                                    foreach (var type in filteredTypes)
                                    {
                                        if (currentNamespace != type.Namespace)
                                        {
                                            currentNamespace = type.Namespace;
                                            EditorGUILayout.LabelField(currentNamespace, EditorStyles.boldLabel);
                                        }

                                        if (GUILayout.Button(type.GetFriendlyName()))
                                        {
                                            newTypes.Add(type);
                                        }
                                    }

                                    EditorGUILayout.EndScrollView();
                                }
                            }

                            break;
                        }
                    }
                    
                    serializedObject.ApplyModifiedProperties();
                },

                keywords = new HashSet<string>(new[] { "Event", "Events", "EventSystem", "lisandroct" })
            };

            return provider;
        }
        
        private static void CreateFolders()
        {
            CoreFolders.Create();
            
            if (!AssetDatabase.IsValidFolder(LibraryPath))
            {
                AssetDatabase.CreateFolder(CoreFolders.CorePath, Library);
            }

            if (AssetDatabase.IsValidFolder(EventsPath))
            {
                FileUtil.DeleteFileOrDirectory(EventsPath);
            }
            if (AssetDatabase.IsValidFolder(ListenersPath))
            {
                FileUtil.DeleteFileOrDirectory(ListenersPath);
            }
            
            AssetDatabase.Refresh();
            
            if (!AssetDatabase.IsValidFolder(EventsPath))
            {
                AssetDatabase.CreateFolder(LibraryPath, Events);
            }
            if (!AssetDatabase.IsValidFolder(ListenersPath))
            {
                AssetDatabase.CreateFolder(LibraryPath, Listeners);
            }
            
            if (!AssetDatabase.IsValidFolder(EventsInspectorsPath))
            {
                AssetDatabase.CreateFolder(EventsPath, Editor);
            }
        }

        private static IEnumerable<Type> LoadTypes()
        {
            IEnumerable<string> assemblies = CompilationPipeline
                .GetAssemblies(AssembliesType.PlayerWithoutTestAssemblies)
                .SelectMany(assembly => new[] {assembly.outputPath}.Concat(assembly.allReferences))
                .Select(a => a.Replace('/', '\\').Split('\\').Last());

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.IsDynamic || assemblies.Contains(assembly.Location.Split('\\').Last()))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(IsNotGeneric)
                .OrderBy(type => type.Namespace)
                .ThenBy(type => type.GetFriendlyName())
                .ToArray();
        }

        private static IEnumerable<Type> FilterTypes(IEnumerable<Type> types, string classFilter, string namespaceFilter)
        {
            if (types == null || (string.IsNullOrEmpty(classFilter) && string.IsNullOrEmpty(namespaceFilter)))
            {
                return null;
            }
            
            return types.Where(type =>
            {
                var typeName = type.GetFriendlyName().ToLowerInvariant();
                var typeNamespace = string.IsNullOrEmpty(type.Namespace) ? "" : type.Namespace.ToLowerInvariant();
                namespaceFilter = string.IsNullOrEmpty(namespaceFilter) ? "" : namespaceFilter.ToLowerInvariant();

                if (string.IsNullOrEmpty(classFilter))
                {
                    return typeNamespace.StartsWith(namespaceFilter);
                }
                
                classFilter = string.IsNullOrEmpty(classFilter) ? "" : classFilter.ToLowerInvariant();

                return typeName.StartsWith(classFilter) && typeNamespace.StartsWith(namespaceFilter);

            });
        }

        private static bool IsNotGeneric(Type type) => !type.IsGenericType;

        private static string ConcatenateTypeNames(IEnumerable<Type> types)
        {
            var builder = new StringBuilder();
            foreach (var type in types)
            {
                builder.Append(type.GetFriendlyName());
            }

            return builder.ToString();
        }
    }
}