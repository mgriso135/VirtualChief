using System;

namespace KisApp.App_Code
{
    public class UserAccount
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Tenant { get; set; }

        public bool ValidatePermissions(string activeWorkspace, string[][] permissions)
        {
            // Simplified permission check for .NET 8
            foreach (var perm in permissions)
            {
                if (perm.Length >= 2 && Role == perm[1] && activeWorkspace.Contains(perm[0]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}