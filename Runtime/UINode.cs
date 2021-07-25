using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;

namespace Crossbow.UI
{
    public interface IUINode {

    }
    public interface IUILayoutNode : IUINode {

    }
    public interface IUIRenderNode : IUINode {

    }
    public interface IPropertyBlock {

    }
    public unsafe struct PropertyBlockHandle<T> where T : unmanaged, IPropertyBlock {
        public T* Value { get; internal set; }
    }
    public struct UINodeHeader
    {
        /// <summary>
        /// Element ID used for layout and rendering
        /// </summary>
        public uint elementId;
        /// <summary>
        /// Node ID in the UI hierarchy.
        /// </summary>
        public uint nodeId;
        /// <summary>
        /// Total size in bytes of the UI Node Block. Excludes the size of the header
        /// </summary>
        public uint size;

        public Constraints constraints;

        public Layout layout;

        public int childCount;
    }
    public struct Constraints
    {
        public float2 minimum;
        public float2 maximum;
    }
    public struct Layout
    {
        public float2 position;
        public float2 size;
    }
    public readonly unsafe struct UIHandle : IEnumerable<UINodeHandle>
    {
        public readonly void* value;
        public bool IsCreated { get => value != null; }
        internal UIHandle(void* value)
        {
            this.value = value;
        }

        public IEnumerator<UINodeHandle> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public struct Enumerator : IEnumerator<UINodeHandle>
        {
            private ulong offset;
            private uint nextSize;
            private ulong totalSize;
            private byte* ptr;
            private UINodeHandle current;
            public UINodeHandle Current => current;
            object IEnumerator.Current => current;

            public Enumerator(UIHandle handle)
            {
                this.offset = 0;
                this.ptr = (byte*)handle.value;
                this.nextSize = sizeof(ulong);
                this.totalSize = handle.GetSize();
                current = default;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                offset += nextSize;
                nextSize = ((uint)sizeof(UINodeHeader)) + ((UINodeHeader*)(ptr + offset))->size;
                current = new UINodeHandle(ptr + offset);
                return (offset + nextSize) < totalSize;
            }

            public void Reset()
            {
                this.offset = 0;
                this.nextSize = sizeof(ulong);
                current = default;
            }
        }
    }
    public readonly unsafe struct UINodeHandle
    {
        public readonly void* value;

        internal UINodeHandle(void* value)
        {
            this.value = value;
        }
    }
    public readonly unsafe struct UINodeOffset
    {
        public readonly ulong value;

        public UINodeOffset(ulong value)
        {
            this.value = value;
        }

        public static implicit operator ulong(UINodeOffset nodeOffset) => nodeOffset.value;
        public static implicit operator UINodeOffset(ulong value) => new UINodeOffset(value);
    }
    public unsafe static class UIHandleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UIHandle GetHandle(this DynamicBuffer<UINodeData> nodeData) => new UIHandle(nodeData.GetUnsafePtr());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UIHandle GetReadOnlyHandle(this DynamicBuffer<UINodeData> nodeData) => new UIHandle(nodeData.GetUnsafeReadOnlyPtr());
        /// <summary>
        /// Returns the overall size in bytes of a given handle. The first 8 bytes of any UI block is reserved for size information.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GetSize(this UIHandle handle) => *(ulong*)handle.value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeHashMap<uint, UINodeHandle> CreateNodeHandleMap(this UIHandle handle, Allocator allocator = Allocator.Temp)
        {
            var ptr = (byte*)handle.value;
            var result = new NativeHashMap<uint, UINodeHandle>(8, allocator);
            ulong offset = sizeof(ulong);
            ulong size = GetSize(handle);
            while (offset < size)
            {
                var header = (UINodeHeader*)(ptr + offset);
                result[header->nodeId] = new UINodeHandle(header);
                offset += ((uint)sizeof(UINodeHeader)) + header->size;
            }
            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeHashMap<uint, UINodeOffset> CreateNodeOffsetMap(this UIHandle handle, Allocator allocator = Allocator.Temp)
        {
            var ptr = (byte*)handle.value;
            var result = new NativeHashMap<uint, UINodeOffset>(8, allocator);
            ulong offset = sizeof(ulong);
            ulong size = GetSize(handle);
            while (offset < size)
            {
                var header = (UINodeHeader*)(ptr + offset);
                result[header->nodeId] = new UINodeOffset(offset);
                offset += ((uint)sizeof(UINodeHeader)) + header->size;
            }
            return result;
        }
    }

    public unsafe static class UINodeHandleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UINodeHeader GetHeader(this UINodeHandle handle) => *(UINodeHeader*)handle.value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NativeArray<uint> GetChildrenIds(this UINodeHandle handle, Allocator allocator = Allocator.Temp)
        {
            var header = handle.GetHeader();
            var result = new NativeArray<uint>(header.childCount, allocator);
            UnsafeUtility.MemCpy(result.GetUnsafePtr(), ((byte*)handle.value) + sizeof(UINodeHeader), sizeof(uint) * header.childCount);
            return result;
        }
    }
}