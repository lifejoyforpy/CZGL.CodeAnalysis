using CZGL.Roslyn;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace RoslynExtensionsTests
{
    /// <summary>
    /// �ֶι�������չ����
    /// </summary>
    public class FiledBuilderExtensionsTests
    {
        private readonly FieldBuilder builder = CodeSyntax.CreateField("T1");
        ITestOutputHelper _tempOutput;
        public FiledBuilderExtensionsTests(ITestOutputHelper tempOutput)
        {
            _tempOutput = tempOutput;
        }

        /// <summary>
        /// WithType ����
        /// </summary>
        [Fact]
        public void ���Ͷ���1()
        {
            builder.WithType<int>();
            var result = builder.ToFormatCode();
#if Log
            _tempOutput.WriteLine(result.WithUnixEOL());
#endif
            Assert.Equal("int T1;", result.WithUnixEOL());
        }

        /// <summary>
        /// WithType ����
        /// </summary>
        [Fact]
        public void ���Ͷ���2()
        {
            builder.WithType(typeof(int));
            var result = builder.ToFormatCode();
#if Log
            _tempOutput.WriteLine(result.WithUnixEOL());
#endif
            Assert.Equal("int T1;", result.WithUnixEOL());
        }

        private static readonly List<int> a;

        /// <summary>
        /// WithType ����
        /// </summary>
        [Fact]
        public void �ֶθ���()
        {
            builder.WithCopy(typeof(FiledBuilderExtensionsTests).GetField("a"));
            var result = builder.ToFormatCode();
#if Log
            _tempOutput.WriteLine(result.WithUnixEOL());
#endif
            Assert.Equal("int T1;", result.WithUnixEOL());
        }

    }
}
