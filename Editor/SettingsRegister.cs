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
        private const string Tests = "Tests";
        private const string Editor = "Editor";
        private const string Listeners = "Listeners";
        
        private static string LibraryPath => $"{CoreFolders.CorePath}/{Library}";
        private static string EventsPath => $"{LibraryPath}/{Events}";
        private static string TestsPath => $"{LibraryPath}/{Tests}";
        private static string EventsInspectorsPath => $"{EventsPath}/{Editor}";
        private static string ListenersPath => $"{LibraryPath}/{Listeners}";

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var tabIndex = 0;
            var scrollPosition = Vector2.zero;
            var generator = new CodeGenerator(EventsPath, TestsPath, EventsInspectorsPath, ListenersPath);

            var namespaceFilter = "";
            var classFilter = "";

            string newName = null;
            var newTypes = new List<Type>();

            IEnumerable<Type> types = null;
            IEnumerable<Type> filteredTypes = null;
            
            var existing = Settings.GetSerializedObject(out var serializedObject);
            var definitionsProperty = serializedObject.FindProperty("definitions");

            var settings = serializedObject.targetObject as Settings;
            if (settings == null)
            {
                return null;
            }

            var provider = new SettingsProvider("Project/Event System", SettingsScope.Project)
            {
                label = "Event System",
                guiHandler = searchContext =>
                {
                    serializedObject.Update();
                    if (!existing)
                    {
                        AddDefaultEvents(settings);
                    }
                    
                    EditorGUI.BeginChangeCheck();
                    tabIndex = GUILayout.Toolbar(tabIndex, new [] { "Event Types", "Add New" });
                    if (EditorGUI.EndChangeCheck())
                    {
                        scrollPosition = Vector2.zero;
                        if (tabIndex == 1)
                        {
                            scrollPosition = Vector2.zero;
                            namespaceFilter = "";
                            classFilter = "";

                            types = LoadTypes();
                            filteredTypes = null;
                        }
                    }
                    
                    switch (tabIndex) {
                        case 0:
                        {
                            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                            for (int i = 0, n = definitionsProperty.arraySize; i < n; i++)
                            {
                                var definition = settings.Definitions[i];

                                if (!definition.IsValid)
                                {
                                    continue;
                                }
                                    
                                if(!EditorGUILayout.ToggleLeft(definition.ToString(), true)) {
                                    definitionsProperty.DeleteArrayElementAtIndex(i);
                                    break;
                                }
                            }

                            EditorGUILayout.EndScrollView();

                            if (GUILayout.Button("Add Default Events"))
                            {
                                AddDefaultEvents(settings);
                            }

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
                                    //var newDefinition = new EventDefinition(targetName, newTypes.ToArray());
                                    var index = definitionsProperty.arraySize;
                                    definitionsProperty.InsertArrayElementAtIndex(index);
                                    var newElement = definitionsProperty.GetArrayElementAtIndex(index);
                                    
                                    var nameProperty = newElement.FindPropertyRelative("_name");
                                    nameProperty.stringValue = targetName;
                                    
                                    var typesProperty = newElement.FindPropertyRelative("_types");
                                    typesProperty.ClearArray();
                                    for(int i = 0, n = newTypes.Count; i < n; i++)
                                    {
                                        var data = new SerializableType(newTypes[i]).GetData();
                                        
                                        typesProperty.InsertArrayElementAtIndex(i);
                                        var newType = typesProperty.GetArrayElementAtIndex(i);
                                        var dataProperty = newType.FindPropertyRelative("data");
                                        dataProperty.ClearArray();
                                        for(int j = 0, m = data.Length; j < m; j++)
                                        {
                                            dataProperty.InsertArrayElementAtIndex(j);
                                            dataProperty.GetArrayElementAtIndex(j).intValue = data[j];
                                        }
                                    }
                                    
                                    newName = null;
                                    newTypes.Clear();
                                }
                            }

                            if (newTypes.Count < 4)
                            {
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField("Search types", EditorStyles.boldLabel);
                                EditorGUI.BeginChangeCheck();
                                var namespaceName = EditorGUILayout.TextField("Namespace", namespaceFilter);
                                var className = EditorGUILayout.TextField("Class Name", classFilter);
                                if (EditorGUI.EndChangeCheck())
                                {
                                    var moreSpecific = className.StartsWith(classFilter) && namespaceName.StartsWith(namespaceFilter);
                                    if (string.IsNullOrEmpty(classFilter) && string.IsNullOrEmpty(namespaceFilter))
                                    {
                                        moreSpecific = false;
                                    }
                                    
                                    classFilter = className;
                                    namespaceFilter = namespaceName;
                                    
                                    filteredTypes = FilterTypes(moreSpecific ? filteredTypes : types, classFilter, namespaceFilter);
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
            if (AssetDatabase.IsValidFolder(TestsPath))
            {
                FileUtil.DeleteFileOrDirectory(TestsPath);
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
            if (!AssetDatabase.IsValidFolder(TestsPath))
            {
                AssetDatabase.CreateFolder(LibraryPath, Tests);
            }
            
            if (!AssetDatabase.IsValidFolder(EventsInspectorsPath))
            {
                AssetDatabase.CreateFolder(EventsPath, Editor);
            }
        }

        private static IEnumerable<Type> LoadTypes()
        {
            var assemblies = CompilationPipeline
                .GetAssemblies(AssembliesType.PlayerWithoutTestAssemblies)
                .SelectMany(assembly => new[] {assembly.outputPath}.Concat(assembly.allReferences))
                .Select(a => a.Replace('/', '\\').Split('\\').Last());

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.IsDynamic || assemblies.Contains(assembly.Location.Replace('/', '\\').Split('\\').Last()))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(IsNotGeneric)
                .OrderBy(type => type.Namespace)
                .ThenBy(type => type.GetFriendlyName())
                .ToArray();
        }

        private static IEnumerable<Type> FilterTypes(IEnumerable<Type> types, string typeFilter, string namespaceFilter)
        {
            if (types == null || (string.IsNullOrEmpty(typeFilter) && string.IsNullOrEmpty(namespaceFilter)))
            {
                return null;
            }
            
            namespaceFilter = string.IsNullOrEmpty(namespaceFilter) ? null : namespaceFilter.ToLowerInvariant();
            typeFilter = string.IsNullOrEmpty(typeFilter) ? null : typeFilter.ToLowerInvariant();

            var ignoreNamespace = namespaceFilter == null;
            var ignoreType = typeFilter == null;
            
            return types.Where(type =>
            {
                if (type == null)
                {
                    return false;
                }
                
                var typeName = type.GetFriendlyName().ToLowerInvariant();
                var typeNamespace = string.IsNullOrEmpty(type.Namespace) ? null : type.Namespace.ToLowerInvariant();

                var namespacePass = ignoreNamespace || typeNamespace != null && typeNamespace.StartsWith(namespaceFilter);
                
                if (ignoreType)
                {
                    return namespacePass;
                }

                return typeName.StartsWith(typeFilter) && namespacePass;
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

        private static void AddDefaultEvents(Settings settings)
        {
            settings.Definitions.Add(new EventDefinition("Bool", typeof(bool)));
            settings.Definitions.Add(new EventDefinition("Int", typeof(int)));
            settings.Definitions.Add(new EventDefinition("Float", typeof(float)));
            settings.Definitions.Add(new EventDefinition("String", typeof(string)));
            settings.Definitions.Add(new EventDefinition("Vector2", typeof(Vector2)));
            settings.Definitions.Add(new EventDefinition("Vector2Int", typeof(Vector2Int)));
            settings.Definitions.Add(new EventDefinition("Vector3", typeof(Vector3)));
            settings.Definitions.Add(new EventDefinition("Vector3Int", typeof(Vector3Int)));
            settings.Definitions.Add(new EventDefinition("Vector4", typeof(Vector4)));
            settings.Definitions.Add(new EventDefinition("Color", typeof(Color)));
        }
    }
}