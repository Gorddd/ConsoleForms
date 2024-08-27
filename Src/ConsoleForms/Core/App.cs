using ConsoleForms.Core.IOStreams;
using ConsoleForms.Forms;
using ConsoleForms.Notifications;

namespace ConsoleForms.Core;

public class App : IDisposable
{
    private readonly TextWriter _defaultWriter = Console.Out;
    private readonly TextReader _defaultReader = Console.In;

    private readonly FormsStack _formsStack = new FormsStack();
    
    private static bool _isRunning = false;
    
    public App(Form mainForm)
    {
        InitializeForm(mainForm);
    }

    public App(Form mainForm, Notifier notifier) : this(mainForm)
    {
        notifier.FormsStack = _formsStack;

        mainForm.Notifier = notifier;
    }

    private void InitializeForm(Form form)
    {
        form.App = this;
        _formsStack.Up(form);
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
            while (_isRunning)
                Dispatcher.ExecuteRequestedActions();
        }).Start();
        consoleInitSemaphore.Wait();

        Console.SetOut(new ThreadSafeWriter(safeThreadId, Thread.CurrentThread.ManagedThreadId, _defaultWriter));
        Console.SetIn(new ThreadSafeReader(safeThreadId, Thread.CurrentThread.ManagedThreadId, _defaultReader, _defaultWriter));
        safeThreadSemaphore.Release();
    }

    ~App()
    {
        Dispose();
    }

    public void Dispose()
    {
        ShutDown();
    }

    public void Run()
    {
        if (_isRunning) throw new InvalidOperationException("You already have running app in this application domain");
        _isRunning = true;

        InitializeThreadSafe();

        while (true)
        {
            if (_formsStack.Current is null) break;
            Console.Clear();

            _formsStack.Current.Draw();
            _formsStack.Current.Activate();
        }

        ShutDown();
    }

    private void ShutDown()
    {
        Console.SetOut(_defaultWriter);
        Console.SetIn(_defaultReader);

        _isRunning = false;
        Dispatcher.Free();
    }

    internal void Show(Form form)
    {
        InitializeForm(form);
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
