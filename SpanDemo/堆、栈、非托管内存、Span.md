# 堆、栈、非托管内存、Span

## **堆内存：**

- 存储引用类型；
- 被托管，由GC回收；
- 容易产生碎片；



## **非托管内存：**

- 本地的堆内存；
- 未被托管，不受GC影响；
- 通过 Marshal 操作；
  - Marshal.AllocHGlobal 申请内存；
  - Marshal.FreeHGlobal 释放内存；
- 需要手动释放；



## **栈内存：**

- 存储值类型；
- 声明域结束立即释放；
- 存在容量上限（默认1MB），无法解决的Exception；
- 微软使用栈内存来快速地记录ETW事件日志；

```csharp
unsafe 
{
    // 申请栈内存
    var a = stackalloc byte[1000]; 
} 
```



## **Span<>**

> [https://github.com/CuteLeon/CacheFramework](https://github.com/CuteLeon/CacheFramework)

- 在原内存区域操作；
- 更好的性能；

```csharp
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
    }
```