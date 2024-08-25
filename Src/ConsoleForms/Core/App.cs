using ConsoleForms.Core.IOStreams;
using ConsoleForms.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Core;

public class App : IDisposable
{
    private readonly TextWriter _defaultWriter = Console.Out;
    private readonly TextReader _defaultReader = Console.In;

    public App(Form mainForm)
    {
        Console.SetOut(new ThreadSafeWriter(Thread.CurrentThread.ManagedThreadId, _defaultWriter));
        Console.SetIn(new ThreadSafeReader(Thread.CurrentThread.ManagedThreadId, _defaultReader));

        Dispatcher.CurrentForm = mainForm;
    }

    public void Dispose()
    {
        Console.SetOut(_defaultWriter);
        Console.SetIn(_defaultReader);
    }

    public void Run()
    {
        while (true)
        {
            Dispatcher.CurrentForm.Show();
        }
    }
}
