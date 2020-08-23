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
        private List<EventDefinition> definitions = new List<EventDefinition>();
        public List<EventDefinition> Definitions => definitions;
        
        private static Settings GetOrCreateSettings()
        {
            CoreFolders.Create();

            var path = $"{CoreFolders.SettingsPath}/{FileName}";
            
            var settings = AssetDatabase.LoadAssetAtPath<Settings>(path);
            if (settings != null) return settings;
            
            settings = CreateInstance<Settings>();
            
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
            
            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();

            return settings;
        }

        internal static SerializedObject GetSerializedObject() => new SerializedObject(GetOrCreateSettings());
    }
}