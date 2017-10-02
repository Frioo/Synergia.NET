using Synergia.NET;
using Synergia.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynergiaConsole.API
{
   public class DataRepository
    {
        public static  Account Account { get; private set; }
        public static LuckyNumber LuckyNumber { get; private set; }

        public static List<Grade> Grades { get; private set; }
        public static List<GradeCategory> GradeCategories { get; private set; }
        public static List<GradeComment> GradeComments { get; private set; }
        public static List<TextGrade> TextGrades { get; private set; }

        public static List<Attendance> Attendances { get; private set; }

        public static List<AttendanceCategory> AttendanceCategories { get; private set; }
        public static List<Announcement> Announcements { get; private set; }

        public static List<Event> Events { get; private set; }
        public static List<EventCategory> EventCategories { get; private set; }

        public static List<Teacher> Teachers { get; private set; }
        public static List<Subject> Subjects { get; private set; }
        public static List<Lesson> Lessons { get; private set; }
        public static List<Average> Averages { get; private set; }

        public DataRepository()
        {
            DownloadData();
        }

        public static void DownloadData()
        {
            SynergiaClient client = Program.Client;
            Account = client.GetAccount();
            LuckyNumber = client.GetLuckyNumber();
        }
    }
}
