using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Core.IOStreams;

internal class ThreadSafeReader : TextReader
{
    private readonly TextReader _reader;
    private readonly int _uiThreadId;

    public ThreadSafeReader(int uiThreadId, TextReader reader)
    {
        _uiThreadId = uiThreadId;
        _reader = reader;
    }

    private void CheckThread()
    {
        if (Thread.CurrentThread.ManagedThreadId != _uiThreadId)
            throw new InvalidOperationException("Cannot use console stream not in UI thread to prevent thread-unsafe operations");
    }

    public override int Read()
    {
        CheckThread();
        return _reader.Read();
    }

    public override string? ReadLine()
    {
        CheckThread();
        return _reader.ReadLine();
    }
}
