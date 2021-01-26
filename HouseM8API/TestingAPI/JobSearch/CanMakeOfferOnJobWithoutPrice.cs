using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using Models;
using TestingAPI;
using Xunit;

namespace HouseM8APITests.JobSearch
{
    [Collection("Sequential")]
    public class CanMakeOfferOnJobWithoutPrice
    {

        private Connection _connection;
        private JobSearchFixture _fixture;

        public CanMakeOfferOnJobWithoutPrice()
        {
            _fixture = new JobSearchFixture();
            this._connection = _fixture.GetConnection();
        }
        [Fact]
        public void CanMakeOfferOnJobWithoutPriceTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "mc@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Lixa";

            Employer returned = EmployerDAO.Create(testEmployer);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Estourada";
            testPost.Category = Categories.PLUMBING;
            testPost.Description = "Grande estouro nos canos da sanita";
            testPost.Tradable = true;
            testPost.InitialPrice = 60.6;
            testPost.Address = "Rua sem fim";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            JobPost jobReturned = jobPostDAO.Create(returned.Id, testPost);

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlg@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Figueiró";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returnedMate = MateDAO.Create(testMate);

            Offer mateOffer = new Offer();
            mateOffer.Price = 0;
            mateOffer.JobPost = jobReturned;
            Offer offer = jobPostDAO.makeOfferOnJob(mateOffer, returnedMate.Id);

            //Verificar que o preço foi estabelicido com o default
            Assert.Equal(testPost.InitialPrice, offer.Price);
            Assert.Equal(mateOffer.JobPost.Id, offer.JobPost.Id);
            Assert.False(offer.Approved);

            _fixture.Dispose();
        }
    }
}
