using System.Runtime.InteropServices;
using System;

namespace Crossbow.UI
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    [Serializable]
    public readonly struct HorizontalAlignment
    {
        public static readonly HorizontalAlignment Left = new HorizontalAlignment(0);
        public static readonly HorizontalAlignment Center = new HorizontalAlignment(1);
        public static readonly HorizontalAlignment Right = new HorizontalAlignment(2);
        public static readonly HorizontalAlignment Stretch = new HorizontalAlignment(3);
        [FieldOffset(0)]
        internal readonly byte value;

        internal HorizontalAlignment(byte value)
        {
            this.value = value;
        }
        public static implicit operator HorizontalAlignment(Alignment alignment) => new HorizontalAlignment((byte)(alignment.value & 3));
    }
[StructLayout(LayoutKind.Explicit, Size = 1)]
    [Serializable]
    public readonly struct VerticalAlignment
    {
        public static readonly VerticalAlignment Top = new VerticalAlignment(0);
        public static readonly VerticalAlignment Center = new VerticalAlignment(4);
        public static readonly VerticalAlignment Bottom = new VerticalAlignment(8);
        public static readonly VerticalAlignment Stretch = new VerticalAlignment(12);
        [FieldOffset(0)]
        internal readonly byte value;

        internal VerticalAlignment(byte value)
        {
            this.value = value;
        }

        public static implicit operator VerticalAlignment(Alignment alignment) => new VerticalAlignment((byte)(alignment.value & 12));
    }
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    [Serializable]
    public readonly struct Alignment
    {
        public static readonly Alignment TopLeft = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Top);
        public static readonly Alignment TopCenter = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Top);
        public static readonly Alignment TopRight = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Top);
        public static readonly Alignment TopStretched = new Alignment(HorizontalAlignment.Stretch, VerticalAlignment.Top);
        public static readonly Alignment CenterLeft = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Center);
        public static readonly Alignment Center = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Center);
        public static readonly Alignment CenterRight = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Center);
        public static readonly Alignment CenterStretchedHorizontally = new Alignment(HorizontalAlignment.Stretch, VerticalAlignment.Center);
        public static readonly Alignment BottomLeft = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);
        public static readonly Alignment BottomCenter = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
        public static readonly Alignment BottomRight = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Bottom);
        public static readonly Alignment BottomStretched = new Alignment(HorizontalAlignment.Stretch, VerticalAlignment.Bottom);
        public static readonly Alignment LeftStretched = new Alignment(HorizontalAlignment.Left, VerticalAlignment.Stretch);
        public static readonly Alignment CenterStretchedVertically = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Stretch);
        public static readonly Alignment RightStretched = new Alignment(HorizontalAlignment.Right, VerticalAlignment.Stretch);
        public static readonly Alignment Stretched = new Alignment(HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
        [FieldOffset(0)]
        internal readonly byte value;

        internal Alignment(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            this.value = (byte)(horizontalAlignment.value | verticalAlignment.value);
        }
        public static implicit operator Alignment(HorizontalAlignment horizontalAlignment) => new Alignment(horizontalAlignment, VerticalAlignment.Top);
        public static implicit operator Alignment(VerticalAlignment verticalAlignment) => new Alignment(HorizontalAlignment.Left, verticalAlignment);
    }
}