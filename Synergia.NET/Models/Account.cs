namespace Synergia.NET.Models
{
    public class Account
    {
        public string ID { get; }
        public string UserID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Login { get; }
        public bool IsPremium { get; }

        public Account(string id, string userId, string firstName, string lastName, string email, string login, bool isPremium)
        {
            this.ID = id;
            this.UserID = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Login = login;
            this.IsPremium = isPremium;
        }
    }
}
