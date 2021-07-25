using System;
using System.Runtime.CompilerServices;

namespace Crossbow.UI
{
    [Serializable]
    public readonly struct Length
    {
        public readonly float value;
        public readonly LengthUnit unit;

        public Length(float value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }
    }
    /// <summary>
    /// Units for <see cref="Length"/>. The first bit denotes whether or not it's an absolute unit (0) or a relative unit (1)
    /// </summary>
    public enum LengthUnit : byte
    {
        #region Absolute Units
        Pixels = 0b00000000,
        Centimeters = 0b00000010,
        Millimeters = 0b00000100,
        Inches = 0b00000110,
        Points = 0b00001000,
        Picas = 0b00001010,
        #endregion
        #region Relative Units
        ElementSize = 0b00000001,
        XHeight = 0b00000011,
        ZeroWidth = 0b00000101,
        RootElementSize = 0b00000111,
        ViewportWidth = 0b00001001,
        ViewportHeight = 0b00001011,
        ViewportMin = 0b00001101,
        ViewportMax = 0b00001111,
        Percentage = 0b00010001,

        #endregion
    }
    public static class LengthExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRelative(this LengthUnit unit) => (((byte)unit) & 1) != 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAbsolute(this LengthUnit unit) => (((byte)unit) & 1) == 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRelative(this Length length) => IsRelative(length.unit);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAbsolute(this Length length) => IsAbsolute(length.unit);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Px(this float value) => new Length(value, LengthUnit.Pixels);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Px(this int value) => new Length(value, LengthUnit.Pixels);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Cm(this float value) => new Length(value, LengthUnit.Centimeters);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Cm(this int value) => new Length(value, LengthUnit.Centimeters);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Mm(this float value) => new Length(value, LengthUnit.Millimeters);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Mm(this int value) => new Length(value, LengthUnit.Millimeters);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length In(this float value) => new Length(value, LengthUnit.Inches);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length In(this int value) => new Length(value, LengthUnit.Inches);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Pt(this float value) => new Length(value, LengthUnit.Points);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Pt(this int value) => new Length(value, LengthUnit.Points);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Pc(this float value) => new Length(value, LengthUnit.Picas);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Pc(this int value) => new Length(value, LengthUnit.Picas);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Em(this float value) => new Length(value, LengthUnit.ElementSize);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Em(this int value) => new Length(value, LengthUnit.ElementSize);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Ch(this float value) => new Length(value, LengthUnit.ZeroWidth);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Ch(this int value) => new Length(value, LengthUnit.ZeroWidth);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Rem(this float value) => new Length(value, LengthUnit.RootElementSize);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Rem(this int value) => new Length(value, LengthUnit.RootElementSize);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vw(this float value) => new Length(value, LengthUnit.ViewportWidth);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vw(this int value) => new Length(value, LengthUnit.ViewportWidth);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vh(this float value) => new Length(value, LengthUnit.ViewportHeight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vh(this int value) => new Length(value, LengthUnit.ViewportHeight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vmin(this float value) => new Length(value, LengthUnit.ViewportMin);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vmin(this int value) => new Length(value, LengthUnit.ViewportMin);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vmax(this float value) => new Length(value, LengthUnit.ViewportMax);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Vmax(this int value) => new Length(value, LengthUnit.ViewportMax);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Perc(this float value) => new Length(value, LengthUnit.Percentage);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Length Perc(this int value) => new Length(value, LengthUnit.Percentage);
    }
}