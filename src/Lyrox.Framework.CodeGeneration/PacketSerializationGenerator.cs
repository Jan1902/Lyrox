using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lyrox.Framework.CodeGeneration
{
    [Generator]
    public class PacketSerializationGenerator : ISourceGenerator
    {
        private const string ReaderName = "reader";
        private const string FileSuffix = "_AutoSerialization.g.cs";
        private const string AutoSerializedAttributeName = "AutoSerializedAttribute";
        private const string CustomSerializedAttributeName = "CustomSerializedAttribute";

        private static readonly string[] customTypes = new[] { "VarInt", "Position" };

        public void Execute(GeneratorExecutionContext context)
        {
            var recordDeclerations = context.Compilation.SyntaxTrees
                .SelectMany(st => st.GetRoot()
                    .DescendantNodes()
                    .OfType<RecordDeclarationSyntax>());

            var mappings = new List<(string PacketTypeName, (string SerializerTypeName, string Namespace) Serializer)>();

            foreach (var recordDecleration in recordDeclerations)
            {
                var semanticModel = context.Compilation.GetSemanticModel(recordDecleration.SyntaxTree);

                if (semanticModel.GetDeclaredSymbol(recordDecleration).GetAttributes().Any(a => a.AttributeClass.Name == AutoSerializedAttributeName))
                {
                    var source = GenerateClass(semanticModel, recordDecleration);
                    source = CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().ToFullString();

                    context.AddSource($"{recordDecleration.Identifier.ValueText}{FileSuffix}", source);

                    mappings.Add((recordDecleration.Identifier.ValueText, (recordDecleration.Identifier.ValueText + "_Serialization",
                        semanticModel.GetDeclaredSymbol(recordDecleration).ContainingNamespace.ToDisplayString())));
                }
                else
                {
                    var customSerializedAttribute = semanticModel.GetDeclaredSymbol(recordDecleration).GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == CustomSerializedAttributeName);

                    mappings.Add((recordDecleration.Identifier.ValueText, (customSerializedAttribute.AttributeClass.TypeParameters.Single().Name,
                        semanticModel.GetDeclaredSymbol(recordDecleration).ContainingNamespace.ToDisplayString())));
                }
            }

            if (mappings.Any())
            {
                var mappingSource = CodeTemplates.SerializerMappingSkeleton;
                mappingSource = ReplacePlaceholder(mappingSource, "namespace", mappings.First().Serializer.Namespace);
                mappingSource = ReplacePlaceholder(mappingSource, "mappingcontent", string.Join("\n", mappings.Select(m => $"{{ {m.PacketTypeName}, {m.Serializer.SerializerTypeName} }}")));

                context.AddSource("SerializerMappings" + FileSuffix, mappingSource);
            }
        }

        public string GenerateClass(SemanticModel semanticModel, RecordDeclarationSyntax packetDecleration)
        {
            var classCode = CodeTemplates.SerializerSkeleton;

            Placeholder("packetname", packetDecleration.Identifier.ValueText);
            Placeholder("namespace", semanticModel.GetDeclaredSymbol(packetDecleration).ContainingNamespace.ToDisplayString());

            Placeholder("deserializecontent", GenerateDeserializeMethod(semanticModel, packetDecleration));
            Placeholder("serializecontent", GenerateSerializeMethod(semanticModel, packetDecleration));

            void Placeholder(string placeholder, string value)
                => classCode = ReplacePlaceholder(classCode, placeholder, value);

            return classCode;
        }

        private string GenerateSerializeMethod(SemanticModel semanticModel, RecordDeclarationSyntax packetDecleration)
        {
            return "";
        }

        private string GenerateDeserializeMethod(SemanticModel semanticModel, RecordDeclarationSyntax packetDecleration)
        {
            var deserializeMethodContent = "";
            var parameterNames = new List<string>();

            foreach (var parameter in packetDecleration.ParameterList.Parameters)
            {
                var isOptional = parameter.AttributeLists.Any(l => l.Attributes.Any(a => a.Name.ToString() == "Optional"));
                var isLengthPrefixed = parameter.AttributeLists.Any(l => l.Attributes.Any(a => a.Name.ToString() == "LengthPrefixed"));

                var customTypeName = parameter.AttributeLists.SelectMany(l => l.Attributes)
                    .FirstOrDefault(a => customTypes.Contains(a.Name.ToString()))?.Name.ToString();

                var lowerCaseParameterName = GetLowerCasedParameterName(parameter);
                parameterNames.Add(lowerCaseParameterName);

                var isArray = false;
                var statements = new List<string>();

                if (parameter.Type is ArrayTypeSyntax arrayType)
                {
                    isArray = true;
                    customTypeName = arrayType.ElementType is PredefinedTypeSyntax pt ? pt.Keyword.ToString() : semanticModel.GetTypeInfo(arrayType.ElementType).Type.Name;
                }

                var typeKeyword = customTypeName ?? (parameter.Type is PredefinedTypeSyntax p ? p.Keyword.ToString() : semanticModel.GetTypeInfo(parameter.Type).Type.Name);
                var value = GetReaderCall(typeKeyword, isLengthPrefixed ? "VarInt" : null);

                if (isArray)
                {
                    if (typeKeyword == "byte")
                        typeKeyword = "bytes";
                    else
                    {
                        statements.Add($"var {lowerCaseParameterName} = new {typeKeyword}[{GetReaderCall("VarInt")}];");
                        statements.Add($"for (int i = 0; i < {lowerCaseParameterName}.Length; i++)");
                        statements.Add($"{{ {lowerCaseParameterName}[i] = {value}; }}");
                    }
                }
                else
                {
                    statements.Add($"var {lowerCaseParameterName} = {value};");
                }

                if (isOptional && !isArray)
                    value = $"{GetReaderCall("bool")} ? {value} : default";

                deserializeMethodContent += $"\n{string.Join("\n", statements)}";
            }

            deserializeMethodContent += $"\nreturn new({string.Join(", ", parameterNames)});";
            return deserializeMethodContent;
        }

        private string GetReaderCall(string keyword, string lengthType = null)
        {
            string method = null;
            keyword = char.ToLower(keyword[0]) + keyword.Substring(1);

            switch (keyword)
            {
                case "string" when lengthType != null:
                    method = "ReadStringWithLength";
                    break;

                case "guid":
                    method = "ReadUUID";
                    break;

                case "string":
                    method = "ReadStringWithVarIntPrefix";
                    break;

                case "int":
                case "long":
                case "short":
                case "bool":
                case "byte":
                case "varInt":
                case "position":
                    method = $"Read{char.ToUpper(keyword[0])}{keyword.Substring(1)}";
                    break;

                case "uint":
                case "ulong":
                case "ushort":
                    method = $"Read{char.ToUpper(keyword[0])}{char.ToUpper(keyword[1])}{keyword.Substring(2)}";
                    break;
            }

            return $"{ReaderName}.{method}({(lengthType != null ? GetReaderCall(lengthType) : "")})";
        }

        private static string GetLowerCasedParameterName(ParameterSyntax parameter)
            => char.ToLower(parameter.Identifier.ValueText[0]) + parameter.Identifier.ValueText.Substring(1);

        private static string ReplacePlaceholder(string text, string placeholder, string value)
            => text.Replace($"{{{placeholder.ToUpper()}}}", value);

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
