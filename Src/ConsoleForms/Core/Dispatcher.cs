using ConsoleForms.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Core;

public static class Dispatcher
{
    private static object locker = new();
    private static Form _currentForm = null!;
    internal static Form CurrentForm
    {
        get => _currentForm;
        set
        {
            lock (locker)
            {
                _currentForm = value;
            }
        }
    }


}
