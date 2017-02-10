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

        private Dictionary<string, string> subjectsIdNameDictionary;
        private Dictionary<string, string> teacherIdNameDictionary;
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
                catch
                {
                    Log("login failed");
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
            JObject accountObject = (JObject)JObject.Parse(Request("/Me")).SelectToken("Me").SelectToken("Account");

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
            catch
            {
                Log("failed to parse response (me/account)");
                throw new Exception("parsing response failed");
            }
        }

        public LuckyNumber GetLuckyNumber()
        {
            JObject luckyNumberObject = (JObject)JObject.Parse(Request("/LuckyNumbers")).SelectToken("LuckyNumber");

            try
            {
                string number = luckyNumberObject.GetValue("LuckyNumber").ToString();
                LocalDate luckyNumberDay = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd").Parse(luckyNumberObject.GetValue("LuckyNumberDay").ToString()).Value;

                LuckyNumber luckyNumber = new LuckyNumber(number, luckyNumberDay);
                this.lucky = luckyNumber;
                return luckyNumber;
            }
            catch
            {
                Log("failed to parse response (lucky numbers)");
                throw new Exception("parsing response failed");
            }
        }

        public List<Subject> GetSubjects()
        {
            JArray arr = (JArray)JObject.Parse(Request("/Subjects"))["Subjects"];
            List<Subject> Subjects = new List<Subject>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject subjectObject = (JObject)arr[i];

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
            catch
            {
                Log("failed to parse response (subjects)");
                throw new Exception("parsing response failed");
            }
        }

        public List<Teacher> GetTeachers()
        {
            JArray arr = (JArray)JObject.Parse(Request("/Users"))["Users"];
            List<Teacher> Teachers = new List<Teacher>();

            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    JObject teacherObject = (JObject)arr[i];

                    int id = int.Parse(teacherObject.GetValue("Id").ToString());
                    string firstName = teacherObject.GetValue("FirstName").ToString();
                    string lastName = teacherObject.GetValue("LastName").ToString();

                    Teacher teacher = new Teacher(id, firstName, lastName);
                    Teachers.Add(teacher);
                }

                this.teachers = Teachers;
                return Teachers;
            }
            catch
            {
                Log("failed to parse response (teachers)");
                throw new Exception("parsing response failed");
            }
        }
        #endregion

        #region Utils
        public Dictionary<string, string> GetSubjectsIDNameDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (subjects == null)
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
        #endregion
    }
}