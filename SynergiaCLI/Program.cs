﻿using System;
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
            Dictionary<string, Teacher> td = client.GetTeachersIDDictionary();
            for (int i = 0; i < tl.Count; i++)
            {
                string name = tl[i].fullName;
                string id = tl[i].id.ToString();
                Console.WriteLine("{0} (id: {1})", td[id].fullName, id);
            }
        }

        private static void displayLessons()
        {
            Console.WriteLine("Lessons:");
            List<Lesson> ll = client.GetLessons();
            Dictionary<string, Teacher> td = client.GetTeachersIDDictionary();
            Dictionary<string, Subject> sd = client.GetSubjectsIDDictionary();
            for(int i = 0; i < ll.Count; i++)
            {
                string teacherName = td[ll[i].teacherId.ToString()].fullName;
                string subjectName = sd[ll[i].subjectId].name;
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

        private static void displayAttendances()
        {
            Console.WriteLine("Attendances:");
            List<Attendance> al = client.GetAttendances();
            Dictionary<string, AttendanceCategory> acd = client.GetAttendanceCategoriesIDDictionary();
            for(int i = 0; i < al.Count; i++)
            {
                string category = acd[al[i].typeId].name;
                string date = al[i].date.ToString();
                string id = al[i].id;
                Console.WriteLine("{0} - {1} (id: {2})", category, date, id);
            }
        }

        private static void displayGrades()
        {
            Console.WriteLine("Grades:");
            List<Grade> gl = client.GetGrades();
            Dictionary<string, GradeCategory> gcad = client.GetGradeCategoriesIDDictionary();
            Dictionary<string, GradeComment> gcod = client.GetGradeCommentsIDDictionary();
            for(int i = 0; i < gl.Count; i++)
            {
                string grade = gl[i].grade.ToString();
                string category = gcad[gl[i].categoryId].name;
                string comment = gcod[gl[i].GradeCommentID].Text;
                string date = gl[i].date.ToString();
                string id = gl[i].id;
                Console.WriteLine($"Grade: {grade} (id: {id})");
                Console.WriteLine($"Category: {category}");
                Console.WriteLine($"Comment: {comment}");
                Console.WriteLine($"Date: {date}" + Environment.NewLine);
            }
        }

        private static void displayAverages()
        {
            Console.WriteLine("Averages:");
            List<SubjectAverage> sal = client.GetSubjectAverages();
            Dictionary<string, Subject> sd = client.GetSubjectsIDDictionary();
            for (int i = 0; i < sal.Count; i++)
            {
                string name = sd[sal[i].SubjectID].name;
                string id = sal[i].SubjectID;
                string firstSemester = sal[i].FirstSemester;
                string secondSemeter = sal[i].SecondSemester;
                string final = sal[i].FullYear;
                Console.WriteLine($"Subject: {name} (id: {id})");
                Console.WriteLine($"Fist semester: {firstSemester}");
                Console.WriteLine($"Second semester: {secondSemeter}");
                Console.WriteLine($"Final: {final}" + Environment.NewLine);
            }
        }
    }
}
