using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;

namespace lisandroct.EventSystem
{
    public class CodeGenerator
    {
        private string EventsPath { get; }
        private string ListenersPath { get; }
        
        public CodeGenerator(string eventsPath, string listenersPath)
        {
            EventsPath = eventsPath;
            ListenersPath = listenersPath;
        }
        
        public void Generate(string name, params Type[] types)
        {
            if (types.Length > 4)
            {
                throw new ArgumentException("More than 4 generic arguments are not allowed");
            }
            
            var eventUnit = GenerateEvent(name, types);
            var listenerUnit = GenerateListener(name, types);

            GenerateCsFile(EventsPath, eventUnit);
            GenerateCsFile(ListenersPath, listenerUnit);
        }
        
        private static CodeCompileUnit GenerateEvent(string name, Type[] types)
        {
            var className = $"{name}Event";
            
            var compileUnit = CreateCompileUnit();
            var codeNamespace = AddNamespace(compileUnit);
            var codeClass = AddClass(className, codeNamespace);
            AddParentType(typeof(Event), types, codeClass);
            AddAnnotation(typeof(CreateAssetMenuAttribute), codeClass, ("fileName", $"On{className}"), ("menuName", $"Events/{name} Event"));
            
            return compileUnit;
        }

        private static CodeCompileUnit GenerateListener(string name, Type[] types)
        {
            var className = $"{name}Listener";

            var compileUnit = CreateCompileUnit();
            var codeNamespace = AddNamespace(compileUnit);
            var codeClass = AddClass(className, codeNamespace);
            AddParentType(typeof(Listener), types, codeClass);
            
            return compileUnit;
        }

        private static CodeCompileUnit CreateCompileUnit() => new CodeCompileUnit();

        private static CodeNamespace AddNamespace(CodeCompileUnit compileUnit)
        {
            var codeNamespace = new CodeNamespace("lisandroct.EventSystem.Events");
            compileUnit.Namespaces.Add(codeNamespace);

            return codeNamespace;
        }

        private static CodeTypeDeclaration AddClass(string name, CodeNamespace codeNamespace)
        {
            var codeClass = new CodeTypeDeclaration(name);
            codeNamespace.Types.Add(codeClass);

            return codeClass;
        }

        private static void AddParentType(Type type, IEnumerable<Type> generics, CodeTypeDeclaration codeClass)
        {
            var baseType = new CodeTypeReference(type);
            foreach (var generic in generics)
            {
                baseType.TypeArguments.Add(new CodeTypeReference(generic, CodeTypeReferenceOptions.GenericTypeParameter));
            }

            codeClass.BaseTypes.Add(baseType);
        }

        private static void AddAnnotation(Type type, CodeTypeDeclaration codeClass, params (string, string)[] arguments)
        {
            var createAssetMenuAnnotation = new CodeAttributeDeclaration(new CodeTypeReference(type));
            foreach (var (name, value) in arguments)
            {
                createAssetMenuAnnotation.Arguments.Add(new CodeAttributeArgument(name, new CodePrimitiveExpression(value)));
            }
            
            codeClass.CustomAttributes.Add(createAssetMenuAnnotation);
        }

        private static void GenerateCsFile(string folder, CodeCompileUnit compileUnit)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            string name = compileUnit.Namespaces[0].Types[0].Name;
            string folderPath = $"{folder}/{name}";
            string path = provider.FileExtension[0] == '.' ? $"{folderPath}{provider.FileExtension}" : $"{folderPath}.{provider.FileExtension}";
            
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");

                provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());

                tw.Close();
            }
            
            AssetDatabase.Refresh();
        }
    }
}