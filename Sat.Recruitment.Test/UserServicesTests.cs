using System;
using System.Dynamic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Logic;
using Sat.Recruitment.Api.Model.ViewModel;
using Sat.Recruitment.Api.Services.Interfaces;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]

    public class UserFixture
    {
        public UserFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IUserServices, UserServices>();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
    public class UserServicesTests : IClassFixture<UserFixture>
    {
        private readonly IUserServices _userServices;
        public UserServicesTests(UserFixture userFixture)
        {
            _userServices = userFixture.ServiceProvider.GetService<IUserServices>();
        }

        [Fact]
        public async void IsValid_CreateUser_ReturnsUserCreated()
        {
            var userController = new UserController(_userServices);

            UserVM userVM = new UserVM
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };

            var result = await userController.CreateUser(userVM);

            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Description);
        }

        [Fact]
        public async void IsDuplicated_CreateUser_ReturnsUserIsDuplicated()
        {
            var userController = new UserController(_userServices);

            UserVM userVM = new UserVM
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };

            var result = await userController.CreateUser(userVM);

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Description);
        }
    }
}
