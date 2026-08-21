using System.Collections.Generic;

namespace KisApp.App_Code
{
    public class UserList
    {
        public List<UserAccount> Users { get; set; } = new List<UserAccount>();

        public UserAccount GetUser(string username)
        {
            return Users.FirstOrDefault(u => u.Username == username);
        }

        public bool Authenticate(string username, string password)
        {
            var user = Users.FirstOrDefault(u => u.Username == username && u.Name == password);
            return user != null;
        }
    }
}