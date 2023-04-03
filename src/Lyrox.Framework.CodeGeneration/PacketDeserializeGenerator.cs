using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lyrox.Framework.CodeGeneration
{
    [Generator]
    public class PacketDeserializeGenerator : ISourceGenerator
    {
        private const string AutoParsedAttributeName = "AutoParsed";
        private const string ParseMethodName = "Parse";
        private const string ReaderName = "Reader";
        private const string FileSuffix = "_AutoParsing.g.cs";

        public void Execute(GeneratorExecutionContext context)
        {
            var recordDeclerationsWithAutoParsed = context.Compilation.SyntaxTrees
                .SelectMany(st => st.GetRoot()
                    .DescendantNodes()
                    .Where(n => n is RecordDeclarationSyntax)
                    .Select(n => n as RecordDeclarationSyntax)
                    .Where(r => r.AttributeLists
                        .SelectMany(al => al.Attributes)
                        .Any(a => a.Name.GetText().ToString() == AutoParsedAttributeName)));

            foreach (var recordDecleration in recordDeclerationsWithAutoParsed)
                context.AddSource($"{recordDecleration.Identifier.ValueText}{FileSuffix}",
                    GenerateRecord(context.Compilation.GetSemanticModel(recordDecleration.SyntaxTree), recordDecleration));
        }

        public string GenerateRecord(SemanticModel semanticModel, RecordDeclarationSyntax packetDecleration)
        {
            var assignments = packetDecleration.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                .Select(d => GeneratePropertyAssignment(semanticModel, d));

            var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), ParseMethodName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.OverrideKeyword))
                .WithBody(SyntaxFactory.Block(assignments));

            var newRecord = SyntaxFactory.RecordDeclaration(SyntaxFactory.Token(SyntaxKind.RecordKeyword), packetDecleration.Identifier.ValueText)
                .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                .AddMembers(method)
                .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken));

            var newNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(packetDecleration.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault().Name.ToFullString()))
                .AddMembers(newRecord);

            var syntaxFactory = SyntaxFactory.CompilationUnit()
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                .AddMembers(newNamespace);

            return syntaxFactory.NormalizeWhitespace().ToFullString();
        }

        private StatementSyntax GeneratePropertyAssignment(SemanticModel semanticModel, PropertyDeclarationSyntax propertyDeclaration)
        {
            StatementSyntax assignment = SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(propertyDeclaration.Identifier),
                    GenerateReaderCall(Type.GetType(semanticModel.GetDeclaredSymbol(propertyDeclaration).Type.ToDisplayString()))));

            if(propertyDeclaration.AttributeLists.Single().Attributes.Any(a => a.Name.ToString() == "AutoParsed"))
                assignment = SyntaxFactory.IfStatement(GenerateReaderCall(typeof(bool)), assignment);

            return assignment;
        }

        private InvocationExpressionSyntax GenerateReaderCall(Type type)
        {
            IdentifierNameSyntax method = null;

            switch(type)
            {
                case Type t when t == typeof(int):
                    method = SyntaxFactory.IdentifierName("ReadInt");
                    break;
                case Type t when t == typeof(bool):
                    method = SyntaxFactory.IdentifierName("ReadBool");
                    break;
            }

            return SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(ReaderName),
                    method ?? throw new Exception($"Invalid Type t {type.ToString()}")));
        }

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
