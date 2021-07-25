namespace Crossbow.UI
{
    public struct Box : IUILayoutNode
    {
        public PropertyBlockHandle<BoxConstraintsPropertyBlock> constraints;
        public PropertyBlockHandle<BoxLayoutPropertyBlock> layout;
        public PropertyBlockHandle<BoxModelPropertyBlock> model;
    }
}