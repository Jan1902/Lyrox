using System;

namespace Lyrox.Framework.CodeGeneration
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class VarIntAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class OptionalAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class LengthPrefixedAttribute : Attribute { }
}
