using Unity.Entities;

namespace Crossbow.UI
{
    [InternalBufferCapacity(0)]
    public struct UINodeData : IBufferElementData
    {
        public byte value;
    }
}