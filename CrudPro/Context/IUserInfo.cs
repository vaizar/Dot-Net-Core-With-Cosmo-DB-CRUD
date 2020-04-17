using CrudPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudPro.Context
{
    public interface IUserInfo
    {
        Task<User> UpsertUser(User user);

        Task<bool> CreateDatabase(string name);
        Task<bool> CreateCollection(string dbName, string name);
        Task<bool> CreateDocument(string dbName, string name, User userInfo);
        Task<dynamic> GetData(string dbName, string name);
        Task<User> DeleteUser(string dbName, string name, string id);
    }
}
