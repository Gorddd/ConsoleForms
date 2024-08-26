using ConsoleForms.Core.IOStreams;
using ConsoleForms.Forms;
using ConsoleForms.Notifications;

namespace ConsoleForms.Core;

public class App : IDisposable
{
    private readonly TextWriter _defaultWriter = Console.Out;
    private readonly TextReader _defaultReader = Console.In;

    private readonly FormsStack _formsStack;
    private bool _hasStopped = false;
    
    public App(Form mainForm)
    {
        mainForm.App = this;
        _formsStack = new FormsStack(mainForm);

        InitializeThreadSafe();
    }

    private void InitializeThreadSafe()
    {
        var consoleInitSemaphore = new SemaphoreSlim(0);
        var safeThreadSemaphore = new SemaphoreSlim(0);
        
        int safeThreadId = default;
        new Thread(() =>
        {
            safeThreadId = Thread.CurrentThread.ManagedThreadId;
            consoleInitSemaphore.Release();
            safeThreadSemaphore.Wait();
            while (!_hasStopped)
                Dispatcher.ExecuteRequestedActions();
        }).Start();
        consoleInitSemaphore.Wait();

        Console.SetOut(new ThreadSafeWriter(safeThreadId, Thread.CurrentThread.ManagedThreadId, _defaultWriter));
        Console.SetIn(new ThreadSafeReader(safeThreadId, Thread.CurrentThread.ManagedThreadId, _defaultReader, _defaultWriter));
        safeThreadSemaphore.Release();
    }

    public App(Form mainForm, Notifier notifier) : this(mainForm)
    {
        notifier.FormsStack = _formsStack;
    }

    ~App()
    {
        Dispose();
    }

    public void Dispose()
    {
        Console.SetOut(_defaultWriter);
        Console.SetIn(_defaultReader);

        _hasStopped = true;
        Dispatcher.Free();
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
