using ConsoleForms.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForms.Forms;

public abstract class Form
{
    private readonly Form? _rootForm;

    protected Form(Form? rootForm = null)
    {
        _rootForm = rootForm;
    }

    public void BackTo(string formName)
    {
        if (Name == formName)
        {
            Dispatcher.CurrentForm = this;
            return;
        }
        if (_rootForm == null)
            throw new InvalidOperationException($"There is no {formName} in the ui forms three");

        _rootForm.BackTo(formName);
    }

    public void BackToFirst()
    {
        if (_rootForm == null)
        {
            Dispatcher.CurrentForm = this;
            return;
        }
        _rootForm.BackToFirst();
    }

    /// <summary>
    /// Name of the form
    /// </summary>
    public abstract string Name { get; set; }

    /// <summary>
    /// Show the form
    /// </summary>
    public abstract void Show();
}
