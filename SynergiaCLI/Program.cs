using System;
using System.Collections.Generic;
using Synergia.NET;
using Synergia.NET.Models;

namespace SynergiaCLI
{
    class Program
    {
        private static SynergiaClient client = new SynergiaClient();

        static void Main(string[] args)
        {
            // Display header and get user credentials
            displayHeader();
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            // Create a new instance and login to Synergia
            client.Login(username, password);

            // Display main screen
            displayHeader();
            displayMenu();
        }

        private static void displayHeader()
        {
            Console.Clear();
            Console.WriteLine("Synergia.NET API test project\n");
        }

        private static void displayMenu()
        {
            Console.WriteLine("1. Account");
            Console.WriteLine("2. Lucky number");
            Console.WriteLine("3. Subjects");
            Console.WriteLine("4. Teachers");
            Console.WriteLine("5. Subject - teacher map (lessons)");
            Console.WriteLine("6. Events");

            // Get input and display proper data
            char input = Console.ReadKey(false).KeyChar;
            switch (input)
            {
                case '1':
                    displayHeader();
                    displayAccount();
                    displayFooter();
                    break;

                case '2':
                    displayHeader();
                    displayLuckyNumber();
                    displayFooter();
                    break;

                case '3':
                    displayHeader();
                    displaySubjects();
                    displayFooter();
                    break;

                case '4':
                    displayHeader();
                    displayTeachers();
                    displayFooter();
                    break;
                
                case '5':
                    displayHeader();
                    displayLessons();
                    displayFooter();
                    break;

                case '6':
                    displayHeader();
                    displayEvents();
                    displayFooter();
                    break;

                default:
                    displayHeader();
                    displayMenu();
                    break;
            }
        }

        private static void displayFooter()
        {
            Console.WriteLine("\n1. Back");
            Console.WriteLine("2. Exit");

            char input = Console.ReadKey(false).KeyChar;
            switch(input)
            {
                case '1':
                    displayHeader();
                    displayMenu();
                    break;

                case '2':
                    Environment.Exit(0);
                    break;
            }
        }

        private static void displayAccount()
        {
            Account a = client.GetAccount();
            Console.WriteLine("Account name: " + a.firstName + " " + a.lastName);
            Console.WriteLine("Account login: " + a.login);
            Console.WriteLine("Premium status: " + a.isPremium);
        }

        private static void displayLuckyNumber()
        {
            LuckyNumber ln = client.GetLuckyNumber();
            Console.WriteLine("Lucky number: " + ln.Number);
            Console.WriteLine("Lucky number day: " + ln.LuckyNumberDay.ToString());
        }

        private static void displaySubjects()
        {
            Console.WriteLine("Subjects:");
            List<Subject> sl = client.GetSubjects();
            Dictionary<string, Subject> sd = client.GetSubjectsIDDictionary();
            for (int i = 0; i < sl.Count; i++)
            {
                string name = sl[i].name;
                string id = sl[i].id.ToString();
                Console.WriteLine("{0} (id: {1})", sd[id].name, id);
            }
        }

        private static void displayTeachers()
        {
            Console.WriteLine("Teachers:");
            List<Teacher> tl = client.GetTeachers();
            Dictionary<string, string> td = client.GetTeachersIDNameDictionary();
            for (int i = 0; i < tl.Count; i++)
            {
                string name = tl[i].fullName;
                string id = tl[i].id.ToString();
                Console.WriteLine("{0} (id: {1})", td[id], id);
            }
        }

        private static void displayLessons()
        {
            Console.WriteLine("Lessons:");
            List<Lesson> ll = client.GetLessons();
            Dictionary<string, Teacher> td = client.GetTeachersIDDictionary();
            Dictionary<string, string> sd = client.GetSubjectsIDNameDictionary();
            for(int i = 0; i < ll.Count; i++)
            {
                string teacherName = td[ll[i].teacherId.ToString()].fullName;
                string subjectName = sd[ll[i].subjectId.ToString()];
                Console.WriteLine("{0} - {1}", subjectName, teacherName);
            }
        }

        private static void displayEvents()
        {
            Console.WriteLine("Events:");
            List<Event> el = client.GetEvents();
            Dictionary<string, Teacher> td = client.GetTeachersIDDictionary();
            Dictionary<string, EventCategory> ecd = client.GetEventCategoriesIDDictionary();
            for(int i = 0; i < el.Count; i++)
            {
                string date = el[i].date.ToString();
                string id = el[i].id.ToString();
                string category = ecd[el[i].eventCategoryId].name;
                Console.WriteLine("{0} - {1}(id: {2})", category, date, id);
            }
        }
    }
}
