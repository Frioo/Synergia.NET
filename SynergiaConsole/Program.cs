using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergia.NET;
using Synergia.NET.Models;
using SynergiaConsole.API;
using Newtonsoft.Json;

namespace SynergiaConsole
{
    class Program
    {
        public static SynergiaClient Client = new SynergiaClient();

        static void Main(string[] args)
        {
            PromptLogin();



            Console.ReadKey(false);
        }

        private static void DisplayMenu()
        {

        }

        private static void PromptLogin()
        {
            LoginCredentials credentials = new LoginCredentials();
            Console.Write("Identyfikator: ");
            credentials.Username = Console.ReadLine();
            Console.Write("Hasło: ");
            credentials.Password = Console.ReadLine();

            Client.Login(credentials.Username, credentials.Password);
            Properties.Settings.Default.LoginCredentials = JsonConvert.SerializeObject(credentials);
        }
    }
}
