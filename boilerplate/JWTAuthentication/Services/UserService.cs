using JWTAuthentication.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace JWTAuthentication.Services
{
    public class UserService : IUserService
    {

        private readonly IMongoCollection<User> _users;

        public UserService(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.CollectionName);
        }

        //Explorar Projection
        //https://mongodb.github.io/mongo-csharp-driver/2.11/getting_started/quick_tour/
        public List<User> Get() => _users.Find(user => true).ToList();

        public User Get(string id) => _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetByUserName(string userName) => _users.Find<User>(user => user.UserName == userName).FirstOrDefault();

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User newUser) => _users.ReplaceOne(user => user.Id == id, newUser);

        public void Remove(User removedUser) => _users.DeleteOne(user => user.Id == removedUser.Id);

        public void Remove(string id) => _users.DeleteOne(user => user.Id == id);

        //Explorar AutoMapper
        //https://stackoverflow.com/questions/64828386/how-can-i-implement-a-dto-for-my-service
    }
}