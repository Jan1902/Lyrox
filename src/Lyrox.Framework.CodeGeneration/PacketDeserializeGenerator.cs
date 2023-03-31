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
                context.AddSource($"{recordDecleration.Identifier.ValueText}_AutoParsing.g.cs", GenerateRecord(context, recordDecleration));
        }

        public string GenerateRecord(GeneratorExecutionContext context, RecordDeclarationSyntax packetDecleration)
        {
            const string parseMethodName = "Parse";

            var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), parseMethodName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.OverrideKeyword))
                .WithBody(SyntaxFactory.Block());

            var newClass = SyntaxFactory.RecordDeclaration(SyntaxFactory.Token(SyntaxKind.RecordKeyword), packetDecleration.Identifier.ValueText)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                .AddMembers(method);

            var newNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(packetDecleration.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault().Name.ToFullString()))
                .AddMembers(newClass);

            var syntaxFactory = SyntaxFactory.CompilationUnit()
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                .AddMembers(newNamespace);

            return syntaxFactory.NormalizeWhitespace().ToFullString();
        }

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
