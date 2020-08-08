using System;
using System.Collections.Generic;
using lisandroct.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace lisandroct.EventSystem
{
    public class Settings : ScriptableObject
    {
        private const string FileName = "EventSystem-settings.asset";
        
        [SerializeField]
        private List<SerializableType> _types = new List<SerializableType>();
        public List<SerializableType> Types => _types;
        
        private HashSet<Type> _set;
        private HashSet<Type> Set
        {
            get
            {
                if (_set == null)
                {
                    _set = new HashSet<Type>();
                    foreach (var type in Types)
                    {
                        _set.Add(type.Type);
                    }
                }
                
                return _set;
            }
        }
        
        private static Settings GetOrCreateSettings()
        {
            CoreFolders.Create();

            var path = $"{CoreFolders.SettingsPath}/{FileName}";
            
            var settings = AssetDatabase.LoadAssetAtPath<Settings>(path);
            if (settings != null) return settings;
            
            settings = CreateInstance<Settings>();
            
            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();

            return settings;
        }

        internal static SerializedObject GetSerializedObject() => new SerializedObject(GetOrCreateSettings());

        public void AddType(Type type)
        {
            if(IsSet(type))
            {
                return;
            }
            
            Set.Add(type);
            Types.Add(new SerializableType(type));
        }

        public void RemoveType(Type type)
        {
            if(!IsSet(type))
            {
                return;
            }
            
            Set.Remove(type);
            var index = _types.FindIndex((t) => t.Type == type);
            if (index >= 0)
            {
                Types.RemoveAt(index);
            }
        }

        public bool IsSet(Type type) => Set.Contains(type);
    }
}