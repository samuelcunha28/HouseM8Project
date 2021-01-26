--CREATE DATABASE housem8DB;

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--Table Achievements
CREATE TABLE [dbo].[Achievements](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Achievements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Table Categories
CREATE TABLE [dbo].[Categories](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Table Ranks
CREATE TABLE [dbo].[Ranks](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Ranks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Table Roles
CREATE TABLE [dbo].[Roles](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Table PaymentType
CREATE TABLE [dbo].[PaymentType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_PaymentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Table User
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_User_2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_EMAIL] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO

ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Roles]
GO

--Table Mate
CREATE TABLE [dbo].[Mate](
	[Id] [int] NOT NULL,
	[Range] [int] NOT NULL,
	[RankId] [int] NOT NULL,
 CONSTRAINT [PK_Mate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Mate]  WITH CHECK ADD  CONSTRAINT [FK_Mate_Ranks] FOREIGN KEY([RankId])
REFERENCES [dbo].[Ranks] ([Id])
GO

ALTER TABLE [dbo].[Mate] CHECK CONSTRAINT [FK_Mate_Ranks]
GO

ALTER TABLE [dbo].[Mate]  WITH CHECK ADD  CONSTRAINT [FK_User_Mate] FOREIGN KEY([Id])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[Mate] CHECK CONSTRAINT [FK_User_Mate]
GO

--Table Employer
CREATE TABLE [dbo].[Employer](
	[Id] [int] NOT NULL,
 CONSTRAINT [PK_Employer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Employer]  WITH CHECK ADD  CONSTRAINT [FK_User_Employer] FOREIGN KEY([Id])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[Employer] CHECK CONSTRAINT [FK_User_Employer]
GO

--Table CategoriesFromM8
CREATE TABLE [dbo].[CategoriesFromM8](
	[MateId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_CategoriesFromM8] PRIMARY KEY CLUSTERED 
(
	[MateId] ASC,
	[CategoryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CategoriesFromM8]  WITH CHECK ADD  CONSTRAINT [FK_CategoriesFromM8_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO

ALTER TABLE [dbo].[CategoriesFromM8] CHECK CONSTRAINT [FK_CategoriesFromM8_Categories]
GO

ALTER TABLE [dbo].[CategoriesFromM8]  WITH CHECK ADD  CONSTRAINT [FK_CategoriesFromM8_Mate] FOREIGN KEY([MateId])
REFERENCES [dbo].[Mate] ([Id])
GO

ALTER TABLE [dbo].[CategoriesFromM8] CHECK CONSTRAINT [FK_CategoriesFromM8_Mate]
GO

--Table EmployerReview
CREATE TABLE [dbo].[EmployerReview](
	[Id] [int] NOT NULL,
	[rating] [float] NOT NULL,
	[EmployerId] [int] NOT NULL,
 CONSTRAINT [PK_EmployerReview] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployerReview]  WITH CHECK ADD  CONSTRAINT [FK_EmployerReview_Employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[Employer] ([Id])
GO

ALTER TABLE [dbo].[EmployerReview] CHECK CONSTRAINT [FK_EmployerReview_Employer]
GO

--Table Favourites
CREATE TABLE [dbo].[Favourites](
	[EmployerId] [int] NOT NULL,
	[MateId] [int] NOT NULL,
 CONSTRAINT [PK_Favourites] PRIMARY KEY CLUSTERED 
(
	[EmployerId] ASC,
	[MateId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Favourites]  WITH CHECK ADD  CONSTRAINT [FK_Favourites_Employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[Employer] ([Id])
GO

ALTER TABLE [dbo].[Favourites] CHECK CONSTRAINT [FK_Favourites_Employer]
GO

ALTER TABLE [dbo].[Favourites]  WITH CHECK ADD  CONSTRAINT [FK_Favourites_Mate] FOREIGN KEY([MateId])
REFERENCES [dbo].[Mate] ([Id])
GO

ALTER TABLE [dbo].[Favourites] CHECK CONSTRAINT [FK_Favourites_Mate]
GO

--Table Invoice
CREATE TABLE [dbo].[Invoice](
	[Id] [int] NOT NULL,
	[Value] [float] NOT NULL,
	[PaymentTypeId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_PaymentType] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentType] ([Id])
GO

ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_PaymentType]
GO

--Table Jobposts
CREATE TABLE [dbo].[JobPosts](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Category] [int] NOT NULL,
	[ImagePath] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Tradable] [bit] NOT NULL,
	[InitialPrice] [float] NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[EmployerId] [int] NOT NULL,
 CONSTRAINT [PK_JobPosts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[JobPosts]  WITH CHECK ADD  CONSTRAINT [FK_JobPosts_Categories] FOREIGN KEY([Category])
REFERENCES [dbo].[Categories] ([Id])
GO

ALTER TABLE [dbo].[JobPosts] CHECK CONSTRAINT [FK_JobPosts_Categories]
GO

ALTER TABLE [dbo].[JobPosts]  WITH CHECK ADD  CONSTRAINT [FK_JobPosts_Employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[Employer] ([Id])
GO

ALTER TABLE [dbo].[JobPosts] CHECK CONSTRAINT [FK_JobPosts_Employer]
GO

--Table Job
CREATE TABLE [dbo].[Job](
	[Id] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[MateId] [int] NOT NULL,
	[PaymentId] [int] NULL,
	[JobPostId] [int] NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[Finished] [bit] NOT NULL,
	[EmployerId] [int] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Employer] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[Employer] ([Id])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Employer]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Invoice] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([Id])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Invoice]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_JobPosts] FOREIGN KEY([JobPostId])
REFERENCES [dbo].[JobPosts] ([Id])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_JobPosts]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_MateId] FOREIGN KEY([MateId])
REFERENCES [dbo].[Mate] ([Id])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_MateId]
GO

--Table MateAchievements
CREATE TABLE [dbo].[MateAchievements](
	[MateId] [int] NOT NULL,
	[AchievementId] [int] NOT NULL,
 CONSTRAINT [PK_MateAchievements] PRIMARY KEY CLUSTERED 
(
	[MateId] ASC,
	[AchievementId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MateAchievements]  WITH CHECK ADD  CONSTRAINT [FK_MateAchievements_Achievements] FOREIGN KEY([AchievementId])
REFERENCES [dbo].[Achievements] ([Id])
GO

ALTER TABLE [dbo].[MateAchievements] CHECK CONSTRAINT [FK_MateAchievements_Achievements]
GO

ALTER TABLE [dbo].[MateAchievements]  WITH CHECK ADD  CONSTRAINT [FK_MateAchievements_Mate] FOREIGN KEY([MateId])
REFERENCES [dbo].[Mate] ([Id])
GO

ALTER TABLE [dbo].[MateAchievements] CHECK CONSTRAINT [FK_MateAchievements_Mate]
GO

--Table MateReview
CREATE TABLE [dbo].[MateReview](
	[Id] [int] NOT NULL,
	[comment] [nvarchar](max) NULL,
	[MateId] [int] NOT NULL,
	[rating] [float] NOT NULL,
 CONSTRAINT [PK_MateReview] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MateReview]  WITH CHECK ADD  CONSTRAINT [FK_MateReview_Mate] FOREIGN KEY([MateId])
REFERENCES [dbo].[Mate] ([Id])
GO

ALTER TABLE [dbo].[MateReview] CHECK CONSTRAINT [FK_MateReview_Mate]
GO

--Table Offer
CREATE TABLE [dbo].[Offer](
	[Id] [int] NOT NULL,
	[approved] [bit] NULL,
	[price] [float] NOT NULL,
	[MateId] [int] NOT NULL,
	[JobPostId] [int] NOT NULL,
 CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Offer]  WITH CHECK ADD  CONSTRAINT [FK_Offer_JobPosts] FOREIGN KEY([JobPostId])
REFERENCES [dbo].[JobPosts] ([Id])
GO

ALTER TABLE [dbo].[Offer] CHECK CONSTRAINT [FK_Offer_JobPosts]
GO

ALTER TABLE [dbo].[Offer]  WITH CHECK ADD  CONSTRAINT [FK_Offer_Mate1] FOREIGN KEY([MateId])
REFERENCES [dbo].[Mate] ([Id])
GO

ALTER TABLE [dbo].[Offer] CHECK CONSTRAINT [FK_Offer_Mate1]
GO

--Table PaymentJob
CREATE TABLE [dbo].[PaymentJob](
	[JobPostId] [int] NOT NULL,
	[PaymentTypeId] [int] NOT NULL,
 CONSTRAINT [PK_PaymentJob] PRIMARY KEY CLUSTERED 
(
	[JobPostId] ASC,
	[PaymentTypeId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentJob]  WITH CHECK ADD  CONSTRAINT [FK_PaymentJob_JobPosts] FOREIGN KEY([JobPostId])
REFERENCES [dbo].[JobPosts] ([Id])
GO

ALTER TABLE [dbo].[PaymentJob] CHECK CONSTRAINT [FK_PaymentJob_JobPosts]
GO

ALTER TABLE [dbo].[PaymentJob]  WITH CHECK ADD  CONSTRAINT [FK_PaymentJob_PaymentType] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentType] ([Id])
GO

ALTER TABLE [dbo].[PaymentJob] CHECK CONSTRAINT [FK_PaymentJob_PaymentType]
GO


