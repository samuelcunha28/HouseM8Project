﻿using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using Xunit;

namespace HouseM8APITests.ProfileTests
{
    [Collection("Sequential")]
    public class CanEmployerRemoveFavoriteMate
    {
        private Connection _connection;
        private ProfileTestsFixture _fixture;

        public CanEmployerRemoveFavoriteMate()
        {
            _fixture = new ProfileTestsFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanEmployerRemoveFavoriteMateTest()
        {
            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Jessica";
            testMate.LastName = "Coelho";
            testMate.UserName = "Jessijow";
            testMate.Password = "123";
            testMate.Email = "beatriz@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Ordem";
            testMate.Categories = new[] { Categories.FURNITURE_ASSEMBLE, Categories.TRANSPORTATION };
            testMate.Rank = Ranks.MATE;
            testMate.Range = 20;

            Mate returnedMate = MateDAO.Create(testMate);

            IEmployerDAO<Employer> employerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Ema";
            testEmployer.LastName = "Coelho";
            testEmployer.UserName = "EmiRegi";
            testEmployer.Password = "123";
            testEmployer.Email = "regina@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lousada";

            Employer returnedEmployer = employerDAO.Create(testEmployer);

            int? returnedId = employerDAO.AddFavorite(returnedEmployer.Id, returnedMate.Id);

            int? deletedID = employerDAO.RemoveFavorite(returnedEmployer.Id, returnedMate.Id);

            Assert.Equal(returnedMate.Id, deletedID);

            _fixture.Dispose();
        }
    }
}
