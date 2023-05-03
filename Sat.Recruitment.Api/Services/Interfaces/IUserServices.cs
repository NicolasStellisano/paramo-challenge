using Microsoft.Net.Http.Headers;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services.Interfaces
{
    public interface IUserServices
    {
        public Task<Result> CreateUser(UserVM user);
        public Task<IList<UserVM>> ReadJson(string fileLocation);
        public Task<string> NormalizeEmail(string Email);
    }
}
