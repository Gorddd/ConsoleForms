using ConsoleForms.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Notifications;

public class Notifier
{
    private readonly Dictionary<Type, List<Delegate>> _registeredEventHandlers;
    internal FormsStack? FormsStack { get; set; }

    internal Notifier(Dictionary<Type, List<Delegate>> registeredEventHandlers)
    {
        _registeredEventHandlers = registeredEventHandlers;
    }

    /// <summary>
    /// Notify current form if implements IObserver<TMessage> or all the observers that were register in RegisterEventHandler
    /// </summary>
    /// <typeparam name="TMessage">Message type</typeparam>
    /// <param name="message">Message</param>
    public void Notify<TMessage>(TMessage message)
    {
        if (_registeredEventHandlers.TryGetValue(typeof(TMessage), out var handlers))
            handlers.ForEach(h => ((Action<TMessage>)h).Invoke(message));

        if (FormsStack?.Current is Notifications.IObserver<TMessage> currentObserver)
            currentObserver.Update(message);
    }
}
