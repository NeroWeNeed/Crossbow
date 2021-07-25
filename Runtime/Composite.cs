namespace Crossbow.UI
{
    public interface IComposite<T> where T : unmanaged
    {
        public T X { get; }
    }
    public interface IComposite2<T> : IComposite<T> where T : unmanaged
    {
        public T Y { get; }
    }
    public interface IComposite3<T> : IComposite2<T> where T : unmanaged
    {
        public T Z { get; }
    }
    public interface IComposite4<T> : IComposite3<T> where T : unmanaged
    {
        public T W { get; }
    }
    public struct Composite<T> : IComposite<T> where T : unmanaged
    {
        private readonly T x;
        public T X { get => x; }
    }
    public struct Composite2<T> : IComposite2<T> where T : unmanaged
    {
        private readonly T x, y;
        public T X { get => x; }
        public T Y { get => y; }
    }
    public struct Composite3<T> : IComposite3<T> where T : unmanaged
    {
        private readonly T x, y, z;
        public T X { get => x; }
        public T Y { get => y; }
        public T Z { get => z; }
    }
    public struct Composite4<T> : IComposite4<T> where T : unmanaged
    {
        private readonly T x, y, z, w;
        public T X { get => x; }
        public T Y { get => y; }
        public T Z { get => z; }
        public T W { get => w; }
    }
}