using Enums;
using HouseM8API.Data_Access;
using HouseM8API.Interfaces;
using HouseM8API.Models;
using Models;
using TestingAPI;
using Xunit;


namespace HouseM8APITests.JobSearch
{
    [Collection("Sequential")]
    public class CanIgnoreValidJob
    {
        private Connection _connection;
        private JobSearchFixture _fixture;

        public CanIgnoreValidJob()
        {
            _fixture = new JobSearchFixture();
            this._connection = _fixture.GetConnection();
        }

        [Fact]
        public void CanIgnoreValidJobTest()
        {
            IEmployerDAO<Employer> EmployerDAO = new EmployerDAO(_connection);
            Employer testEmployer = new Employer();

            testEmployer.FirstName = "Marcelo";
            testEmployer.LastName = "Carvalho";
            testEmployer.UserName = "VeryGoodDev";
            testEmployer.Password = "123";
            testEmployer.Email = "mcsadfsdafsd@gmail.com";
            testEmployer.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testEmployer.Address = "Rua Eng. Luís Carneiro Leão, Nº 54 4590-244 Porto, Portugal";

            Employer returned = EmployerDAO.Create(testEmployer);

            IJobDAO jobPostDAO = new JobDAO(_connection);
            JobPost testPost = new JobPost();

            testPost.Title = "Canalização Estourada";
            testPost.Category = Categories.PLUMBING;
            testPost.Description = "Grande estouro nos canos da sanita";
            testPost.Tradable = true;
            testPost.InitialPrice = 60.6;
            testPost.Address = "Rua de Salgueiros, Penafiel";
            testPost.PaymentMethod = new[] { Payment.PAYPAL, Payment.MONEY };

            JobPost jobReturned = jobPostDAO.Create(returned.Id, testPost);

            IMateDAO<Mate> MateDAO = new MateDAO(_connection);
            Mate testMate = new Mate();

            testMate.FirstName = "Miguel";
            testMate.LastName = "Dev";
            testMate.UserName = "DevMig";
            testMate.Password = "123";
            testMate.Email = "DevMIGmlfdasfdasfg@gmail.com";
            testMate.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
            testMate.Address = "Rua de Salgueiros, Penafiel";
            testMate.Categories = new[] { Categories.CLEANING, Categories.PLUMBING };
            testMate.Rank = Ranks.SUPER_MATE;
            testMate.Range = 20;

            Mate returnedMate = MateDAO.Create(testMate);

            IgnoredJobModel job = new IgnoredJobModel();
            job.Id = jobReturned.Id;

            Assert.True(MateDAO.IgnoreJobPost(returnedMate.Id, job));

            Categories[] categories = { Categories.PLUMBING };
            Assert.Empty(jobPostDAO.GetJobs(categories, testMate.Address, 400, null, returnedMate.Id));

            _fixture.Dispose();
        }
    }
}
