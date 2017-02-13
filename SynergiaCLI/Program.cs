using System;
using System.Collections.Generic;
using Synergia.NET;
using Synergia.NET.Models;

namespace SynergiaCLI
{
    class Program
    {
        #region Variables
        private static SynergiaClient client = new SynergiaClient();
        private static Account account;
        private static LuckyNumber lucky;
        private static List<Subject> subjects;
        private static List<Teacher> teachers;
        private static List<Lesson> lessons;
        private static List<Event> events;
        private static List<Attendance> attendances;
        private static List<Grade> grades;
        private static List<SubjectAverage> averages;
        #endregion

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
            // Clear console screen and write header text (+ add an empty line at the bottom)
            Console.Clear();
            Console.WriteLine("Synergia.NET API test project\n");
        }

        private static void displayMenu()
        {
            //Display menu options
            Console.WriteLine("1. Account");
            Console.WriteLine("2. Lucky number");
            Console.WriteLine("3. Subjects");
            Console.WriteLine("4. Teachers");
            Console.WriteLine("5. Lessons");
            Console.WriteLine("6. Events");
            Console.WriteLine("7. Attendances");
            Console.WriteLine("8. Grades");
            Console.WriteLine("9. Averages");

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

                case '7':
                    displayHeader();
                    displayAttendances();
                    displayFooter();
                    break;

                case '8':
                    displayHeader();
                    displayGrades();
                    displayFooter();
                    break;

                case '9':
                    displayHeader();
                    displayAverages();
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
            // Write bottom menu options
            Console.WriteLine("\n1. Back");
            Console.WriteLine("2. Exit");

            // Get input and do proper actions
            char input = Console.ReadKey(false).KeyChar;
            switch(input)
            {
                // User chose '1' so we display the main menu combo
                case '1':
                    displayHeader();
                    displayMenu();
                    break;

                // User chose '2' our program exits
                case '2':
                    Environment.Exit(0);
                    break;
            }
        }


        private static void displayAccount()
        {
            Console.WriteLine("Account:");
            if (account == null)
            {
                account = client.GetAccount();
            }
            Console.WriteLine("Name: " + account.firstName + " " + account.lastName);
            Console.WriteLine("Login: " + account.login);
            Console.WriteLine("Premium status: " + account.isPremium);
        }

        private static void displayLuckyNumber()
        {
            if (lucky == null)
            {
                lucky = client.GetLuckyNumber();
            }
            Console.WriteLine("Lucky number: " + lucky.Number);
            Console.WriteLine("Lucky number day: " + lucky.LuckyNumberDay.ToString());
        }

        private static void displaySubjects()
        {
            Console.WriteLine("Subjects:");
            if (subjects == null)
            {
                subjects = client.GetSubjects();
            }
            Dictionary<string, Subject> subjectDictionary = client.GetSubjectsIDDictionary();
            for (int i = 0; i < subjects.Count; i++)
            {
                string name = subjects[i].name;
                string id = subjects[i].id.ToString();
                Console.WriteLine("{0} (id: {1})", subjectDictionary[id].name, id);
            }
        }

        private static void displayTeachers()
        {
            Console.WriteLine("Teachers:");
            if (teachers == null)
            {
                teachers = client.GetTeachers();
            }
            Dictionary<string, Teacher> teacherDictionary = client.GetTeachersIDDictionary();
            for (int i = 0; i < teachers.Count; i++)
            {
                string name = teachers[i].fullName;
                string id = teachers[i].id.ToString();
                Console.WriteLine("{0} (id: {1})", teacherDictionary[id].fullName, id);
            }
        }

        private static void displayLessons()
        {
            Console.WriteLine("Lessons:");
            if (lessons == null)
            {
                lessons = client.GetLessons();
            }
            Dictionary<string, Teacher> teachersDictionary = client.GetTeachersIDDictionary();
            Dictionary<string, Subject> subjectDictionary = client.GetSubjectsIDDictionary();
            for(int i = 0; i < lessons.Count; i++)
            {
                string teacherName = teachersDictionary[lessons[i].teacherId.ToString()].fullName;
                string subjectName = subjectDictionary[lessons[i].subjectId].name;
                Console.WriteLine("{0} - {1}", subjectName, teacherName);
            }
        }

        private static void displayEvents()
        {
            Console.WriteLine("Events:");
            if (events == null)
            {
                events = client.GetEvents();
            }
            Dictionary<string, Teacher> teachersDictionary = client.GetTeachersIDDictionary();
            Dictionary<string, EventCategory> eventCategoriesDictionary = client.GetEventCategoriesIDDictionary();
            for(int i = 0; i < events.Count; i++)
            {
                string date = events[i].date.ToString();
                string id = events[i].id.ToString();
                string category = eventCategoriesDictionary[events[i].eventCategoryId].name;
                Console.WriteLine("{0} - {1}(id: {2})", category, date, id);
            }
        }

        private static void displayAttendances()
        {
            Console.WriteLine("Attendances:");
            if (attendances == null)
            {
                attendances = client.GetAttendances();
            }
            Dictionary<string, AttendanceCategory> attendanceCategoriesDictionary = client.GetAttendanceCategoriesIDDictionary();
            for(int i = 0; i < attendances.Count; i++)
            {
                string category = attendanceCategoriesDictionary[attendances[i].typeId].name;
                string date = attendances[i].date.ToString();
                string id = attendances[i].id;
                Console.WriteLine("{0} - {1} (id: {2})", category, date, id);
            }
        }

        private static void displayGrades()
        {
            Console.WriteLine("Grades:");
            if (grades == null)
            {
                grades = client.GetGrades();
            }
            Dictionary<string, GradeCategory> gradeCategoriesDictionary = client.GetGradeCategoriesIDDictionary();
            Dictionary<string, GradeComment> gradeCommentsDictionary = client.GetGradeCommentsIDDictionary();
            for(int i = 0; i < grades.Count; i++)
            {
                string grade = grades[i].grade.ToString();
                string category = gradeCategoriesDictionary[grades[i].categoryId].name;
                string comment = gradeCommentsDictionary[grades[i].GradeCommentID].Text;
                string date = grades[i].date.ToString();
                string id = grades[i].id;
                Console.WriteLine($"Grade: {grade} (id: {id})");
                Console.WriteLine($"Category: {category}");
                Console.WriteLine($"Comment: {comment}");
                Console.WriteLine($"Date: {date}" + Environment.NewLine);
            }
        }

        private static void displayAverages()
        {
            Console.WriteLine("Averages:");
            if (averages == null)
            {
                averages = client.GetSubjectAverages();
            }
            Dictionary<string, Subject> sd = client.GetSubjectsIDDictionary();
            for (int i = 0; i < averages.Count; i++)
            {
                string name = sd[averages[i].SubjectID].name;
                string id = averages[i].SubjectID;
                string firstSemester = averages[i].FirstSemester;
                string secondSemeter = averages[i].SecondSemester;
                string final = averages[i].FullYear;
                Console.WriteLine($"Subject: {name} (id: {id})");
                Console.WriteLine($"Fist semester: {firstSemester}");
                Console.WriteLine($"Second semester: {secondSemeter}");
                Console.WriteLine($"Final: {final}" + Environment.NewLine);
            }
        }
    }
}
