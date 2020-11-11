using CZGL.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Reflection;

namespace ConsoleTest
{
    class Program
    {
        delegate void A();
        static event A Test;

        public static int a { get; set; }
        
        static void Main(string[] args)
        {
            PropertyInfo info = typeof(Program).GetProperty("a");
           
            Console.WriteLine(info.Attributes.ToString());

            // ����ѡ��
            // ����ѡ����Բ�����
            DomainOptionBuilder option = new DomainOptionBuilder()
                .WithPlatform(Platform.AnyCpu)                     // ���ɿ���ֲ����
                .WithDebug(false)                                  // ʹ�� Release ����
                .WithKind(OutputKind.DynamicallyLinkedLibrary)     // ���ɶ�̬��
                .WithLanguageVersion(LanguageVersion.CSharp7_3);   // ʹ�� C# 7.3


            CompilationBuilder builder = CodeSyntax.CreateCompilation("Test.dll")
                .WithPath(Directory.GetParent(typeof(Program).Assembly.Location).FullName)
                .WithOption(option)                                // ����ʡ��
                .WithAutoAssembly()                                // �Զ���ӳ�������
                .WithNamespace(NamespaceBuilder.FromCode(@"using System;
    namespace MySpace
    {      
        public class Test
        {
            public string MyMethod()
            {
                Console.WriteLine(""�������гɹ�"");
                return ""���Գɹ�"";
        }
    }
}
"));

            try
            {
                if (builder.CreateDomain(out var messages))
                {
                    Console.WriteLine("����ɹ�����ʼִ�г��򼯽�����֤��");
                    var assembly = Assembly.LoadFile(Directory.GetParent(typeof(Program).Assembly.Location).FullName + "/Test.dll");
                    var type = assembly.GetType("MySpace.Test");
                    var method = type.GetMethod("MyMethod");
                    object obj = Activator.CreateInstance(type);
                    string result = (string)method.Invoke(obj, null);

                    if (result.Equals("���Գɹ�"))
                        Console.WriteLine("ִ�г��򼯲��Գɹ���");
                    else
                        Console.WriteLine("ִ�г��򼯲���ʧ�ܣ�");
                }
                else
                {
                    _ = messages.Execute(item =>
                    {
                        Console.WriteLine(@$"ID:{item.Id}
���س̶�:{item.Severity}     
λ�ã�{item.Location.SourceSpan.Start}~{item.Location.SourceSpan.End}
��Ϣ:{item.Descriptor.Title}   {item}");
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.ToString()}");
            }



            //List<PortableExecutableReference> references = assemblies.Select(c => MetadataReference.CreateFromStream(c)).ToList();



            //var tmp = DependencyContext.Default.RuntimeLibraries
            //    .Execute(item =>
            //    {
            //        item.Dependencies.Execute(itemNode =>
            //        {
            //            var t = $"{itemNode.Name}.dll";
            //            Console.WriteLine(item.Path + "|" + item.HashPath + "|" + item.Path);
            //            var c = MetadataReference.CreateFromFile(t);
            //            references.Add(c);
            //        });

            //    }).ToArray();


            //PortableExecutableReference[] mscorlibs = references.ToArray();

            //PortableExecutableReference mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            //CSharpCompilation compilation = CSharpCompilation.Create("MyCompilation",
            //    syntaxTrees: new[] { tree }, references: mscorlibs);

            ////Emitting to file is available through an extension method in the Microsoft.CodeAnalysis namespace
            //EmitResult emitResult = compilation.Emit("output.dll", "output.pdb");

            ////If our compilation failed, we can discover exactly why.
            //if (!emitResult.Success)
            //{
            //    foreach (var diagnostic in emitResult.Diagnostics)
            //    {
            //        Console.WriteLine(diagnostic.ToString());
            //    }
            //}



            //var a = DependencyContext.Default.CompileLibraries;
            //var b = a.Count;

            //foreach (var item in DependencyContext.Default.CompileLibraries.SelectMany(cl=>cl.ResolveReferencePaths()))
            //{
            //    Console.WriteLine(item);
            //}

            //GenericBuilder generic = new GenericBuilder();
            //generic.AddConstarint(new GenericScheme("T1", GenericConstraintsType.Struct));
            //generic.AddConstarint(new GenericScheme("T2", GenericConstraintsType.Class));
            //generic.AddConstarint(new GenericScheme("T3", GenericConstraintsType.Notnull));
            //generic.AddConstarint(new GenericScheme("T4", GenericConstraintsType.Unmanaged));
            //generic.AddConstarint(new GenericScheme("T5", GenericConstraintsType.New));
            //// ����ܹ������õ� Type
            //generic.AddConstarint(new GenericScheme("T6", GenericConstraintsType.BaseClass, typeof(int)));
            //// ���Ҫ���ַ�������������ͣ���ʹ�� ��API
            //generic.AddBaseClassConstarint("T7", " IEnumerable<int>");
            //generic.AddTUConstarint("T8", "T2");
            //generic.AddConstarint(new GenericScheme("T9", GenericConstraintsType.Class, GenericConstraintsType.New));
            //var syntax = generic.Build();
            //var result = syntax.ToFullString();
            //Console.WriteLine(result);

            //ClassBuilder buidler = new ClassBuilder();
            //var build = buidler.SetVisibility(ClassVisibilityType.Public)
            //    .SetName("Test")
            //    .AddMethodMember(b =>
            //    {
            //        b.SetVisibility(MemberVisibilityType.Public)
            //        .SetRondomName()
            //        .SetBlock("System.Console.WriteLine(\"111\");");
            //    })
            //    .Build();

            // CompilationBuilder compilation = new CompilationBuilder();
            //compilation.Test(build);
            Console.ReadKey();

        }

    }
}
