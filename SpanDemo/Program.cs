using System;
using System.Runtime.InteropServices;

namespace SpanDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // 在堆中分配内存，并使用 Span<> 处理
            byte[] buffer = new byte[1024];
            Span<byte> bufferSpan = buffer;
            // 使用索引器直接操作
            bufferSpan[1] = 1;
            // Span<>.Slice 比数组的 CopyTo 和字符串的 Substring 效率更高，因为后两者都需要在内存中重新分配内存并拷贝，而 Slice 是在原内存直接操作
            _ = bufferSpan.Slice(0);
            _ = bufferSpan.Slice(512, 512);
            _ = bufferSpan.Slice(new Range(new Index(128), new Index(128, true)));

            // 使用不安全的代码在栈分配内存，返回一个指针
            unsafe
            {
                byte* bufferPoint = stackalloc byte[1024];
                bufferPoint[10] = 1;
            }

            // 在非托管区域分配内存
            var bufferMemory = Marshal.AllocHGlobal(1024);
            // 读写非托管内存
            Marshal.ReadInt64(bufferMemory, 0);
            Marshal.WriteInt64(bufferMemory, 8, 100);
            // 手动释放非托管内存
            Marshal.FreeHGlobal(bufferMemory);

            // Memory<> 类型也像 Span<> 那样是个高效操作内存的类型，不同的是 Memory<> 是值类型，无法 ref；
            Memory<byte> memory = new Memory<byte>(buffer);
            memory.Slice(0, 128);
        }
    }
}
