using System;
using System.Text;
using UnityEngine;
using System.Linq;


namespace lisandroct.EventSystem
{
    [Serializable]
    public class EventDefinition
    {
        [SerializeField]
        private string _name;
        public string Name => _name;
        
        [SerializeField]
        private SerializableType[] _types;
        private SerializableType[] Types => _types; 

        public EventDefinition(string name, params Type[] types)
        {
            _name = name;

            _types = types.Select(type => new SerializableType(type)).ToArray();
        }

        public Type[] GetTypes() => Types.Select(type => type.Type).ToArray();

        public bool IsValid => Types.All(serializedType => serializedType.Type != null);

        public override string ToString()
        {
            var builder = new StringBuilder(Name);

            if (Types == null || Types.Length <= 0) return builder.ToString();
            
            builder.Append(": (");
            for(int i = 0, n = Types.Length; i < n; i++)
            {
                builder.Append(Types[i].Type.GetFriendlyName());
                if (i < n - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}