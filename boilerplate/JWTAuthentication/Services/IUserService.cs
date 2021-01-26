using JWTAuthentication.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace JWTAuthentication.Services
{
    public interface IUserService
    {
        public List<User> Get();

        public User Get(string id);

        public User GetByUserName(string userName);

        public User Create(User user);

        public void Update(string id, User newUser);

        public void Remove(User removedUser);

        public void Remove(string id);
    }

}