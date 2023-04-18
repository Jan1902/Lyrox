using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Lyrox.Framework.CodeGeneration
{
    [Generator]
    public class PacketDeserializeGenerator : ISourceGenerator
    {
        private const string DeserializeMethodName = "Deserialize";
        private const string SerializeMethodName = "Serialize";
        private const string ReaderName = "reader";
        private const string WriterName = "writer";
        private const string ReaderType = "IMojangBinaryReader";
        private const string WriterType = "IMojangBinaryWriter";
        private const string FileSuffix = "_AutoParsing.g.cs";
        private const string AttributeName = "AutoParsedAttribute";
        private const string PacketParserInterfaceName = "IPacketParser";

        private static readonly string[] customTypes = new[] { "VarInt", "Position" };

        public void Execute(GeneratorExecutionContext context)
        {
            var recordDeclerations = context.Compilation.SyntaxTrees
                .SelectMany(st => st.GetRoot()
                    .DescendantNodes()
                    .Where(n => n is RecordDeclarationSyntax)
                    .Select(n => n as RecordDeclarationSyntax));

            foreach (var recordDecleration in recordDeclerations)
            {
                var semanticModel = context.Compilation.GetSemanticModel(recordDecleration.SyntaxTree);

                if (semanticModel.GetDeclaredSymbol(recordDecleration).GetAttributes().Any(a => a.AttributeClass.Name == AttributeName))
                {
                    var source = GenerateClass(context.Compilation.GetSemanticModel(recordDecleration.SyntaxTree), recordDecleration);

                    File.WriteAllText($@"C:\Users\heinbokel\Desktop\{recordDecleration.Identifier.ValueText}{FileSuffix}", source);

                    context.AddSource($"{recordDecleration.Identifier.ValueText}{FileSuffix}", source);
                }
            }
        }

        public string GenerateClass(SemanticModel semanticModel, RecordDeclarationSyntax packetDecleration)
        {
            var baseList = BaseList(SingletonSeparatedList<BaseTypeSyntax>(
                SimpleBaseType(
                    GenericName(Identifier(PacketParserInterfaceName))
                        .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList<TypeSyntax>(
                            IdentifierName(packetDecleration.Identifier.ValueText)))))));

            var newClass = ClassDeclaration($"{packetDecleration.Identifier.ValueText}_Parsing")
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithBaseList(baseList)
                .AddMembers(GenerateDeserializeMethod(semanticModel, packetDecleration),
                    GenerateSerializeMethod(semanticModel, packetDecleration));

            var newNamespace = NamespaceDeclaration(
                    IdentifierName(semanticModel.GetDeclaredSymbol(packetDecleration).ContainingNamespace.ToDisplayString()))
                .AddMembers(newClass);

            var compilationUnit = CompilationUnit()
                .AddUsings(UsingDirective(ParseName("Lyrox.Framework.Networking.Mojang.Data.Abstraction")),
                    UsingDirective(ParseName("Lyrox.Framework.CodeGeneration.Shared")))
                .AddMembers(newNamespace);

            return compilationUnit.NormalizeWhitespace().ToFullString();
        }

        private MethodDeclarationSyntax GenerateSerializeMethod(SemanticModel semanticModel, RecordDeclarationSyntax packetDecleration)
            => MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), SerializeMethodName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithParameterList(ParameterList(SeparatedList(
                    new ParameterSyntax[] {
                        Parameter(Identifier(WriterName))
                            .WithType(IdentifierName(WriterType)),
                        Parameter(Identifier("packet"))
                            .WithType(IdentifierName(packetDecleration.Identifier)) 
                    })))
                .WithBody(Block());

        private MethodDeclarationSyntax GenerateDeserializeMethod(SemanticModel semanticModel, RecordDeclarationSyntax packetDecleration)
        {
            var assignments = packetDecleration.ParameterList.Parameters
                .SelectMany(p => GenerateAssignments(semanticModel, p));

            var creationArgumentList = packetDecleration.ParameterList.Parameters
                .Select(p => Argument(IdentifierName(GetLowerCasedParameterName(p))))
                .ToList();

            var returnStatement = ReturnStatement(
                ImplicitObjectCreationExpression()
                    .WithArgumentList(ArgumentList(SeparatedList(creationArgumentList))));

            var parameterList = ParameterList(
                SingletonSeparatedList(Parameter(Identifier(ReaderName))
                    .WithType(IdentifierName(ReaderType))));

            var method = MethodDeclaration(IdentifierName(packetDecleration.Identifier), DeserializeMethodName)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithParameterList(parameterList)
                .WithBody(Block(assignments
                    .Concat(new StatementSyntax[] { returnStatement })));

            return method;
        }

        private IEnumerable<StatementSyntax> GenerateAssignments(SemanticModel semanticModel, ParameterSyntax parameter)
        {
            var isOptional = parameter.AttributeLists.Any(l => l.Attributes.Any(a => a.Name.ToString() == "Optional"));
            var isLengthPrefixed = parameter.AttributeLists.Any(l => l.Attributes.Any(a => a.Name.ToString() == "LengthPrefixed"));

            var customTypeName = parameter.AttributeLists.SelectMany(l => l.Attributes)
                .FirstOrDefault(a => customTypes.Contains(a.Name.ToString()))?.Name.ToString();

            var lowerCaseParameterName = GetLowerCasedParameterName(parameter);

            var isArray = false;
            var statements = new List<StatementSyntax>();

            if (parameter.Type is ArrayTypeSyntax arrayType)
            {
                isArray = true;
                customTypeName = semanticModel.GetTypeInfo(arrayType.ElementType).Type.Name;
            }

            var typeKeyword = customTypeName ?? (parameter.Type is PredefinedTypeSyntax p ? p.Keyword.ToString() : semanticModel.GetTypeInfo(parameter.Type).Type.Name);

            ExpressionSyntax readerCall = GenerateReaderCall(typeKeyword, isLengthPrefixed ? "int" : null);

            if (isOptional)
            {
                readerCall = ConditionalExpression(
                    GenerateReaderCall("bool"),
                    readerCall,
                    LiteralExpression(SyntaxKind.DefaultLiteralExpression, Token(SyntaxKind.DefaultKeyword)));
            }

            if (isArray)
            {
                var lengthName = lowerCaseParameterName + "Length";

                statements.Add(GenerateVariableDecleration(lengthName, GenerateReaderCall("int")));

                var arrayDecleration = GenerateVariableDecleration(lowerCaseParameterName,
                    ArrayCreationExpression(ArrayType(IdentifierName(typeKeyword))
                        .WithRankSpecifiers(
                            SingletonList(ArrayRankSpecifier(
                                    SingletonSeparatedList<ExpressionSyntax>(
                                        IdentifierName(lengthName)))))));

                statements.Add(arrayDecleration);

                var arrayAssignment = AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    ElementAccessExpression(
                        IdentifierName(lowerCaseParameterName))
                    .WithArgumentList(BracketedArgumentList(SingletonSeparatedList(Argument(IdentifierName("i"))))),
                    readerCall);

                statements.Add(GenerateSimpleForLoop(lengthName, ExpressionStatement(arrayAssignment), "i"));
            }
            else
            {
                statements.Add(GenerateVariableDecleration(lowerCaseParameterName, readerCall));
            }

            return statements;
        }


        private LocalDeclarationStatementSyntax GenerateVariableDecleration(string name, ExpressionSyntax value)
            => LocalDeclarationStatement(
                VariableDeclaration(ParseTypeName("var"))
                    .WithVariables(SingletonSeparatedList(
                        VariableDeclarator(name)
                            .WithInitializer(EqualsValueClause(value)))));

        private ForStatementSyntax GenerateSimpleForLoop(string lengthName, StatementSyntax statement, string variableName)
            => ForStatement(statement)
                .WithDeclaration(
                    VariableDeclaration(
                        PredefinedType(
                            Token(SyntaxKind.IntKeyword)))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                Identifier(variableName))
                            .WithInitializer(
                                EqualsValueClause(
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(0)))))))
                .WithCondition(
                    BinaryExpression(
                        SyntaxKind.LessThanExpression,
                        IdentifierName(variableName),
                        IdentifierName(lengthName)))
                .WithIncrementors(
                    SingletonSeparatedList<ExpressionSyntax>(
                        PostfixUnaryExpression(
                            SyntaxKind.PostIncrementExpression,
                            IdentifierName(variableName))));

        private InvocationExpressionSyntax GenerateReaderCall(string keyword, string lengthType = null)
        {
            IdentifierNameSyntax method = null;
            keyword = char.ToLower(keyword[0]) + keyword.Substring(1);

            switch (keyword)
            {
                case "string" when lengthType != null:
                    method = IdentifierName("ReadStringWithLength");
                    break;

                case "int":
                case "long":
                case "short":
                case "bool":
                case "string":
                case "byte":
                case "varInt":
                case "position":
                    method = IdentifierName($"Read{char.ToUpper(keyword[0])}{keyword.Substring(1)}");
                    break;

                case "uint":
                case "ulong":
                case "ushort":
                    method = IdentifierName($"Read{char.ToUpper(keyword[0])}{char.ToUpper(keyword[1])}{keyword.Substring(2)}");
                    break;
            }

            var invocation = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(ReaderName),
                    method ?? throw new Exception($"Invalid Type {keyword}")));

            if (lengthType != null)
                invocation = invocation.WithArgumentList(ArgumentList(SingletonSeparatedList(
                        Argument(GenerateReaderCall(lengthType)))));

            return invocation;
        }

        private string GetLowerCasedParameterName(ParameterSyntax parameter)
            => char.ToLower(parameter.Identifier.ValueText[0]) + parameter.Identifier.ValueText.Substring(1);

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
