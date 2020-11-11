﻿using CZGL.Roslyn.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace CZGL.Roslyn
{
    /// <summary>
    /// 结构体
    /// </summary>
    public class StructBuilder: ClassTemplate<StructBuilder>
    {
        internal StructBuilder()
        {
            _TBuilder = this;
        }

        internal StructBuilder(string name) : this()
        {
            base.WithName(name);
        }

        /// <summary>
        /// 通过字符串代码生成类
        /// </summary>
        /// <param name="Code">字符串代码</param>
        /// <returns></returns>
        public static StructDeclarationSyntax BuildSyntax(string Code)
        {
            StructDeclarationSyntax memberDeclaration;
            memberDeclaration = CSharpSyntaxTree.ParseText(Code)
                .GetRoot()
                .DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .FirstOrDefault();

            if (memberDeclaration is null)
                throw new InvalidOperationException("未能构建结构体，请检查代码语法是否有错误！");

            return memberDeclaration;
        }


        public StructDeclarationSyntax BuildSyntax()
        {
            StructDeclarationSyntax memberDeclaration;
            memberDeclaration = CSharpSyntaxTree.ParseText(ToFullCode())
               .GetRoot()
               .DescendantNodes()
               .OfType<StructDeclarationSyntax>()
               .FirstOrDefault();

            //if (_member.Atributes.Count != 0)
            //    memberDeclaration = memberDeclaration
            //        .WithAttributeLists(CodeSyntax.CreateAttributeList(_member.Atributes.ToArray()));

            if (memberDeclaration is null)
                throw new InvalidOperationException("未能构建结构体，请检查代码语法是否有错误！");

            return memberDeclaration;
        }

        /// <summary>
        /// 通过代码直接生成
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static StructBuilder FromCode(string Code)
        {
            if (string.IsNullOrEmpty(Code))
                throw new ArgumentNullException(nameof(Code));

            return new StructBuilder().WithFromCode(Code);
        }

        public override string ToFormatCode()
        {
            return BuildSyntax().NormalizeWhitespace().ToFullString();
        }

        public override string ToFullCode()
        {
            if (_base.UseCode)
                return _base.Code;

            const string Template = @"{Attributes}{Access} {Keyword} struct {Name}{GenericParams} {:}{BaseClass}{,}{Interfaces}{GenericList}
{
{Fields}

{Properties}

{Delegates}

{Events}

{Methods}

{Others}
}";

            var code = Template
                .Replace("{Attributes}", _member.Atributes.Join("\n").CodeNewAfter("\n"))
                .Replace("{Access}", _member.Access)
                .Replace("{Keyword}", _class.Keyword)
                .Replace("{Name}", _base.Name)
                .Replace("{GenericParams}", _member.GenericParams.GetParamCode().CodeNewBefore("<").CodeNewAfter(">"))
                .Replace("{:}", string.IsNullOrEmpty(_class.BaseClass) && _class.Interfaces.Count == 0 ? "" : ":")
                .Replace("{BaseClass}", _class.BaseClass)
                .Replace("{,}", (string.IsNullOrEmpty(_class.BaseClass) || _class.Interfaces.Count == 0) ? "" : ",")
                .Replace("{Interfaces}", _class.Interfaces.Join(","))
                .Replace("{GenericList}", _member.GenericParams.GetWhereCode(true).CodeNewBefore("\n"))
                .Replace("{Fields}", _class.Fields.Select(item => item.ToFullCode()).Join("\n"))
                .Replace("{Properties}", _class.Propertys.Select(item => item.ToFullCode()).Join("\n"))
                .Replace("{Delegates}", _class.Delegates.Select(item => item.ToFullCode()).Join("\n"))
                .Replace("{Events}", _class.Events.Select(item => item.ToFullCode()).Join("\n"))
                .Replace("{Methods}", _class.Methods.Select(item => item.ToFullCode()).Join("\n"))
                .Replace("{Others}", _class.Others.OfType<BaseTemplate>().Select(item => item.ToFullCode()).Join("\n"));

            return code;
        }
    }
}
