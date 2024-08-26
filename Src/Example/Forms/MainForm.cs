using ConsoleForms.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Forms;

class MainForm : Form
{
    public override string Name => nameof(MainForm);

    public override void Activate()
    {
        Console.WriteLine("Time to interact with user!");
        Console.WriteLine("What's your name?");
        var name = Console.ReadLine()!;
        Console.WriteLine($"Hello {name}");
        Console.WriteLine("Enter 0 - Close; 1 - Your profile");
        while (true)
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.D0:
                    Close();
                    return;
                case ConsoleKey.D1:
                    Open(new ProfileForm(name));
                    return;
            }
        }
    }

    public override void Draw()
    {
        Console.WriteLine("Hello, friend");
        Console.WriteLine("Here i'm writing some common info of this form");
    }
}
