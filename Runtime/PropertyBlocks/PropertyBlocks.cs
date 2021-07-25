using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace Crossbow.UI
{
    public struct BoxConstraintsPropertyBlock : IPropertyBlock
    {
        public Length minWidth, maxWidth, minHeight, maxHeight;
    }
    public struct BoxLayoutPropertyBlock : IPropertyBlock
    {
        public BoxLayoutDirection direction;
        public Length spacing;
        public Alignment alignment;
    }
    public struct BorderPropertyBlock : IPropertyBlock
    {
        public Composite4<Length> width;
        public Composite4<Color> color;
        public Composite4<Composite2<Length>> radii;
    }
    public struct BoxModelPropertyBlock : IPropertyBlock
    {
        public Composite4<Length> padding;
        public Composite4<Length> margin;
    }
    public enum BoxLayoutDirection : byte
    {
        Row = 0,
        RowReverse = 1,
        Column = 2,
        ColumnReverse = 3
    }
    

}