using Concrete;
using Entities;
using System;
using System.Collections.Generic;

namespace Interface
{
    public interface IAdminService
    {
        User Login(string username, string password);
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User CreateUser(User user, string password);
        void UpdateUser(User user, string password = null);
        void DeleteUser(int id);
    }
}
