using AutoMapper;
using HouseM8API.Entities;
using HouseM8API.Models;
using HouseM8API.Models.Users;
using Models;

namespace HouseM8API.Configs
{
    /// <summary>
    /// Classe para fazer mapeamento entre modelos
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Método que contém os modelos para os quais são mapeados
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<EmployerRegister, Employer>();

            CreateMap<EmployerUpdate, Employer>();
            CreateMap<Employer, EmployerUpdate>();

            CreateMap<MateUpdate, Mate>();
            CreateMap<Mate, MateUpdate>();

            CreateMap<MateRegister, Mate>();

            CreateMap<Mate, MateModel>();
            CreateMap<Employer, EmployerModel>();

            CreateMap<JobPost, JobPostModel>();
            CreateMap<JobPost, JobPostReturnedModel>();
            CreateMap<JobPostModel, JobPost>();

            CreateMap<OfferModel, Offer>();
            CreateMap<Offer, OfferModel>();

            CreateMap<MateModelExtended, Mate>();
            CreateMap<Mate, MateModelExtended>();

            CreateMap<EmployerProfileModel, Employer>();
            CreateMap<Employer, EmployerProfileModel>();

            CreateMap<MateProfileModel, Mate>();
            CreateMap<Mate, MateProfileModel>();

            CreateMap<WorkModel, Job>();
            CreateMap<Job, WorkModel>();

            CreateMap<ReviewsModel, Review>();
            CreateMap<Review, ReviewsModel>();

            CreateMap<MateReviewsModel, MateReview>();
            CreateMap<MateReview, MateReviewsModel>();

            CreateMap<ReportModel, Report>();
            CreateMap<Report, ReportModel>();

            CreateMap<PasswordUpdateModel, User>();
            CreateMap<User, PasswordUpdateModel>();

            CreateMap<InvoiceModel, Invoice>();
            CreateMap<Invoice, InvoiceModel>();

            CreateMap<MateRegister, MateModel>();
            CreateMap<MateModel, MateRegister>();
        }
    }
}
