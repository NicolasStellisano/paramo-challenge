using Newtonsoft.Json;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Model.ViewModel;
using Sat.Recruitment.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Logic
{
    public class UserServices : IUserServices
    {
        private IList<UserVM> _users = new List<UserVM>();

        public async Task<Result> CreateUser(UserVM userVM)
        {
            var errors = "";

            ValidateErrors(userVM, ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Description = errors
                };
            else
            {

                //Normalize email
                userVM.Email = NormalizeEmail(userVM.Email).Result;
                var path = Directory.GetCurrentDirectory() + "/Resources/users.json";
                _users = await ReadJson(path);


                if (_users.Any(x => x.Email == userVM.Email || x.Phone == userVM.Phone
                    || (x.Name == userVM.Name && x.Address == userVM.Address)))
                {
                    Debug.WriteLine("The user is duplicated");
                    await Task.CompletedTask;
                    return new Result()
                    {
                        IsSuccess = false,
                        Description = "The user is duplicated"
                    };
                }
                else
                {
                    switch (userVM.UserType)
                    {
                        case "Normal":
                            if (userVM.Money > 100)
                            {
                                var percentage = Convert.ToDecimal(0.12);
                                //If new user is normal and has more than USD100
                                var gif = userVM.Money * percentage;
                                userVM.Money = userVM.Money + gif;
                            }
                            else if (userVM.Money < 100 && userVM.Money > 10)
                            {
                                var percentage = Convert.ToDecimal(0.8);
                                var gif = userVM.Money * percentage;
                                userVM.Money = userVM.Money + gif;
                            }
                            break;
                        case "SuperUser":
                            if (userVM.Money > 100)
                            {
                                var percentage = Convert.ToDecimal(0.20);
                                var gif = userVM.Money * percentage;
                                userVM.Money = userVM.Money + gif;
                            }
                            break;
                        case "Premium":
                            if (userVM.Money > 100)
                            {
                                var gif = userVM.Money * 2;
                                userVM.Money = userVM.Money + gif;
                            }
                            break;

                    }

                    Debug.WriteLine("User Created");
                    await Task.CompletedTask;
                    return new Result()
                    {
                        IsSuccess = true,
                        Description = "User Created"
                    };
                }
            }
        }

        public async Task<string> NormalizeEmail(string Email)
        {
            var aux = Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            Email = string.Join("@", new string[] { aux[0], aux[1] });

            await Task.CompletedTask;
            return Email;
        }

        public async Task<IList<UserVM>> ReadJson(string fileLocation)
        {
            using (StreamReader r = new StreamReader(fileLocation))
            {
                string json = r.ReadToEnd();
                await Task.CompletedTask;
                return JsonConvert.DeserializeObject<List<UserVM>>(json);
            }
        }

        //Validate errors
        private void ValidateErrors(UserVM user, ref string errors)
        {
            if (user.Name == null)
                //Validate if Name is null
                errors = "The name is required";
            if (user.Email == null)
                //Validate if Email is null
                errors = errors + " The email is required";
            if (user.Address == null)
                //Validate if Address is null
                errors = errors + " The address is required";
            if (user.Phone == null)
                //Validate if Phone is null
                errors = errors + " The phone is required";
        }
    }
}
