using ConsoleForms.Core;
using ConsoleForms.Notifications;
using ExampleWithObservers.Forms;

var observersRegister = new ObserversRegister();
//observersRegister.RegisterEventHandler((string m) => m.GetHashCode());   /// You can explicitly register event handler
var notifier = observersRegister.GetNotifier();
var app = new App(new MainForm(), notifier);                               /// and/or you can use form observers that implement ConsoleForms.Notifications.IObserver<TMessage>

var cancellationTokenSource = new CancellationTokenSource();
BackgroundFunction(notifier, cancellationTokenSource.Token);
app.Run();


// Notify every minute in the other thread
void BackgroundFunction(Notifier notifier, CancellationToken cancellationToken)
{
    Task.Run(() =>
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested) break;
            Task.Delay(TimeSpan.FromMinutes(1));
            notifier.Notify(new SomeMessage
            {
                Text = $"Hello, it's {DateTime.Now} already. Have you done your homework?"
            });
        }
    });
}