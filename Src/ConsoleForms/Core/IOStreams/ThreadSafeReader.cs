using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Core.IOStreams;

internal class ThreadSafeReader : TextReader
{
    private readonly TextReader _reader;
    private readonly TextWriter _writer;
    private readonly int _safeThreadId;
    private readonly int _uiThreadId;

    public ThreadSafeReader(int safeThreadId, int uiThreadid, TextReader reader, TextWriter writer)
    {
        _safeThreadId = safeThreadId;
        _uiThreadId = uiThreadid;
        _reader = reader;
        _writer = writer;
    }

    private void CheckThread()
    {
        if (Thread.CurrentThread.ManagedThreadId != _safeThreadId && Thread.CurrentThread.ManagedThreadId != _uiThreadId)
        {
            var ex = new InvalidOperationException("Cannot use console stream not in UI thread to prevent thread-unsafe operations. Use Dispatcher.Invoke()");
            _writer.WriteLine(ex);
            throw ex;
        }
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
