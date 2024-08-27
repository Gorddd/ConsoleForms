using ConsoleForms.Forms;

namespace ConsoleForms.Core;

internal class FormsStack
{
    private Stack<Form> _forms = new();

    public Form? Current => _forms.TryPeek(out var current) ? current : null;

    public FormsStack()
    {

    }

    public FormsStack(Form rootForm)
    {
        _forms.Push(rootForm);
    }

    public void Up(Form form)
    {
        _forms.Push(form);
    }

    public void Down()
    {
        _forms.Pop();
    }

    public void DownTo(string formName)
    {
        if (!_forms.Any(f => f.Name != formName))
            throw new InvalidOperationException($"There is no {formName} in forms stack");

        while (_forms.Peek().Name != formName)
            _forms.Pop();
    }

    public void DownToRoot()
    {
        if (_forms.Count == 0)
            throw new InvalidOperationException("There is no any form in forms stack");

        while (_forms.Count > 1)
            _forms.Pop();
    }
}
