using UnityEditor;
using UnityEngine;

namespace lisandroct.EventSystem
{
    [CustomEditor(typeof(Event))]
    public class EventInspector : Editor
    {
        private Event Target => target as Event;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Raise"))
            {
                Target.Raise();
            }
        }
    }
}