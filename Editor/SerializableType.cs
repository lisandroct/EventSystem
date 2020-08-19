using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace lisandroct.EventSystem
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField]
        private byte[] data;
        
        public Type Type { get; private set; }

        public SerializableType(Type type) {
            Type = type;
        }

        public static Type Read(BinaryReader reader)
        {
            var paramCount = reader.ReadByte();
            if (paramCount == 0xFF)
                return null;
            var typeName = reader.ReadString();
            var type = Type.GetType(typeName);
            if (type == null)
                return null;
            if (type.IsGenericTypeDefinition && paramCount > 0)
            {
                var p = new Type[paramCount];
                for (int i = 0; i < paramCount; i++)
                {
                    p[i] = Read(reader);
                }

                type = type.MakeGenericType(p);
            }

            return type;
        }

        public static void Write(BinaryWriter writer, Type type)
        {
            if (type == null)
            {
                writer.Write((byte) 0xFF);
                return;
            }

            if (type.IsGenericType)
            {
                var t = type.GetGenericTypeDefinition();
                var p = type.GetGenericArguments();
                writer.Write((byte) p.Length);
                writer.Write(t.AssemblyQualifiedName);
                for (int i = 0; i < p.Length; i++)
                {
                    Write(writer, p[i]);
                }

                return;
            }

            writer.Write((byte) 0);
            writer.Write(type.AssemblyQualifiedName);
        }


        public void OnBeforeSerialize()
        {
            using (var stream = new MemoryStream())
            using (var w = new BinaryWriter(stream))
            {
                Write(w, Type);
                data = stream.ToArray();
            }
        }

        public void OnAfterDeserialize()
        {
            using (var stream = new MemoryStream(data))
            using (var r = new BinaryReader(stream))
            {
                Type = Read(r);
            }
        }
    }
}