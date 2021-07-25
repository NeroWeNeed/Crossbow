using System;

namespace Crossbow.UI
{
    /// <summary>
    /// Limits the amount of node children an element can have. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class MaxChildrenAttribute : Attribute
    {
        public int Value { get; set; } = int.MaxValue;
        public MaxChildrenAttribute(int value)
        {
            Value = value;
        }
    }
    /// <summary>
    /// Special Attribute for assigning property names to composite types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CompositePropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string XSuffix { get; set; }
        public string YSuffix { get; set; }
        public string ZSuffix { get; set; }
        public string WSuffix { get; set; }
    }
    /// <summary>
    /// Attribute for meta information on a given property block.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class PropertyBlockAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsGlobal { get; set; }
    }
    /// <summary>
    /// Attribute for meta information on a given property in a property block.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// Attribute for meta information on a given bit property in a property block.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = true)]
    public sealed class BitPropertyAttribute : Attribute
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public BitPropertyAttribute(int index, string name)
        {
            Index = index;
            Name = name;
        }
    }
}