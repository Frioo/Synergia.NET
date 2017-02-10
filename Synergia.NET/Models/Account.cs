namespace Synergia.NET.Models
{
    public class Account
    {
        public string id { get; }
        public string userId { get; }
        public string firstName { get; }
        public string lastName { get; }
        public string email { get; }
        public string login { get; }
        public bool isPremium { get; }

        public Account(string id, string userId, string firstName, string lastName, string email, string login, bool isPremium)
        {
            this.id = id;
            this.userId = userId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.login = login;
            this.isPremium = isPremium;
        }
    }
}
