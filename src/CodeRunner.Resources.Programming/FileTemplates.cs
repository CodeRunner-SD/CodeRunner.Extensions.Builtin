using CodeRunner.Packaging;
using CodeRunner.Templates;
using System;
using System.Reflection;

namespace CodeRunner.Resources.Programming
{
    public static class FileTemplates
    {
        private static Package<ITemplate> CreateSingleFile(string name, string ext, string source)
        {
            return new Package<ITemplate>(new PackageFileTemplate(
                new StringTemplate(
                    StringTemplate.GetVariableTemplate("name") + $".{ext}",
                    new Variable[] { new Variable("name").Required() }
                )
            ).UseTemplate(new TextFileTemplate(new StringTemplate(source))))
            {
                Metadata = new PackageMetadata
                {
                    Name = name,
                    Author = nameof(CodeRunner),
                    CreationTime = DateTimeOffset.Now,
                    Version = Assembly.GetAssembly(typeof(FileTemplates))?.GetName().Version ?? new Version()
                }
            };
        }

        public static Package<ITemplate> C => CreateSingleFile("c", "c", Properties.Resources.tpl_c);
        public static Package<ITemplate> Cpp => CreateSingleFile("cpp", "cpp", Properties.Resources.tpl_cpp);
        public static Package<ITemplate> CSharp => CreateSingleFile("csharp", "cs", Properties.Resources.tpl_csharp);
        public static Package<ITemplate> Python => CreateSingleFile("python", "py", Properties.Resources.tpl_python);
        public static Package<ITemplate> FSharp => CreateSingleFile("fsharp", "fs", Properties.Resources.tpl_fsharp);
        public static Package<ITemplate> Go => CreateSingleFile("go", "go", Properties.Resources.tpl_go);
        public static Package<ITemplate> Java => CreateSingleFile("java", "java", Properties.Resources.tpl_java);
    }
}
