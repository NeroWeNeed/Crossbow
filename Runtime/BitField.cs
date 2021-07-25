
using System.Runtime.InteropServices;

namespace Crossbow.UI
{
    public interface IBitField
    {
        public bool this[int index]
        {
            get;
            set;
        }
        public void Toggle(int index);
        public void Clear();
        public void Fill();
    }
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct FixedBitField8 : IBitField
    {
        [FieldOffset(0)]
        private byte value;
        public bool this[int index]
        {
            get => (value & (1 << index)) != 0;
            set
            {
                if (value)
                {
                    this.value |= (byte)(1 << index);
                }
                else
                {
                    this.value &= (byte)~(1 << index);
                }
            }
        }

        public void Clear()
        {
            value = 0;
        }

        public void Fill()
        {
            value = byte.MaxValue;
        }

        public void Toggle(int index)
        {
            value ^= (byte)(1 << index);
        }
    }
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct FixedBitField16 : IBitField
    {
        [FieldOffset(0)]
        private ushort value;
        public bool this[int index]
        {
            get => (value & (1 << index)) != 0;
            set
            {
                if (value)
                {
                    this.value |= (ushort)(1 << index);
                }
                else
                {
                    this.value &= (ushort)~(1 << index);
                }
            }
        }

        public void Clear()
        {
            value = 0;
        }

        public void Fill()
        {
            value = ushort.MaxValue;
        }

        public void Toggle(int index)
        {
            value ^= (ushort)(1 << index);
        }
    }
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct FixedBitField32 : IBitField
    {
        [FieldOffset(0)]
        private uint value;
        public bool this[int index]
        {
            get => (value & (1 << index)) != 0;
            set
            {
                if (value)
                {
                    this.value |= (uint)(1 << index);
                }
                else
                {
                    this.value &= (uint)~(1 << index);
                }
            }
        }

        public void Clear()
        {
            value = 0;
        }

        public void Fill()
        {
            value = uint.MaxValue;
        }

        public void Toggle(int index)
        {
            value ^= (uint)(1 << index);
        }
    }
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct FixedBitField64 : IBitField
    {
        [FieldOffset(0)]
        private ulong value;
        public bool this[int index]
        {

            get => (value & (ulong)(1 << index)) != 0UL;
            set
            {
                if (value)
                {
                    this.value |= 1UL << index;
                }
                else
                {
                    this.value &= ~(1UL << index);
                }
            }
        }

        public void Clear()
        {
            value = 0;
        }

        public void Fill()
        {
            value = ulong.MaxValue;
        }

        public void Toggle(int index)
        {
            value ^= 1UL << index;
        }
    }
}