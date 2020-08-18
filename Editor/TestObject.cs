using UnityEngine;

namespace lisandroct.EventSystem
{
    public abstract class TestObject<T> : ScriptableObject
    {
        [SerializeField]
        private T argument0;
        public T Argument0 => argument0;
    }
    
    public abstract class TestObject<T, U> : ScriptableObject
    {
        [SerializeField]
        private T argument0;
        public T Argument0 => argument0;
        
        [SerializeField]
        private U argument1;
        public U Argument1 => argument1;
    }
    
    public abstract class TestObject<T, U, V> : ScriptableObject
    {
        [SerializeField]
        private T argument0;
        public T Argument0 => argument0;
        
        [SerializeField]
        private U argument1;
        public U Argument1 => argument1;
        
        [SerializeField]
        private V argument2;
        public V Argument2 => argument2;
    }
    
    public abstract class TestObject<T, U, V, W> : ScriptableObject
    {
        [SerializeField]
        private T argument0;
        public T Argument0 => argument0;
        
        [SerializeField]
        private U argument1;
        public U Argument1 => argument1;
        
        [SerializeField]
        private V argument2;
        public V Argument2 => argument2;
        
        [SerializeField]
        private W argument3;
        public W Argument3 => argument3;
    }
}