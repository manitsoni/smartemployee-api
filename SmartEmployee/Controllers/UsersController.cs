using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SmartEmployee.Controllers
{

    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UsersSQL usersSQL;

        public UsersController(UsersSQL usersSQL)
        {
            this.usersSQL = usersSQL;
        }

        [HttpPost("AddUser")]
        public UsersResult AddUser([FromBody]UsersEntity objUser)
        {
            UsersResult result = new UsersResult();
            result.IsSuccess = true;
            try
            {
                usersSQL.AddEditUser(objUser);
                if (objUser.UserID <= 0)
                {
                    result.Message = "User Added Successfully.";
                }
                else
                {
                    result.Message = "User Update Successfully.";
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }
            return (result);
        }

        [HttpGet("GetUsers")]
        public string GetUsers()
        {
            UsersResult result = new UsersResult();
            var Result = new { result };
            result.IsSuccess = true;
            try
            {
                result.lstUsers = usersSQL.GetUsers();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }
            return JsonConvert.SerializeObject(Result);
        }

        [HttpDelete("DeleteUser")]
        public UsersResult DeleteUser([FromBody]UsersEntity obj)
        {
            UsersResult result = new UsersResult();

            result.IsSuccess = true;
            try
            {
                if (obj.UserID > 0)
                    usersSQL.DeleteUser(obj.UserID);
                else
                {
                    result.Message = "Select User";
                    result.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }
            return (result);
        }



        [HttpPost("Login")]
        public UsersResult UserLogin([FromBody]UserLoginEntity obj)
        {
            UsersResult result = new UsersResult();
            result.IsSuccess = false    ;
            try
            {
                int Status = 0;
                Status = usersSQL.UserLogin(obj.UserName.Trim(), obj.Password, Status);
                if (Status == 1)
                {
                    result.Message = "Login Succesfully.";
                }
                else if (Status == -1)
                {
                    result.Message = "Invalid LoginID or Password.";
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.IsSuccess = false;
            }
            return (result);
        }
    }
}