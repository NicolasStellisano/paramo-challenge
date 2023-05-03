using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Model.ViewModel;
using Sat.Recruitment.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sat.Recruitment.Api.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public partial class UserController : ControllerBase
    {

        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        [Route("create-user")]
        public async Task<Result> CreateUser(UserVM userVM)
        {
            return await _userServices.CreateUser(userVM);
        }

    }

}
