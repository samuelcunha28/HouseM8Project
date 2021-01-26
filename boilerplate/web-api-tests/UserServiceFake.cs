using JWTAuthentication.Models;
using JWTAuthentication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api_tests
{
    class UserServiceFake : IUserService
    {

        private readonly List<User> _users;

        public UserServiceFake()
        {
            _users = new List<User>();
            _users.Add(new User()
            {
                Id = "1",
                UserName = "DevMig",
                Password = "Arroz6969",
                Email = "devmig@microsoft.com",
                Role = "Admin"
            });
            _users.Add(new User()
            {
                Id = "2",
                UserName = "Mig",
                Password = "Arroz6969",
                Email = "mig@microsoft.com",
                Role = "Admin"
            });
        }

        public User Create(User user)
        {
            _users.Add(user);
            return user;
        }

        public List<User> Get()
        {
            return _users.ToList();
        }

        public User Get(string id)
        {
            return _users.Where(a => a.Id == id).FirstOrDefault();
        }

        public User GetByUserName(string userName)
        {
            return _users.Where(a => a.UserName == userName).FirstOrDefault();
        }

        public void Remove(User removedUser)
        {
            var existing = _users.First(a => a.Id == removedUser.Id);
            _users.Remove(existing);
        }

        public void Remove(string id)
        {
            var existing = _users.First(a => a.Id == id);
            _users.Remove(existing);
        }

        public void Update(string id, User newUser)
        {
            var existing = _users.First(a => a.Id == id);
            if (existing != null)
            {
                _users.Remove(existing);
                _users.Add(newUser);
            }
        }
    }
}
