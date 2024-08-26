using ConsoleForms.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Forms;

class ProfileForm : Form
{
    private string _username;

    public ProfileForm(string username)
    {
        _username = username;
    }

    public override string Name => nameof(ProfileForm);

    public override void Activate()
    {
        Console.WriteLine("What do you want?");
        Console.WriteLine("Enter 0 - Back");
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
        Console.WriteLine($"Hello {_username}!");
    }
}
