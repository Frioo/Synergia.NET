using System;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Synergia.NET.Models;
using NodaTime;
using NodaTime.Text;
using System.Collections.Generic;

namespace Synergia.NET
{
    public class SynergiaClient
    {
        #region Constants
        private static readonly string API_BASE_URL = @"https://api.librus.pl/2.0/";
        private static readonly string API_AUTH_URL = @"https://api.librus.pl/OAuth/Token";
        private static readonly string AUTH_TOKEN = @"MzU6NjM2YWI0MThjY2JlODgyYjE5YTMzZjU3N2U5NGNiNGY=";
        private static readonly string TAG = @"Synergia.NET:SynergiaClient: ";
        #endregion

        #region Variables
        private string accessToken = null;
        private string refreshToken = null;
        private bool isLoggedIn = false;

        private Account account;
        private LuckyNumber lucky;
        private List<Subject> subjects;
        private List<Average> subjectAverages;
        private List<Teacher> teachers;
        private List<Lesson> lessons;
        private List<Event> events;
        private List<EventCategory> eventCategories;
        private List<Attendance> attendances;
        private List<AttendanceCategory> attendanceCategories;
        private List<Grade> grades;
        private List<GradeCategory> gradeCategories;
        private List<GradeComment> gradeComments;
        private List<TextGrade> textGrades;
        List<Announcement> announcements;
        //private List<Message> messages;
        //private MessagesHelper messagesHelper;

        private Dictionary<string, Subject> subjectsIdDictionary;
        private Dictionary<string, Average> subjectAveragesIdDictionary;
        private Dictionary<string, Teacher> teachersIdDictionary;
        private Dictionary<string, Event> eventsIdDictionary;
        private Dictionary<string, EventCategory> eventCategoriesIdDictionary;
        private Dictionary<string, Attendance> attendancesIdDictionary;
        private Dictionary<string, AttendanceCategory> attendanceCategoriesIdDictionary;
        private Dictionary<string, Grade> gradesIdDictionary;
        private Dictionary<string, GradeCategory> gradeCategoriesIdDictionary;
        private Dictionary<string, GradeComment> gradeCommentsIdDictionary;
        private Dictionary<string, Lesson> lessonsIdDictionary;
        #endregion

        #region Core
        public void Login(string username, string password)
        {
            RestClient loginClient = new RestClient();
            loginClient.BaseUrl = new Uri(API_AUTH_URL);

            RestRequest loginRequest = new RestRequest();
            loginRequest.Method = Method.POST;
            loginRequest.AddHeader("Authorization", "Basic " + AUTH_TOKEN);
            loginRequest.AddParameter("username", username);
            loginRequest.AddParameter("password", password);
            loginRequest.AddParameter("grant_type", "password");
            loginRequest.AddParameter("librus_long_term_token", "1");
            loginRequest.AddParameter("librus_rules_accepted", true);
            loginRequest.AddParameter("librus_mobile_rules_accepted", true);

            IRestResponse loginResponse = loginClient.Execute(loginRequest);
            if (loginResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    //Login successful
                    Log("login successful");
                    JObject response = JObject.Parse(loginResponse.Content);
                    accessToken = response.GetValue("access_token").ToString();
                    refreshToken = response.GetValue("refresh_token").ToString();
                    isLoggedIn = true;
                }
                catch(Exception ex)
                {
                    Log("login failed");
                    Log(ex.Message);
                    throw ex;
                }
            }
            else
            {
                Log("login failed");
            }
        }

        private void RefreshAccess()
        {
            RestClient refreshClient = new RestClient();
            refreshClient.BaseUrl = new Uri(API_AUTH_URL);

            RestRequest refreshRequest = new RestRequest();
            refreshRequest.Method = Method.POST;
            refreshRequest.AddParameter("grant_type", "refresh_token");
            refreshRequest.AddParameter("refresh_token", refreshToken);

            IRestResponse refreshResponse = refreshClient.Execute(refreshRequest);
            if(refreshResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    JObject responseObject = JObject.Parse(refreshResponse.Content);

                    accessToken = responseObject.GetValue("access_token").ToString();
                    refreshToken = responseObject.GetValue("refresh_token").ToString();
                }
                catch(Exception ex)
                {
                    Log("Could not refresh access");
                    Log(ex.Message);
                    throw ex;
                }
            }
        }

        private string Request(string endpoint)
        {
            if (!isLoggedIn)
            {
                Log("user is not logged in!");
                throw new Exception("user has to be logged in first");
            }
            RestClient requestClient = new RestClient();
            requestClient.BaseUrl = new Uri(API_BASE_URL + endpoint);

            RestRequest request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Authorization", "Bearer " + refreshToken);

            Log("performing request: " + endpoint);
            IRestResponse response = requestClient.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Request successful
                Log("request successful: " + endpoint);
                return response.Content;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Log("request failed: " + endpoint);
                Log("retrying request: " + endpoint);
                RefreshAccess();
                return Request(endpoint);
            }
            else
            {
                //Request failed
                Log("request failed: " + endpoint);
                throw new Exception("request failed");
            }

        }

        private void Log(String text)
        {
            Debug.WriteLine(TAG + text);
        }
        #endregion

        #region Getters
        public Account GetAccount()
        {
            JObject accountObject = JObject.Parse(Request("/Me")).SelectToken("Me").SelectToken("Account").ToObject<JObject>();

            try
            {
                string id = accountObject.GetValue("Id").ToString();
                string userId = accountObject.GetValue("UserId").ToString();
                string firstName = accountObject.GetValue("FirstName").ToString();
                string lastName = accountObject.GetValue("LastName").ToString();
                string email = accountObject.GetValue("Email").ToString();
                string login = accountObject.GetValue("Login").ToString();
                bool isPremium = bool.Parse(accountObject.GetValue("IsPremium").ToString());

                Account account = new Account(id, userId, firstName, lastName, email, login, isPremium);
                this.account = account;
                return account;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (me/account)");
                Log(ex.Message);
                throw ex;
            }
        }

        public LuckyNumber GetLuckyNumber()
        {
            JObject luckyNumberObject = JObject.Parse(Request("/LuckyNumbers")).SelectToken("LuckyNumber").ToObject<JObject>();

            try
            {
                string number = luckyNumberObject.GetValue("LuckyNumber").ToString();
                LocalDate luckyNumberDay = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(luckyNumberObject.GetValue("LuckyNumberDay").ToString()).Value;

                LuckyNumber luckyNumber = new LuckyNumber(number, luckyNumberDay);
                this.lucky = luckyNumber;
                return luckyNumber;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (lucky numbers)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<Subject> GetSubjects()
        {
            JArray arr = JObject.Parse(Request("/Subjects"))["Subjects"].ToObject<JArray>();
            List<Subject> Subjects = new List<Subject>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject subjectObject = arr[i].ToObject<JObject>();

                    string id = subjectObject.GetValue("Id").ToString();
                    string name = subjectObject.GetValue("Name").ToString();
                    string shortName = subjectObject.GetValue("Short").ToString();
                    int number = int.Parse(subjectObject.GetValue("No").ToString());
                    bool isExtracurricular = bool.Parse(subjectObject.GetValue("IsExtracurricular").ToString());

                    Subject subject = new Subject(id, name, shortName, number, isExtracurricular);
                    Subjects.Add(subject);
                }

                this.subjects = Subjects;
                return Subjects;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (subjects)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<Teacher> GetTeachers()
        {
            JArray arr = JObject.Parse(Request("/Users"))["Users"].ToObject<JArray>();
            List<Teacher> Teachers = new List<Teacher>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject teacherObject = arr[i].ToObject<JObject>();

                    int id = int.Parse(teacherObject.GetValue("Id").ToString());
                    string firstName = teacherObject.GetValue("FirstName").ToString();
                    string lastName = teacherObject.GetValue("LastName").ToString();

                    Teacher teacher = new Teacher(id, firstName, lastName);
                    Teachers.Add(teacher);
                }

                this.teachers = Teachers;
                return Teachers;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (teachers)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<Lesson> GetLessons()
        {
            JArray arr = JObject.Parse(Request("/Lessons"))["Lessons"].ToObject<JArray>();
            List<Lesson> Lessons = new List<Lesson>();

            try
            {
                for(int i = 0; i < arr.Count; i++)
                {
                    JObject lessonObject = arr[i].ToObject<JObject>();

                    string id = lessonObject.GetValue("Id").ToString();
                    string teacherId = lessonObject.SelectToken("Teacher").ToObject<JObject>().GetValue("Id").ToString();
                    string subjectId = lessonObject.SelectToken("Subject").ToObject<JObject>().GetValue("Id").ToString();

                    Lesson lesson = new Lesson(id, teacherId, subjectId);
                    Lessons.Add(lesson);
                }

                lessons = Lessons;
                return Lessons;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (lessons)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<Event> GetEvents()
        {
            JArray arr = JObject.Parse(Request("/HomeWorks"))["HomeWorks"].ToObject<JArray>();
            List<Event> Events = new List<Event>();

            try
            {
                for(int i = 0; i < arr.Count; i++)
                {
                    JObject eventObject = arr[i].ToObject<JObject>();

                    string id = eventObject.GetValue("Id").ToString();
                    string description = eventObject.GetValue("Content").ToString();
                    LocalDate date = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(eventObject.GetValue("Date").ToString()).Value;
                    string eventCategoryId = eventObject.SelectToken("Category").ToObject<JObject>().GetValue("Id").ToString();
                    int lessonNumber = int.Parse(eventObject.GetValue("LessonNo").ToString());
                    string authorId = eventObject.SelectToken("CreatedBy").ToObject<JObject>().GetValue("Id").ToString();
                    LocalDateTime addDate = LocalDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm:ss").Parse(eventObject.GetValue("AddDate").ToString()).Value;

                    Event e = new Event(id, description, date, eventCategoryId, lessonNumber, authorId, addDate);
                    Events.Add(e);
                }
                this.events = Events;
                return Events;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (events)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<EventCategory> GetEventCategories()
        {
            JArray arr = JObject.Parse(Request("/HomeWorks/Categories"))["Categories"].ToObject<JArray>();
            List<EventCategory> EventCategories = new List<EventCategory>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject eventCategoryObject = arr[i].ToObject<JObject>();

                    string id = eventCategoryObject.GetValue("Id").ToString();
                    string name = eventCategoryObject.GetValue("Name").ToString();

                    EventCategory ec = new EventCategory(id, name);
                    EventCategories.Add(ec);
                }
                this.eventCategories = EventCategories;
                return EventCategories;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (event categories)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<Attendance> GetAttendances()
        {
            JArray arr = JObject.Parse(Request("/Attendances"))["Attendances"].ToObject<JArray>();
            List<Attendance> Attendances = new List<Attendance>();

            try
            {
                for(int i = 0; i < arr.Count; i++)
                {
                    JObject attendanceObject = arr[i].ToObject<JObject>();

                    string id = attendanceObject.GetValue("Id").ToString();
                    string lessonId = attendanceObject.SelectToken("Lesson").ToObject<JObject>().GetValue("Id").ToString();
                    // trip
                    LocalDate date = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(attendanceObject.GetValue("Date").ToString()).Value;
                    LocalDateTime addDate = LocalDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm:ss").Parse(attendanceObject.GetValue("AddDate").ToString()).Value;
                    int lessonNumber = int.Parse(attendanceObject.GetValue("LessonNo").ToString());
                    int semesterNumber = int.Parse(attendanceObject.GetValue("Semester").ToString());
                    string typeId = attendanceObject.SelectToken("Type").ToObject<JObject>().GetValue("Id").ToString();
                    string authorId = attendanceObject.SelectToken("AddedBy").ToObject<JObject>().GetValue("Id").ToString();

                    Attendance attendance = new Attendance(id, lessonId, date, addDate, lessonNumber, semesterNumber, typeId, authorId);
                    Attendances.Add(attendance);
                }
                attendances = Attendances;
                return Attendances;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (attendances)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<AttendanceCategory> GetAttendanceCategories()
        {
            JArray arr = JObject.Parse(Request("/Attendances/Types"))["Types"].ToObject<JArray>();
            List<AttendanceCategory> AttendanceCategories = new List<AttendanceCategory>();

            try
            {
                for(int i = 0; i < arr.Count; i++)
                {
                    JObject attendanceCategoryObject = arr[i].ToObject<JObject>();

                    string id = attendanceCategoryObject.GetValue("Id").ToString();
                    string name = attendanceCategoryObject.GetValue("Name").ToString();
                    string shortName = attendanceCategoryObject.GetValue("Short").ToString();
                    bool isPresenceType = bool.Parse(attendanceCategoryObject.GetValue("IsPresenceKind").ToString());

                    AttendanceCategory attendanceCategory = new AttendanceCategory(id, name, shortName, isPresenceType);
                    AttendanceCategories.Add(attendanceCategory);
                }
                attendanceCategories = AttendanceCategories;
                return AttendanceCategories;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (attendance categories)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<Grade> GetGrades()
        {
            JArray arr = JObject.Parse(Request("/Grades"))["Grades"].ToObject<JArray>();
            List<Grade> Grades = new List<Grade>();

            try
            {
                for(int i = 0; i < arr.Count; i++)
                {
                    JObject gradeObject = arr[i].ToObject<JObject>();

                    string id = gradeObject.GetValue("Id").ToString();
                    string lessonId = gradeObject.SelectToken("Lesson").ToObject<JObject>().GetValue("Id").ToString();
                    string subjectId = gradeObject.SelectToken("Subject").ToObject<JObject>().GetValue("Id").ToString();
                    string categoryId = gradeObject.SelectToken("Category").ToObject<JObject>().GetValue("Id").ToString();
                    string authorId = gradeObject.SelectToken("AddedBy").ToObject<JObject>().GetValue("Id").ToString();
                    string grade = gradeObject.GetValue("Grade").ToString();
                    LocalDate date = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(gradeObject.GetValue("Date").ToString()).Value;
                    LocalDateTime addDate = LocalDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm:ss").Parse(gradeObject.GetValue("AddDate").ToString()).Value;
                    int semesterNumber = int.Parse(gradeObject.GetValue("Semester").ToString());
                    bool isConstituent = bool.Parse(gradeObject.GetValue("IsConstituent").ToString());
                    bool isSemesterGrade = bool.Parse(gradeObject.GetValue("IsSemester").ToString());
                    bool isSemesterProposition = bool.Parse(gradeObject.GetValue("IsSemesterProposition").ToString());
                    bool isFinalGrade = bool.Parse(gradeObject.GetValue("IsFinal").ToString());
                    bool isFinalProposition = bool.Parse(gradeObject.GetValue("IsFinalProposition").ToString());
                    string gradeCommentId;
                    if (gradeObject.Property("Comments") != null)
                    {
                        gradeCommentId = gradeObject.SelectToken("Comments").ToObject<JArray>()[0].ToObject<JObject>().GetValue("Id").ToString();
                    }
                    else
                    {
                        gradeCommentId = String.Empty;
                    }

                    Grade g = new Grade(id, lessonId, subjectId, categoryId, authorId, grade, date, addDate, semesterNumber, isConstituent, isSemesterGrade, isSemesterProposition, isFinalGrade, isFinalProposition, gradeCommentId);
                    Grades.Add(g);
                }
                grades = Grades;
                return Grades;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (grades)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<GradeCategory> GetGradeCategories()
        {
            JArray arr = JObject.Parse(Request("/Grades/Categories"))["Categories"].ToObject<JArray>();
            List<GradeCategory> GradeCategories = new List<GradeCategory>();

            try
            {
                for(int i = 0; i < arr.Count; i++)
                {
                    JObject gradeCategoryObject = arr[i].ToObject<JObject>();

                    string id = gradeCategoryObject.GetValue("Id").ToString();
                    string name = gradeCategoryObject.GetValue("Name").ToString();
                    int weight;
                    if(gradeCategoryObject.Property("weight") != null)
                    {
                        weight = int.Parse(gradeCategoryObject.GetValue("Weight").ToString());
                    }
                    else
                    {
                        weight = 0;
                    }
                    GradeCategory gc = new GradeCategory(id, name, weight);
                    GradeCategories.Add(gc);
                }
                gradeCategories = GradeCategories;
                return GradeCategories;
            }
            catch(Exception ex)
            {
                Log("failed to parse response (grade categories)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<GradeComment> GetGradeComments()
        {
            JArray arr = JObject.Parse(Request("/Grades/Comments"))["Comments"].ToObject<JArray>();
            List<GradeComment> result = new List<GradeComment>();

            for (int i = 0; i < arr.Count; i++)
            {
                JObject obj = arr[i].ToObject<JObject>();

                string id = obj.GetValue("Id").ToString();
                string authorId = obj.SelectToken("AddedBy").ToObject<JObject>().GetValue("Id").ToString();
                string gradeId = obj.SelectToken("Grade").ToObject<JObject>().GetValue("Id").ToString();
                string text = obj.GetValue("Text").ToString();

                GradeComment gc = new GradeComment(id, authorId, gradeId, text);
                result.Add(gc);
            }
            gradeComments = result;
            return result;
        }

        public List<Average> GetSubjectAverages()
        {
            JArray arr = JObject.Parse(Request("/Grades/Averages"))["Averages"].ToObject<JArray>();
            List<Average> result = new List<Average>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject averageObject = arr[i].ToObject<JObject>();

                    string subjectId = averageObject.SelectToken("Subject").ToObject<JObject>().GetValue("Id").ToString();
                    string firstSemester = averageObject.GetValue("Semester1").ToString();
                    string secondSemester = averageObject.GetValue("Semester2").ToString();
                    string final = averageObject.GetValue("FullYear").ToString();

                    Average sa = new Average(subjectId, firstSemester, secondSemester, final);
                    result.Add(sa);
                }
                subjectAverages = result;
                return result;
            }
            catch (Exception ex)
            {
                Log("failed to parse response (averages)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<TextGrade> GetTextGrades()
        {
            JArray arr = JObject.Parse(Request("/TextGrades"))["Grades"].ToObject<JArray>();
            List<TextGrade> result = new List<TextGrade>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject textGradeObject = arr[i].ToObject<JObject>();

                    string id = textGradeObject.GetValue("Id").ToString();
                    string lessonId = textGradeObject.SelectToken("Lesson").ToObject<JObject>().GetValue("Id").ToString();
                    string subjectId = textGradeObject.SelectToken("Subject").ToObject<JObject>().GetValue("Id").ToString();
                    string authorId = textGradeObject.SelectToken("AddedBy").ToObject<JObject>().GetValue("Id").ToString();
                    string categoryId = textGradeObject.SelectToken("Category").ToObject<JObject>().GetValue("Id").ToString();
                    string grade = textGradeObject.GetValue("Grade").ToString();
                    LocalDate date = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(textGradeObject.GetValue("Date").ToString()).Value;
                    LocalDateTime addDate = LocalDateTimePattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm:ss").Parse(textGradeObject.GetValue("AddDate").ToString()).Value;
                    int semesterNumber = int.Parse(textGradeObject.GetValue("Semester").ToString());
                    bool isSemester = bool.Parse(textGradeObject.GetValue("IsSemester").ToString());
                    bool isSemesterProposition = bool.Parse(textGradeObject.GetValue("IsSemesterProposition").ToString());
                    bool isFinal = bool.Parse(textGradeObject.GetValue("IsFinal").ToString());
                    bool isFinalProposition = bool.Parse(textGradeObject.GetValue("IsFinalProposition").ToString());

                    TextGrade tg = new TextGrade(id, lessonId, subjectId, authorId, categoryId, grade, date, addDate, semesterNumber, isSemester, isSemesterProposition, isFinal, isFinalProposition);
                    result.Add(tg);
                }
                textGrades = result;
                return result;
            }
            catch (Exception ex)
            {
                Log("failed to parse response (text grades)");
                Log(ex.Message);
                throw ex;
            }
        }

        public List<Announcement> GetAnnouncements()
        {
            JArray arr = JObject.Parse(Request("/SchoolNotices"))["SchoolNotices"].ToObject<JArray>();
            List<Announcement> res = new List<Announcement>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject announcementObject = arr[i].ToObject<JObject>();

                    string id = announcementObject.GetValue("Id").ToString();
                    LocalDate start = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(announcementObject.GetValue("StartDate").ToString()).Value;
                    LocalDate end = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(announcementObject.GetValue("EndDate").ToString()).Value;
                    string subject = announcementObject.GetValue("Subject").ToString();
                    string content = announcementObject.GetValue("Content").ToString();
                    string authorId = announcementObject.SelectToken("AddedBy").ToObject<JObject>().GetValue("Id").ToString();

                    Announcement a = new Announcement(id, start, end, subject, content, authorId);
                    res.Add(a);
                }
                announcements = res;
                return res;
            }
            catch (Exception ex)
            {
                Log("failed to parse response (announcements)");
                Log(ex.Message);
                throw ex;
            }
        }

        /*private List<Message> GetMessages()
        {
            JObject root = JObject.Parse(Request("Messages"));
            JObject pagesInfo = root["Pages"].ToObject<JObject>();
            JArray messageArray = root["Messages"].ToObject<JArray>();

            // Pages info
            int limit = int.Parse(pagesInfo.GetValue("Limit").ToString());
            int currentPage = int.Parse(pagesInfo.GetValue("CurrentPage").ToString());
            int next = int.Parse(pagesInfo.GetValue("Next").ToString().Replace(@"http://api.librus.pl/2.0/Messages/?", String.Empty));
            int totalPages = int.Parse(pagesInfo.GetValue("AllPages").ToString());
            int totalMessages = int.Parse(pagesInfo.GetValue("AllMessages").ToString());

            // Messages
            for (int i = 0; i < messageArray.Count; i++)
            {
                JObject messageObj = messageArray[i].ToObject<JObject>();
                string id = messageObj.GetValue("Id").ToString();
                bool isNote = bool.Parse(messageObj.GetValue("IsNote").ToString());
                bool isDeleted = bool.Parse(messageObj.GetValue("IsTrash").ToString());
                bool isSent = bool.Parse(messageObj.GetValue("IsSended").ToString());
                bool isReceived = bool.Parse(messageObj.GetValue("IsReceived").ToString());
                bool isArchived = bool.Parse(messageObj.GetValue("IsArchive").ToString());
                bool isSpam = bool.Parse(messageObj.GetValue("IsSpam").ToString());
                string receiverId = messageObj.SelectToken("Receiver").ToObject<JObject>().GetValue("Id").ToString();
                string senderId = messageObj.SelectToken("Sender").ToObject<JObject>().GetValue("Id").ToString();
                int sendDate = int.Parse(messageObj.GetValue("SendDate").ToString());
                int readDate = int.Parse(messageObj.SelectToken("ReadDates").ToObject<JArray>()[0].ToObject<JObject>().GetValue("ReadDate").ToString());
            }
        }*/
        #endregion

        #region Utils
        public enum Limit
        {
            Default = 10
        }

        public Dictionary<string, Subject> GetSubjectsIDDictionary()
        {
            Dictionary<string, Subject> dictionary = new Dictionary<string, Subject>();
            if(subjects == null)
            {
                GetSubjects();
            }

            foreach(Subject s in this.subjects)
            {
                string id = s.ID.ToString();
                dictionary.Add(id, s);
            }
            subjectsIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, Teacher> GetTeachersIDDictionary()
        {
            Dictionary<string, Teacher> dictionary = new Dictionary<string, Teacher>();
            if(teachers == null)
            {
                GetTeachers();
            }

            foreach(Teacher t in this.teachers)
            {
                string id = t.ID.ToString();
                dictionary.Add(id, t);
            }
            teachersIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, Event> GetEventsIDDictionary()
        {
            Dictionary<string, Event> dictionary = new Dictionary<string, Event>();
            if (events == null)
            {
                GetEvents();
            }

            foreach(Event e in this.events)
            {
                string id = e.ID.ToString();
                dictionary.Add(id, e);
            }
            eventsIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, EventCategory> GetEventCategoriesIDDictionary()
        {
            Dictionary<string, EventCategory> dictionary = new Dictionary<string, EventCategory>();
            if(eventCategories == null)
            {
                GetEventCategories();
            }

            foreach(EventCategory ec in this.eventCategories)
            {
                string id = ec.ID.ToString();
                dictionary.Add(id, ec);
            }
            eventCategoriesIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, Attendance> GetAttendancesIDDictionary()
        {
            Dictionary<string, Attendance> dictionary = new Dictionary<string, Attendance>();
            if(attendances == null)
            {
                GetAttendances();
            }

            foreach(Attendance a in this.attendances)
            {
                string id = a.ID.ToString();
                dictionary.Add(id, a);
            }
            attendancesIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, AttendanceCategory> GetAttendanceCategoriesIDDictionary()
        {
            Dictionary<string, AttendanceCategory> dictionary = new Dictionary<string, AttendanceCategory>();
            if (attendanceCategories == null)
            {
                GetAttendanceCategories();
            }

            foreach (AttendanceCategory ac in this.attendanceCategories)
            {
                string id = ac.ID;
                dictionary.Add(id, ac);
            }
            attendanceCategoriesIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, Grade> GetGradesIDDictionary()
        {
            Dictionary<string, Grade> dictionary = new Dictionary<string, Grade>();
            if(gradeCategories == null)
            {
                GetGrades();
            }

            foreach(Grade g in this.grades)
            {
                string id = g.ID;
                dictionary.Add(id, g);
            }
            gradesIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, GradeCategory> GetGradeCategoriesIDDictionary()
        {
            Dictionary<string, GradeCategory> dictionary = new Dictionary<string, GradeCategory>();
            if(gradeCategories == null)
            {
                GetGradeCategories();
            }

            foreach(GradeCategory gc in this.gradeCategories)
            {
                string id = gc.ID;
                dictionary.Add(id, gc);
            }
            gradeCategoriesIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, GradeComment> GetGradeCommentsIDDictionary()
        {
            Dictionary<string, GradeComment> dictionary = new Dictionary<string, GradeComment>();
            if (gradeComments == null)
            {
                GetGradeComments();
            }

            foreach (GradeComment gc in this.gradeComments)
            {
                string id = gc.ID;
                dictionary.Add(id, gc);
            }
            /*
             * Not all grades have a comment nor a comment property in server's JSON response,
             * this ensures that an empty string is returned (instead of an exception being thrown)
             *  when we try to get a comment for a grade that does not have one.
             */
            dictionary.Add(String.Empty, new GradeComment(String.Empty, String.Empty, String.Empty, String.Empty));
            gradeCommentsIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, Average> GetSubjectAveragesIDDictionary()
        {
            Dictionary<string, Average> dictionary = new Dictionary<string, Average>();
            if (subjectAverages == null)
            {
                GetSubjectAverages();
            }

            foreach (Average sa in this.subjectAverages)
            {
                string id = sa.SubjectID;
                dictionary.Add(id, sa);
            }
            subjectAveragesIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, Lesson> GetLessonsIDDictionary()
        {
            Dictionary<string, Lesson> dictionary = new Dictionary<string, Lesson>();
            if (lessons == null)
            {
                GetLessons();
            }

            foreach (Lesson l in lessons)
            {
                string id = l.ID;
                dictionary.Add(id, l);
            }
            lessonsIdDictionary = dictionary;
            return dictionary;
        }
        #endregion
    }
}