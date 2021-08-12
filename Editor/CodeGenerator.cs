using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace lisandroct.EventSystem
{
    public class CodeGenerator
    {
        private string EventsPath { get; }
        private string EventsInspectorsPath { get; }
        private string ListenersPath { get; }
        
        public CodeGenerator(string eventsPath, string eventsInspectorsPath, string listenersPath)
        {
            EventsPath = eventsPath;
            EventsInspectorsPath = eventsInspectorsPath;
            ListenersPath = listenersPath;
        }
        
        public void Generate(string name, Type[] types)
        {
            if (types.Length <= 0)
            {
                throw new ArgumentException("There must be at least one type");
            }
            if (types.Length > 4)
            {
                throw new ArgumentException("More than 4 generic arguments are not allowed");
            }
            
            var eventUnit = GenerateEvent(name, types);
            var eventInspectorUnit = GenerateEventInspector(name, types);
            var listenerUnit = GenerateListener(name, types);

            GenerateCsFile(EventsPath, eventUnit);
            GenerateCsFile(EventsInspectorsPath, eventInspectorUnit);
            GenerateCsFile(ListenersPath, listenerUnit);
        }
        
        private static CodeCompileUnit GenerateEvent(string name, Type[] types)
        {
            var className = $"{name}Event";
            
            var compileUnit = CreateCompileUnit();
            var codeNamespace = AddNamespace(compileUnit);
            var codeClass = AddClass(className, codeNamespace);
            AddParentType(codeClass, typeof(GameEvent), types);
            AddAnnotation(typeof(CreateAssetMenuAttribute), codeClass, ("fileName", $"On{className}"), ("menuName", $"Events/{name} Event"));
            
            return compileUnit;
        }
        
        private static CodeCompileUnit GenerateEventInspector(string name, Type[] types)
        {
            var inspectorClassName = $"{name}Inspector";
            var testObjectClassName = $"{name}Test";
            
            var compileUnit = CreateCompileUnit();
            var codeNamespace = AddNamespace(compileUnit);

            var inspectorCodeClass = AddClass(inspectorClassName, codeNamespace);
            var inspectorBaseClass = AddParentType(inspectorCodeClass, typeof(EventInspector), types);
            AddGenerics(inspectorBaseClass, testObjectClassName);
            AddAnnotationWithType(typeof(CustomEditor), inspectorCodeClass, $"{name}Event");

            var testObjectCodeClass = AddClass(testObjectClassName, codeNamespace);
            switch (types.Length)
            {
                case 1:
                    AddParentType(testObjectCodeClass, typeof(TestObject<>), types);
                    break;
                case 2:
                    AddParentType(testObjectCodeClass, typeof(TestObject<,>), types);
                    break;
                case 3:
                    AddParentType(testObjectCodeClass, typeof(TestObject<,,>), types);
                    break;
                case 4:
                    AddParentType(testObjectCodeClass, typeof(TestObject<,,,>), types);
                    break;
            }
            
            return compileUnit;
        }

        private static CodeCompileUnit GenerateListener(string name, Type[] types)
        {
            var compileUnit = CreateCompileUnit();
            var codeNamespace = AddNamespace(compileUnit);
            
            #if UNITY_2020_1_OR_NEWER
            var className = $"{name}Listener";

            var codeClass = AddClass(className, codeNamespace);
            AddParentType(codeClass, typeof(Listener), types);
            #else
            var listenerClassName = $"{name}Listener";
            var responseClassName = $"{name}Response";

            var listenerCodeClass = AddClass(listenerClassName, codeNamespace);
            var listenerBaseClass = AddParentType(listenerCodeClass, typeof(Listener), types);
            AddGenerics(listenerBaseClass, $"{name}Event", responseClassName);

            var responseCodeClass = AddClass(responseClassName, codeNamespace);
            AddParentType(responseCodeClass, typeof(UnityEvent), types);
            AddAnnotation(typeof(SerializableAttribute), responseCodeClass);
            #endif

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

        private static CodeTypeReference AddParentType(CodeTypeDeclaration codeClass, Type baseType, params Type[] generics)
        {
            var baseTypeReference = new CodeTypeReference(baseType);
            AddGenerics(baseTypeReference, generics);

            codeClass.BaseTypes.Add(baseTypeReference);

            return baseTypeReference;
        }

        private static CodeTypeReference AddParentType(CodeTypeDeclaration codeClass, Type baseType, params string[] generics)
        {
            var baseTypeReference = new CodeTypeReference(baseType);
            AddGenerics(baseTypeReference, generics);

            codeClass.BaseTypes.Add(baseTypeReference);

            return baseTypeReference;
        }

        private static void AddGenerics(CodeTypeReference typeReference, params Type[] generics)
        {
            foreach (var generic in generics)
            {
                typeReference.TypeArguments.Add(new CodeTypeReference(generic, CodeTypeReferenceOptions.GenericTypeParameter));
            }
        }

        private static void AddGenerics(CodeTypeReference typeReference, params string[] generics)
        {
            foreach (var generic in generics)
            {
                typeReference.TypeArguments.Add(new CodeTypeReference(generic, CodeTypeReferenceOptions.GenericTypeParameter));
            }
        }

        private static void AddAnnotation(Type type, CodeTypeMember codeMember, params (string, string)[] arguments)
        {
            var annotation = new CodeAttributeDeclaration(new CodeTypeReference(type));
            foreach (var (name, value) in arguments)
            {
                annotation.Arguments.Add(new CodeAttributeArgument(name, new CodePrimitiveExpression(value)));
            }
            
            codeMember.CustomAttributes.Add(annotation);
        }

        private static void AddAnnotationWithType(Type type, CodeTypeDeclaration codeClass, string typeArgument)
        {
            var annotation = new CodeAttributeDeclaration(new CodeTypeReference(type));
            annotation.Arguments.Add(new CodeAttributeArgument(new CodeTypeOfExpression(typeArgument)));

            codeClass.CustomAttributes.Add(annotation);
        }

        private static void GenerateCsFile(string folder, CodeCompileUnit compileUnit)
        {
            var provider = new CSharpCodeProvider();

            var name = compileUnit.Namespaces[0].Types[0].Name;
            var folderPath = $"{folder}/{name}";
            var path = provider.FileExtension[0] == '.' ? $"{folderPath}{provider.FileExtension}" : $"{folderPath}.{provider.FileExtension}";
            
            using (var sw = new StreamWriter(path, false))
            {
                var tw = new IndentedTextWriter(sw, "    ");

                provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());

                tw.Close();
            }
            
            AssetDatabase.Refresh();
        }
    }
}