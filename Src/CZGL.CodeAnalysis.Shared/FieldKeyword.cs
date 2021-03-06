﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.CodeAnalysis.Shared
{
    /// <summary>
    /// 字段的关键字
    /// </summary>
    [Flags]
    public enum FieldKeyword
    {
        [MemberDefineName(Name = "")]
        Default = 0,

        [MemberDefineName(Name = "const")]
        Const = 1,

        [MemberDefineName(Name = "static")]
        Static = 1 << 1,

        [MemberDefineName(Name = "readonly")]
        Readonly = 1 << 2,

        [MemberDefineName(Name = "static readonly")]
        StaticReadonly = Static | Readonly,

        [MemberDefineName(Name = "volatile")]
        Volatile = StaticReadonly << 1,
        [MemberDefineName(Name = "volatile static")]
        VolatileStatic = Volatile << 1
    }
}
