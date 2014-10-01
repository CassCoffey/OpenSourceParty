using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSourceParty
{
    public class Runner
    {
        public static void Main()
        {
            MainMenu menu = new MainMenu();
            menu.ShowDialog();
        }
    }
}
