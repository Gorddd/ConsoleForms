using ConsoleForms.Core;
using ConsoleForms.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Forms;

public abstract class Form
{
    /// <summary>
    /// Name of the form
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Activate the form
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Draw the form
    /// </summary>
    public abstract void Draw();

    internal App App { get; set; } = null!;

    protected void Open(Form form)
    {
        App.Show(form);
    }

    protected void Close()
    {
        App.Back();
    }

    protected void BackTo(string formName)
    {
        App.BackTo(formName);
    }

    protected void BackToRoot()
    {
        App.BackToRoot();
    }

    /// <summary>
    /// Not null if App was created with Notifier
    /// </summary>
    protected internal Notifier? Notifier { get; internal set; }
}
