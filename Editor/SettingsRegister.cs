using System;
using System.Collections.Generic;
using System.Linq;
using lisandroct.Core.Editor;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace lisandroct.EventSystem
{
    public class SettingsRegister {
        private const string Library = "EventSystem";
        private const string Events = "Events";
        private const string Listeners = "Listeners";
        
        private static string LibraryPath => $"{CoreFolders.CorePath}/{Library}";
        private static string EventsPath => $"{LibraryPath}/{Events}";
        private static string ListenersPath => $"{LibraryPath}/{Listeners}";
        
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var tabIndex = 0;
            var scrollPosition = Vector2.zero;
            var generator = new CodeGenerator(EventsPath, ListenersPath);

            string namespaceFilter = null;
            string classFilter = null;

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
                    tabIndex = GUILayout.Toolbar(tabIndex, new [] { "Events", "Add New Events" });
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
                            string currentNamespace = null;
                            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                            if (settings.Types != null)
                            {
                                foreach (var sType in settings.Types)
                                {
                                    var type = sType.Type;
                                    if (currentNamespace != type.Namespace)
                                    {
                                        currentNamespace = type.Namespace;
                                        EditorGUILayout.LabelField(currentNamespace, EditorStyles.boldLabel);
                                    }

                                    var toggle = EditorGUILayout.ToggleLeft(type.GetFriendlyName(), true);
                                    if (!toggle)
                                    {
                                        settings.RemoveType(type);
                                        break;
                                    }
                                }
                            }

                            EditorGUILayout.EndScrollView();

                            if (GUILayout.Button("Generate"))
                            {
                                CreateFolders();

                                if (settings.Types != null)
                                {
                                    foreach (var sType in settings.Types)
                                    {
                                        var type = sType.Type;
                                        generator.Generate(type);
                                    }
                                }
                                

                                AssetDatabase.Refresh();
                            }

                            break;
                        }
                        case 1:
                        {
                            EditorGUI.BeginChangeCheck();
                            namespaceFilter = EditorGUILayout.TextField("Namespace", namespaceFilter);
                            classFilter = EditorGUILayout.TextField("Class Name", classFilter);
                            if(EditorGUI.EndChangeCheck())
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

                                    EditorGUI.BeginChangeCheck();
                                    var toggle = EditorGUILayout.ToggleLeft(type.GetFriendlyName(), settings.IsSet(type));
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        if (toggle)
                                        {
                                            settings.AddType(type);
                                        }
                                        else
                                        {
                                            settings.RemoveType(type);
                                        }
                                    }
                                }

                                EditorGUILayout.EndScrollView();
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
            
            AssetDatabase.CreateFolder(LibraryPath, Events);
            AssetDatabase.CreateFolder(LibraryPath, Listeners);
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
                .Where(IsSerializable)
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
        
        private static bool IsSerializable(Type type)
        {
            if (type.IsAbstract)
            {
                return false;
            }
            
            if (type.IsGenericType)
            {
                if (type.GetGenericArguments().Any(genericArgument => !IsSerializable(genericArgument)))
                {
                    return false;
                }
            }

            if (type.IsPrimitive || type.IsEnum || type.IsValueType)
            {
                return true;
            }
            
            var typeObject = typeof(UnityEngine.Object);
            return typeObject.IsAssignableFrom(type) || type.IsSerializable;
        }
    }
}