﻿using System;
using UnityEditor;
using UnityEngine;

namespace lisandroct.EventSystem
{
    [CustomEditor(typeof(GameEvent))]
    public class EventInspector : Editor
    {
        private GameEvent Event => target as GameEvent;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Raise"))
            {
                Event.Invoke();
            }
        }
    }
    
    public class EventInspector<T, S> : Editor where S : TestObject<T>
    {
        private GameEvent<T> Event => target as GameEvent<T>;

        private S TestObject { get; set; }
        private SerializedObject SerializedTestObject { get; set; }
        private SerializedProperty Argument0Property { get; set; }
        
        private void OnEnable()
        {
            TestObject = (S) CreateInstance(typeof(S));
            
            SerializedTestObject = new SerializedObject(TestObject);
            
            Argument0Property = SerializedTestObject.FindProperty("argument0");
        }

        private void OnDisable()
        {
            DestroyImmediate(TestObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (!Application.isPlaying) return;
            
            EditorGUILayout.Space();

            SerializedTestObject.Update();
            EditorGUILayout.PropertyField(Argument0Property);
            SerializedTestObject.ApplyModifiedPropertiesWithoutUndo();

            EditorGUILayout.Space();
            
            if (GUILayout.Button("Raise"))
            {
                Event.Invoke(TestObject.Argument0);
            }
        }
    }
    
    public abstract class EventInspector<T, U, S> : Editor where S : TestObject<T, U>
    {
        private GameEvent<T, U> Event => target as GameEvent<T, U>;

        private S TestObject { get; set; }
        private SerializedObject SerializedTestObject { get; set; }
        private SerializedProperty Argument0Property { get; set; }
        private SerializedProperty Argument1Property { get; set; }
        
        private void OnEnable()
        {
            TestObject = (S) CreateInstance(typeof(S));
            
            SerializedTestObject = new SerializedObject(TestObject);
            
            Argument0Property = SerializedTestObject.FindProperty("argument0");
            Argument1Property = SerializedTestObject.FindProperty("argument1");
        }

        private void OnDisable()
        {
            DestroyImmediate(TestObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;
            
            EditorGUILayout.Space();

            SerializedTestObject.Update();
            EditorGUILayout.PropertyField(Argument0Property);
            EditorGUILayout.PropertyField(Argument1Property);
            SerializedTestObject.ApplyModifiedPropertiesWithoutUndo();
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Raise"))
            {
                Event.Invoke(TestObject.Argument0, TestObject.Argument1);
            }
        }
    }
    
    public abstract class EventInspector<T, U, V, S> : Editor where S : TestObject<T, U, V>
    {
        private GameEvent<T, U, V> Event => target as GameEvent<T, U, V>;

        private S TestObject { get; set; }
        private SerializedObject SerializedTestObject { get; set; }
        private SerializedProperty Argument0Property { get; set; }
        private SerializedProperty Argument1Property { get; set; }
        private SerializedProperty Argument2Property { get; set; }
        
        private void OnEnable()
        {
            TestObject = (S) CreateInstance(typeof(S));
            
            SerializedTestObject = new SerializedObject(TestObject);
            
            Argument0Property = SerializedTestObject.FindProperty("argument0");
            Argument1Property = SerializedTestObject.FindProperty("argument1");
            Argument2Property = SerializedTestObject.FindProperty("argument2");
        }

        private void OnDisable()
        {
            DestroyImmediate(TestObject);
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;
            
            EditorGUILayout.Space();

            SerializedTestObject.Update();
            EditorGUILayout.PropertyField(Argument0Property);
            EditorGUILayout.PropertyField(Argument1Property);
            EditorGUILayout.PropertyField(Argument2Property);
            SerializedTestObject.ApplyModifiedPropertiesWithoutUndo();
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Raise"))
            {
                Event.Invoke(TestObject.Argument0, TestObject.Argument1, TestObject.Argument2);
            }
        }
    }
    
    public abstract class EventInspector<T, U, V, W, S> : Editor where S : TestObject<T, U, V, W>
    {
        private GameEvent<T, U, V, W> Event => target as GameEvent<T, U, V, W>;

        private S TestObject { get; set; }
        private SerializedObject SerializedTestObject { get; set; }
        private SerializedProperty Argument0Property { get; set; }
        private SerializedProperty Argument1Property { get; set; }
        private SerializedProperty Argument2Property { get; set; }
        private SerializedProperty Argument3Property { get; set; }
        
        private void OnEnable()
        {
            TestObject = (S) CreateInstance(typeof(S));
            
            SerializedTestObject = new SerializedObject(TestObject);
            
            Argument0Property = SerializedTestObject.FindProperty("argument0");
            Argument1Property = SerializedTestObject.FindProperty("argument1");
            Argument2Property = SerializedTestObject.FindProperty("argument2");
            Argument3Property = SerializedTestObject.FindProperty("argument3");
        }

        private void OnDisable()
        {
            DestroyImmediate(TestObject);
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;
            
            EditorGUILayout.Space();

            SerializedTestObject.Update();
            EditorGUILayout.PropertyField(Argument0Property);
            EditorGUILayout.PropertyField(Argument1Property);
            EditorGUILayout.PropertyField(Argument2Property);
            EditorGUILayout.PropertyField(Argument3Property);
            SerializedTestObject.ApplyModifiedPropertiesWithoutUndo();
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Raise"))
            {
                Event.Invoke(TestObject.Argument0, TestObject.Argument1, TestObject.Argument2, TestObject.Argument3);
            }
        }
    }
}