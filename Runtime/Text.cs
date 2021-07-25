using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Crossbow.UI
{
    public unsafe interface IUTF8TextHandle : IEnumerable<uint>
    {
        public void* Value { get; }
    }

    public unsafe struct UTF8TextBlob : IUTF8TextHandle
    {
        public void* Value { get; }

        public UTF8TextBlob(void* value)
        {
            Value = value;
        }

        public IEnumerator<uint> GetEnumerator() => new UTF8CodePointEnumerator((byte*)Value);

        IEnumerator IEnumerable.GetEnumerator() => new UTF8CodePointEnumerator((byte*)Value);
    }
    public unsafe struct UTF8Text : IUTF8TextHandle, IDisposable
    {
        public void* Value { get; }
        private Allocator allocator;

        public UTF8Text(string text, Allocator allocator)
        {
            this.allocator = allocator;
            var bytes = UTF8Encoding.UTF8.GetBytes(text);
            var ptr = (byte*)UnsafeUtility.Malloc(sizeof(int) + ByteOrderMark.UTF8.Length + bytes.Length + 1, 4, allocator);
            ((int*)ptr)[0] = text.Length;
            ByteOrderMark.UTF8.WriteTo(ptr + sizeof(int));
            fixed (byte* bytesPtr = bytes)
            {
                UnsafeUtility.MemCpy(ptr + sizeof(int) + ByteOrderMark.UTF8.Length, bytesPtr, bytes.Length);
            }
            ptr[sizeof(int) + ByteOrderMark.UTF8.Length + bytes.Length] = 0;
            Value = ptr;
        }

        public IEnumerator<uint> GetEnumerator() => new UTF8CodePointEnumerator((byte*)Value);

        IEnumerator IEnumerable.GetEnumerator() => new UTF8CodePointEnumerator((byte*)Value);

        public void Dispose()
        {
            UnsafeUtility.Free(Value, allocator);
        }
    }
    [Serializable]
    public unsafe struct ByteOrderMark : IEquatable<ByteOrderMark>
    {
        public static readonly ByteOrderMark UTF8 = new ByteOrderMark(0xEF, 0xBB, 0xBF);
        public static readonly ByteOrderMark UTF16BE = new ByteOrderMark(0xFE, 0xFF);
        public static readonly ByteOrderMark UTF16LE = new ByteOrderMark(0xFF, 0xFE);
        public static readonly ByteOrderMark UTF32BE = new ByteOrderMark(0, 0, 0xFE, 0xFF);
        public static readonly ByteOrderMark UTF32LE = new ByteOrderMark(0xFF, 0xFE, 0, 0);
        private fixed byte value[4];
        private byte length;
        public byte Length { get => length; }
        internal ByteOrderMark(byte a, byte b, byte c, byte d)
        {
            this.value[0] = a;
            this.value[1] = b;
            this.value[2] = c;
            this.value[3] = d;
            length = 4;
        }
        internal ByteOrderMark(byte a, byte b, byte c)
        {
            this.value[0] = a;
            this.value[1] = b;
            this.value[2] = c;
            this.value[3] = 0;
            length = 3;
        }
        internal ByteOrderMark(byte a, byte b)
        {
            this.value[0] = a;
            this.value[1] = b;
            this.value[2] = 0;
            this.value[3] = 0;
            length = 2;
        }
        public bool Matches(byte* ptr)
        {
            for (int i = 0; i < length; i++)
            {
                if (value[i] != ptr[i])
                {
                    return false;
                }
            }
            return true;
        }
        public void WriteTo(byte* destination)
        {
            for (int i = 0; i < length; i++)
            {
                destination[i] = value[i];
            }
        }
        public static ByteOrderMark Parse(byte* ptr)
        {
            var boms = stackalloc ByteOrderMark[] {
                UTF16LE,
                UTF16BE,
                UTF8,
                UTF32LE,
                UTF32BE
            };
            for (int i = 0; i < 5; i++)
            {
                if (boms[i].Matches(ptr))
                {
                    return boms[i];
                }
            }
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            throw new InvalidOperationException($"Unknown ByteOrderMark: {ptr[0]:X2} {ptr[1]:X2} {ptr[2]:X2} {ptr[3]:X2}");
#else
            return default;
#endif
        }
        public static bool TryParse(byte* ptr, out ByteOrderMark result)
        {
            var boms = stackalloc ByteOrderMark[] {
                UTF16LE,
                UTF16BE,
                UTF8,
                UTF32LE,
                UTF32BE
            };
            for (int i = 0; i < 5; i++)
            {
                if (boms[i].Matches(ptr))
                {
                    result = boms[i];
                    return true;
                }
            }
            result = default;
            return false;
        }

        public bool Equals(ByteOrderMark other)
        {
            if (length != other.length)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (value[i] != other.value[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
    public unsafe static class TextHandleExtensions
    {
        public static int GetLength<T>(this T handle) where T : unmanaged, IUTF8TextHandle
        {
            return *(int*)(handle.Value);
        }
        public static ByteOrderMark GetPreamble<T>(this T handle) where T : unmanaged, IUTF8TextHandle
        {
            return ByteOrderMark.Parse(((byte*)handle.Value) + sizeof(int));
        }

    }
    /// <summary>
    /// Enumerator for UTF8 strings in unmanaged memory.
    /// </summary>
    public unsafe struct UTF8CodePointEnumerator : IEnumerator<uint>
    {
        private byte* codePoints;
        private ulong offset;
        private uint current;
        private ByteOrderMark byteOrderMark;
        private int length;
        private uint next;
        public uint Current => current;
        object IEnumerator.Current => current;
        public UTF8CodePointEnumerator(byte* codePoints)
        {
            this.codePoints = codePoints;
            this.offset = 0;
            this.current = 0;
            length = *(int*)codePoints;
            byteOrderMark = ByteOrderMark.Parse(codePoints + 4);
            this.next = (uint)(sizeof(int) + byteOrderMark.Length);
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            offset += next;
            var b1 = *(codePoints + offset);
            if (b1 < 0x80)
            {
                next = 1;
                current = (uint)(b1 & 0x7F);
            }
            else if (b1 >= 0xC0 && b1 < 0xE0)
            {
                next = 2;
                current = (uint)((b1 & 0x1F) << 6) | (uint)(*(codePoints + offset + 1) & 0x3F);
            }
            else if (b1 >= 0xE0 && b1 < 0xF0)
            {
                next = 3;
                current = (uint)((b1 & 0xF) << 12) | (uint)((*(codePoints + offset + 1) & 0x3F) << 6) | (uint)(*(codePoints + offset + 2) & 0x3F);
            }
            else if (b1 >= 0xF0 && b1 < 0xF8)
            {
                next = 4;
                current = (uint)((b1 & 0x7) << 18) | (uint)((*(codePoints + offset + 1) & 0x3F) << 12) | (uint)((*(codePoints + offset + 2) & 0x3F) << 6) | (uint)(*(codePoints + offset + 3) & 0x3F);
            }
            else
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                throw new InvalidOperationException("Unknown character specified.");
#else
                return false;
#endif
            }
            return *(codePoints + offset + next) != 0;
        }

        public void Reset()
        {
            offset = 0;
            current = 0;
            length = *(int*)codePoints;
            byteOrderMark = ByteOrderMark.Parse(codePoints + 4);
            this.next = (uint)(sizeof(int) + byteOrderMark.Length);
        }
    }

}