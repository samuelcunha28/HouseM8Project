using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace HouseM8APITests.MateSearch
{
    [Collection("Sequential")]
    public class CanSearchMateWithAllParams
    {
        private Connection _connection;
        private MateSearchFixture _fixture;

        public CanSearchMateWithAllParams()
        {
            _fixture = new MateSearchFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanSearchMateWithAllParamsTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);

            Mate firstMate = new Mate();

            firstMate.FirstName = "Marcelo";
            firstMate.LastName = "Carvalho";
            firstMate.UserName = "VeryGoodDev";
            firstMate.Password = "123";
            firstMate.Email = "cgfgr@gmail.com";
            firstMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            firstMate.Address = "Rua Eng. Luís Carneiro Leão, Figueiró";
            firstMate.Categories = new[] { Categories.CLEANING, Categories.ELECTRICITY };
            firstMate.Rank = Ranks.MATE;
            firstMate.Range = 50;

            Mate returnedFirst = MateDAO.Create(firstMate);
            
            using (SqlCommand cmd = _connection.Fetch().CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE dbo.[User] SET AverageRating = @Ar " +
                    "WHERE Id = @id";

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = returnedFirst.Id;
                cmd.Parameters.Add("@Ar", SqlDbType.NVarChar).Value = 4;

                cmd.ExecuteNonQuery();
            }
            
            Mate secondMate = new Mate();

            secondMate.FirstName = "Samuel";
            secondMate.LastName = "Cunha";
            secondMate.UserName = "VeryGoodDev";
            secondMate.Password = "123";
            secondMate.Email = "34fsdr@gmail.com";
            secondMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            secondMate.Address = "Vila Nova de Gaia";
            secondMate.Categories = new[] { Categories.GARDENING, Categories.FURNITURE_ASSEMBLE };
            secondMate.Rank = Ranks.SUPER_MATE;
            secondMate.Range = 20;

            MateDAO.Create(secondMate);

            Categories[] categories = { Categories.CLEANING, Categories.GARDENING };
            string myAddress = "Rua de Salgueiros, Penafiel";

            List<Mate> mates = MateDAO.GetMates(categories, myAddress, Ranks.MATE, 40, 4);

            Mate[] matesArray = mates.ToArray();

            Assert.Equal(returnedFirst.FirstName, matesArray[0].FirstName);
            Assert.Equal(returnedFirst.LastName, matesArray[0].LastName);
            Assert.Equal(returnedFirst.UserName, matesArray[0].UserName);
            Assert.Equal(returnedFirst.Email, matesArray[0].Email);
            Assert.Equal(returnedFirst.Description, matesArray[0].Description);
            Assert.Equal(returnedFirst.Address, matesArray[0].Address);
            Array.Reverse(matesArray[0].Categories);
            Assert.Equal(returnedFirst.Categories, matesArray[0].Categories);
            Assert.Equal(returnedFirst.Rank, matesArray[0].Rank);
            Assert.Equal(returnedFirst.Range, matesArray[0].Range);

            Assert.Single(matesArray);

            _fixture.Dispose();
        }
    }
}
