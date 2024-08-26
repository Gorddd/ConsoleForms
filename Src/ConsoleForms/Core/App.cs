using ConsoleForms.Core.IOStreams;
using ConsoleForms.Forms;
using ConsoleForms.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Core;

public class App : IDisposable
{
    private readonly TextWriter _defaultWriter = Console.Out;
    private readonly TextReader _defaultReader = Console.In;

    private readonly FormsStack _formsStack;

    public App(Form mainForm)
    {
        Console.SetOut(new ThreadSafeWriter(Thread.CurrentThread.ManagedThreadId, _defaultWriter));
        Console.SetIn(new ThreadSafeReader(Thread.CurrentThread.ManagedThreadId, _defaultReader));

        mainForm.App = this;
        _formsStack = new FormsStack(mainForm);
    }

    public App(Form mainForm, Notifier notifier) : this(mainForm)
    {
        notifier.FormsStack = _formsStack;
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
            if (_formsStack.Current is null) return;
            Console.Clear();

            _formsStack.Current.Draw();
            _formsStack.Current.Activate();
        }
    }

    internal void Show(Form form)
    {
        form.App = this;
        _formsStack.Up(form);
    }

    internal void Back()
    {
        _formsStack.Down();
    }

    internal void BackTo(string formName)
    {
        _formsStack.DownTo(formName);
    }

    internal void BackToRoot()
    {
        _formsStack.DownToRoot();
    }
}
