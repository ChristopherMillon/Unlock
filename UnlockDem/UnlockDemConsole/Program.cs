using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnlockDemConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var unlockManager = new UnlockDemManager(
                new Slack.SlackListener(new System.Net.Http.HttpClient()),
                new ParsyBoi.ParseMan(),
                new PowerShell.PowerShellBoi()))
            {
                Console.WriteLine("Press any key to abort...");
                Console.ReadKey();
            };
        }
    }
}
