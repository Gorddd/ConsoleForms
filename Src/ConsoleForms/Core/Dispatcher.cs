using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Core;

/// <summary>
/// Allows to use Console stream
/// </summary>
public static class Dispatcher
{
    private static ConcurrentQueue<Action> _actions = new();
    private static SemaphoreSlim _semaphore = new(0);

    internal static void Free()
    {
        _semaphore.Release();
    }

    internal static void ExecuteRequestedActions()
    {
        _semaphore.Wait();

        Action? actionToInvoke = null;
        while (_actions.Count > 0 && !_actions.TryDequeue(out actionToInvoke)) ;
        actionToInvoke?.Invoke();
    }

    /// <summary>
    /// Adds action to queue to call in safe thread
    /// </summary>
    /// <param name="action"></param>
    public static void Invoke(Action action)
    { 
        _actions.Enqueue(action);
        _semaphore.Release();
    }
}
