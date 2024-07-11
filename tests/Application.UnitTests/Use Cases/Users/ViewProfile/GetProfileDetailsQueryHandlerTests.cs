//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Ardalis.GuardClauses;
//using AutoMapper;
//using FitLog.Application.Common.Exceptions;
//using FitLog.Application.Common.Interfaces;
//using FitLog.Application.Users.Queries.GetUserDetails;
//using FitLog.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Xunit;

//namespace FitLog.Application.UnitTests.Users.Queries.GetUserDetails
//{
//    public class GetProfileDetailsQueryHandlerTests
//    {
//        [Fact]
//        public async Task Handle_ValidUserId_ShouldReturnUserProfileDTO()
//        {
//            // Arrange
//            var userId = "test_user_id";
//            var user = new AspNetUser
//            {
//                Id = userId,
//                UserName = "testuser",
//                Email = "testuser@example.com",
//                Programs = new List<Program>
//                {
//                    new Program { ProgramId = 1, ProgramName = "Program 1" },
//                    new Program { ProgramId = 2, ProgramName = "Program 2" }
//                },
//                Certifications = new List<Certification>
//                {
//                    new Certification { CertificationId = 1, CertificationName = "Cert 1", CertificationDateIssued = new DateOnly(2020, 1, 1), CertificationExpirationData = new DateOnly(2023, 1, 1) },
//                    new Certification { CertificationId = 2, CertificationName = "Cert 2", CertificationDateIssued = new DateOnly(2019, 6, 1), CertificationExpirationData = new DateOnly(2022, 6, 1) }
//                },
//                CoachingServices = new List<CoachingService>
//                {
//                    new CoachingService { ServiceName = "Service 1", Description = "Service 1 Description", Duration = 60, Price = 100.0m, ServiceAvailability = true, AvailabilityAnnouncement = "Available" },
//                    new CoachingService { ServiceName = "Service 2", Description = "Service 2 Description", Duration = 45, Price = 80.0m, ServiceAvailability = false, AvailabilityAnnouncement = "Not available" }
//                }
//            };

//            var dbContextMock = new Mock<IApplicationDbContext>();
//            dbContextMock.Setup(x => x.AspNetUsers)
//                .Returns(new MockDbSet<AspNetUser> { user }.Mock);

//            var mapperConfiguration = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile(new UserProfileDTO.Mapping());
//                cfg.AddProfile(new ProgramDTO.Mapping());
//                cfg.AddProfile(new CertificationDTO.Mapping());
//                cfg.AddProfile(new CoachingServiceDTO.Mapping());
//            });
//            var mapper = new Mapper(mapperConfiguration);

//            var request = new GetProfileDetailsRequest { UserId = userId };
//            var handler = new GetProfileDetailsQueryHandler(dbContextMock.Object, mapper);

//            // Act
//            var result = await handler.Handle(request, CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(userId, result.Id);
//            Assert.Equal(user.UserName, result.UserName);
//            Assert.Equal(user.Email, result.Email);
//            Assert.Equal(user.Programs.Count, result.Programs.Count);
//            Assert.Equal(user.Certifications.Count, result.Certifications.Count);
//            Assert.Equal(user.CoachingServices.Count, result.CoachingServices.Count);

//            // Assert ProgramDTO mapping
//            for (int i = 0; i < user.Programs.Count; i++)
//            {
//                Assert.Equal(user.Programs[i].ProgramId, result.Programs[i].ProgramId);
//                Assert.Equal(user.Programs[i].ProgramName, result.Programs[i].ProgramName);
//                // Add more assertions for other fields if needed
//            }

//            // Assert CertificationDTO mapping
//            for (int i = 0; i < user.Certifications.Count; i++)
//            {
//                Assert.Equal(user.Certifications[i].CertificationId, result.Certifications[i].CertificationId);
//                Assert.Equal(user.Certifications[i].CertificationName, result.Certifications[i].CertificationName);
//                // Add more assertions for other fields if needed
//            }

//            // Assert CoachingServiceDTO mapping
//            for (int i = 0; i < user.CoachingServices.Count; i++)
//            {
//                Assert.Equal(user.CoachingServices[i].CoachingServiceId, result.CoachingServices[i].CoachingServiceId);
//                Assert.Equal(user.CoachingServices[i].ServiceName, result.CoachingServices[i].ServiceName);
//                // Add more assertions for other fields if needed
//            }

//            dbContextMock.Verify(x => x.AspNetUsers, Times.Once); // Ensure AspNetUsers DbSet is queried once
//        }

//        [Fact]
//        public async Task Handle_InvalidUserId_ShouldThrowNotFoundException()
//        {
//            // Arrange
//            var userId = "non_existing_user_id";

//            var dbContextMock = new Mock<IApplicationDbContext>();
//            dbContextMock.Setup(x => x.AspNetUsers)
//                .Returns(new MockDbSet<AspNetUser>().Mock);

//            var mapperConfiguration = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile(new UserProfileDTO.Mapping());
//                cfg.AddProfile(new ProgramDTO.Mapping());
//                cfg.AddProfile(new CertificationDTO.Mapping());
//                cfg.AddProfile(new CoachingServiceDTO.Mapping());
//            });
//            var mapper = new Mapper(mapperConfiguration);

//            var request = new GetProfileDetailsRequest { UserId = userId };
//            var handler = new GetProfileDetailsQueryHandler(dbContextMock.Object, mapper);

//            // Act & Assert
//            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
//        }
//    }

//    // Mock DbSet using IEnumerable for mocking DbSet in EF Core
//    public class MockDbSet<TEntity> : Mock<DbSet<TEntity>> where TEntity : class
//    {
//        public MockDbSet<TEntity> Mock
//        {
//            get { return this; }
//        }

//        public override DbSet<TEntity> Object => (DbSet<TEntity>)Object;
//    }
//}
