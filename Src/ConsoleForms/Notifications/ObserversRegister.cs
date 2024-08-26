using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Notifications;

public class ObserversRegister
{
    private readonly Dictionary<Type, List<Delegate>> _registeredEventHandlers = new();

    /// <summary>
    /// Explicitly declare action that will be called on event with TMessage
    /// </summary>
    /// <typeparam name="TMessage">Message type</typeparam>
    /// <param name="onEvent">Action to call when an event occurs</param>
    public void RegisterEventHandler<TMessage>(Action<TMessage> onEvent)
    {
        if (_registeredEventHandlers.TryGetValue(typeof(TMessage), out var handlers))
            handlers.Add(onEvent);
        else
            _registeredEventHandlers.Add(typeof(TMessage), new List<Delegate> { onEvent });
    }

    public Notifier GetNotifier()
    {
        return new Notifier(_registeredEventHandlers);
    }
}
