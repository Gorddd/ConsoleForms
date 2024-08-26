using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Core.IOStreams;

internal class ThreadSafeWriter : TextWriter
{
    public override Encoding Encoding => Encoding.Default;

    private readonly TextWriter _writer;
    private readonly int _safeThreadId;
    private readonly int _uiThreadId;

    public ThreadSafeWriter(int safeThreadId, int uiThreadId, TextWriter writer)
    {
        _writer = writer;
        _safeThreadId = safeThreadId;
        _uiThreadId = uiThreadId;
    }

    private void CheckThread()
    {
        if (Thread.CurrentThread.ManagedThreadId != _safeThreadId && Thread.CurrentThread.ManagedThreadId != _uiThreadId)
        {
            var ex = new InvalidOperationException("Cannot use console stream not in UI thread to prevent thread-unsafe operations");
            _writer.WriteLine(ex);
            throw ex;
        }
    }

    public override void Write(bool value)
    {
        CheckThread();
        _writer.Write(value);
    }

    public override void Write(char value)
    {
        CheckThread();
        _writer.Write(value);
    }

    public override void Write(char[] buffer, int index, int count)
    {
        CheckThread();
        _writer.Write(buffer, index, count);
    }

    public override void Write(string? value)
    {
        CheckThread();
        _writer.Write(value);
    }

    public override void Write(int value)
    {
        CheckThread();
        _writer.Write(value);
    }

    public override void WriteLine()
    {
        CheckThread();
        _writer.WriteLine();
    }

    public override void WriteLine(int value)
    {
        CheckThread();
        _writer.WriteLine(value);
    }

    public override void WriteLine(string? value)
    {
        CheckThread();
        _writer.WriteLine(value);
    }
}
