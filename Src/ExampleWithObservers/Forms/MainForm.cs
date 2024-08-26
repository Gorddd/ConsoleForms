using ConsoleForms.Core;
using ConsoleForms.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleWithObservers.Forms;

class MainForm : Form, ConsoleForms.Notifications.IObserver<SomeMessage>
{
    public override string Name => nameof(MainForm);

    public override void Activate()
    {
        while (true)
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.D0:
                    Close();
                    return;
            }
        }
    }

    public override void Draw()
    {
        Console.WriteLine("Enter 0 - close");
    }

    public void Update(SomeMessage message)
    {
        Dispatcher.Invoke(() =>
        {
            Console.Clear();
            Console.WriteLine($"You got some message: {message.Text}");
            Draw();
        });
    }
}

class SomeMessage
{
    public string Text { get; set; } = null!;
}