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
        private static string API_BASE_URL = @"https://api.librus.pl/2.0/";
        private static string API_AUTH_URL = @"https://api.librus.pl/OAuth/Token";
        private static string AUTH_TOKEN = @"MzU6NjM2YWI0MThjY2JlODgyYjE5YTMzZjU3N2U5NGNiNGY=";
        private static string TAG = @"Synergia.NET:SynergiaClient: ";
        #endregion

        #region Variables
        private string accessToken = null;
        private string refreshToken = null;
        private bool isLoggedIn = false;

        private Account account;
        private LuckyNumber lucky;
        private List<Subject> subjects;
        private List<Teacher> teachers;
        private List<Lesson> lessons;
        private List<Event> events;
        private List<EventCategory> eventCategories;

        private Dictionary<string, string> subjectsIdNameDictionary;
        private Dictionary<string, Subject> subjectsIdDictionary;
        private Dictionary<string, string> teacherIdNameDictionary;
        private Dictionary<string, Teacher> teacherIdDictionary;
        private Dictionary<string, EventCategory> eventCategoriesIdDictionary;
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

                    int id = int.Parse(subjectObject.GetValue("Id").ToString());
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

                    int id = int.Parse(lessonObject.GetValue("Id").ToString());
                    int teacherId = int.Parse(lessonObject.SelectToken("Teacher").ToObject<JObject>().GetValue("Id").ToString());
                    int subjectId = int.Parse(lessonObject.SelectToken("Subject").ToObject<JObject>().GetValue("Id").ToString());

                    Lesson lesson = new Lesson(id, teacherId, subjectId);
                    Lessons.Add(lesson);
                }

                this.lessons = Lessons;
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
        #endregion

        #region Utils
        public Dictionary<string, string> GetSubjectsIDNameDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if(subjects == null)
            {
                GetSubjects();
            }

            foreach (Subject s in this.subjects)
            {
                string name = s.name;
                string id = s.id.ToString();
                dictionary.Add(id, name);
            }
            subjectsIdNameDictionary = dictionary;
            return dictionary;
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
                string id = s.id.ToString();
                dictionary.Add(id, s);
            }
            subjectsIdDictionary = dictionary;
            return dictionary;
        }

        public Dictionary<string, string> GetTeachersIDNameDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (teachers == null)
            {
                GetTeachers();
            }

            foreach (Teacher t in this.teachers)
            {
                string name = t.fullName;
                string id = t.id.ToString();
                dictionary.Add(id, name);
            }
            teacherIdNameDictionary = dictionary;
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
                string id = t.id.ToString();
                dictionary.Add(id, t);
            }
            teacherIdDictionary = dictionary;
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
                string id = ec.id.ToString();
                dictionary.Add(id, ec);
            }
            eventCategoriesIdDictionary = dictionary;
            return dictionary;
        }
        #endregion
    }
}