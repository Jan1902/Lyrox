using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Lyrox.Framework.CodeGeneration
{
    [Generator]
    public class PacketDeserializeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            const string attributeName = "AutoParsed";

            var classesWithAttribute = context.Compilation.SyntaxTrees
                .SelectMany(st => st.GetRoot()
                    .DescendantNodes()
                    .Where(n => n is RecordDeclarationSyntax)
                    .Select(n => n as RecordDeclarationSyntax)
                    .Where(r => r.AttributeLists
                        .SelectMany(al => al.Attributes)
                        .Any(a => a.Name.GetText().ToString() == attributeName)));

            foreach (var recordDecleration in classesWithAttribute)
                context.AddSource($"{recordDecleration.Identifier.ValueText}_AutoParsing.g.cs", GenerateRecord(recordDecleration));
        }

        public string GenerateRecord(RecordDeclarationSyntax packetDecleration)
        {
            const string parseMethodName = "Parse";

            var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), parseMethodName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.OverrideKeyword))
                .WithBody(SyntaxFactory.Block());

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

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
