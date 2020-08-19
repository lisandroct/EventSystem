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
            
            Debug.Log("New Settings!");
            
            settings = CreateInstance<Settings>();
            
            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();

            return settings;
        }

        internal static SerializedObject GetSerializedObject() => new SerializedObject(GetOrCreateSettings());
    }
}