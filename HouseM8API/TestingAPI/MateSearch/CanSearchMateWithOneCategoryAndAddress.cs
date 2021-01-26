using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace HouseM8APITests.MateSearch
{
    [Collection("Sequential")]
    public class CanSearchMateWithOneCategoryAndAddress
    {
        private Connection _connection;
        private MateSearchFixture _fixture;

        public CanSearchMateWithOneCategoryAndAddress()
        {
            _fixture = new MateSearchFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanSearchMateWithOneCategoryAndAddressTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);

            Mate firstMate = new Mate();

            firstMate.FirstName = "Marcelo";
            firstMate.LastName = "Carvalho";
            firstMate.UserName = "VeryGoodDev";
            firstMate.Password = "123";
            firstMate.Email = "1@gmail.com";
            firstMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            firstMate.Address = "Sra. Aparecida, Lousada";
            firstMate.Categories = new[] { Categories.CLEANING, Categories.ELECTRICITY };
            firstMate.Rank = Ranks.SUPER_MATE;
            firstMate.Range = 50;

            Mate returnedFirst = MateDAO.Create(firstMate);

            Mate secondMate = new Mate();

            secondMate.FirstName = "Samuel";
            secondMate.LastName = "Cunha";
            secondMate.UserName = "VeryGoodDev";
            secondMate.Password = "123";
            secondMate.Email = "2@gmail.com";
            secondMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            secondMate.Address = "Rua de Salgueiros, Penafiel";
            secondMate.Categories = new[] { Categories.GARDENING, Categories.FURNITURE_ASSEMBLE };
            secondMate.Rank = Ranks.SUPER_MATE;
            secondMate.Range = 50;

            MateDAO.Create(secondMate);

            Categories[] categories = { Categories.CLEANING };
            string myAddress = "Rua de Salgueiros, Penafiel";

            List<Mate> mates = MateDAO.GetMates(categories, myAddress, null, null, null);

            Mate[] matessArray = mates.ToArray();

            Assert.Equal(returnedFirst.FirstName, matessArray[0].FirstName);
            Assert.Equal(returnedFirst.LastName, matessArray[0].LastName);
            Assert.Equal(returnedFirst.UserName, matessArray[0].UserName);
            Assert.Equal(returnedFirst.Email, matessArray[0].Email);
            Assert.Equal(returnedFirst.Description, matessArray[0].Description);
            Assert.Equal(returnedFirst.Address, matessArray[0].Address);
            Array.Reverse(matessArray[0].Categories);
            Assert.Equal(returnedFirst.Categories, matessArray[0].Categories);
            Assert.Equal(returnedFirst.Rank, matessArray[0].Rank);
            Assert.Equal(returnedFirst.Range, matessArray[0].Range);

            Assert.Single(matessArray);

            _fixture.Dispose();
        }
    }
}
