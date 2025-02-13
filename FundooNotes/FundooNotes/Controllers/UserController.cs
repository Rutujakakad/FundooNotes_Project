using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLayer.Models;
using FundooNotes.Helpers;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
//using RepositoryLayer.Migrations;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase //ControllerBase is used for OK and BadRequest
    {
        private readonly FundooDBContext context;
        private readonly IUserManager manager;
        private readonly IBus _bus;
        public UserController(IUserManager manager, IBus bus, FundooDBContext context)
        {
            this.manager = manager;
            this._bus = bus;
            this.context = context;
        }



        [HttpPost]
        [Route("Reg")]

        public IActionResult Register(RegisterModel model)
        {

            try
            {
                var checkMail = manager.MailExist(model.Email);
                if (checkMail)
                {
                    return BadRequest(new ResponseModel<bool> { Success = true, Message = "Mail already Exist", Data = false });
                }
                else
                {
                    var result = manager.Registration(model);// calling the register method
                    if (result != null)
                    {
                        return Ok(new ResponseModel<UserEntity> { Success = true, Message = "Register Successful", Data = result });
                    }
                    else
                    {
                        return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Register failed" });
                    }
                }
            }
            catch (AppException ex)
            {
                //logger.LogCritical(ex, " Exception Thrown...");
                return NotFound(new { success = false, message = ex.Message });
            }

        }

        //var loginemailpswrd = manager.Login(model.Email, model.Password);//
        //if (loginemailpswrd)
        //{
        //    return BadRequest(new ResponseModel<string> { Success = true, Message = "Login is successful" });
        //}


        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel login)
        {
            var loginPage = manager.Login(login);//calling the login method ()
            if (loginPage != null)
            {
                return Ok(new ResponseModel<string> { Success = true, Message = "Login is successful", Data = loginPage });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Success = false, Message = "Login failed" });
            }

        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                ForgotPasswordModel forgotPasswordModel = manager.ForgotPassword(email);
                Send send = new Send();
                send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(forgotPasswordModel);
                return Ok(new ResponseModel<string> { Success = true, Message = "Mail Sent Successfully", Data = "Token has been sent to your mail to reset password" });
            }
            catch (AppException ex)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Please provide valid email !!!", Data = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            try
            {
                string Email = User.FindFirstValue("Email");
                if (resetPasswordModel.Password == resetPasswordModel.ConfirmPassword)
                {

                    if (manager.ResetPassword(Email, resetPasswordModel))
                    {
                        return Ok(new ResponseModel<bool> { Success = true, Message = "User reset password is successful", Data = true });
                    }
                    else
                        return BadRequest(new ResponseModel<bool> { Success = false, Message = "User reset password is failed", Data = false });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "User reset password is failed", Data = "Password missmatched" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Please provide valid password !!!", Data = ex.Message });
            }
        }
        //-----------------------review session
        //[Authorize]
        //[HttpGet]
        //[Route("GetUsers")]
        //public IActionResult GetAllUsers()
        //{
        //    try
        //    {
        //        var getAllUsers = context.Users.ToList();
        //        if (getAllUsers.Count == 0)
        //        {
        //            return BadRequest(new ResponseModel<string> { Success = false, Message = "User Not Found" });
        //        }
        //        else
        //        {
        //            return Ok(new ResponseModel<string> { Success = true, Message = "Got All Users" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "No Data Found!!!", Data = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("getUseById")]
        //public IActionResult GetUserById(int UserID)
        //{
        //    try
        //    {
        //        var userById = context.Users.FirstOrDefault(a => a.UserID == UserID);
        //        if (userById != null)
        //        {
        //            return Ok(new ResponseModel<string> { Success = true, Message = "User Found Successfully" });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<string> { Success = false, Message = "User Not Found" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "User Does Not Exist", Data = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("nameStartingWith")]
        //public IActionResult NameStartingWithA()
        //{
        //    try
        //    {
        //        var userNameStartsWith = context.Users.Where(u => u.FirstName.StartsWith("A")).ToList();
        //        if (userNameStartsWith.Any())
        //        {
        //            return Ok(new ResponseModel<string> { Success = true, Message = "User Found Successfully" });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<string> { Success = false, Message = "User Does Not Exist Name Starting with 'A' "});
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "User Does Not Exist", Data = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("totalUsers")]
        //public IActionResult TotalUsersCount()
        //{
        //    try
        //    {
        //        var totalCount = context.Users.Count();
                
        //        if (totalCount > 0)
        //        {
        //            return Ok(new ResponseModel<int> { Success = true, Message = $"Total Count Of User is{totalCount} " });
        //        }
        //        if (totalCount < 0)
        //        {
        //            return BadRequest(new ResponseModel<int> { Success = false, Message = $"Total Count Of User is 0" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "Error occurred while fetching user count", Data = ex.Message });
                
        //    }
        //    return null;
        //}

        //[HttpGet]
        //[Route("nameByOrder")]
        //public IActionResult NameByOrder()
        //{
        //    try
        //    {
        //        var nameOrderBy = context.Users.OrderBy(u => u.FirstName).ToList();
        //        if(nameOrderBy.Count != null)
        //        {
        //            return Ok(new ResponseModel<string> { Success = true, Message = "Got All the UsersName OrderBy Successfully" }); 
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<string> { Success = false, Message = "No User Found"});
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "Error Occured", Data = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("nameByOrder")]
        //public IActionResult NameByOrderDesc()
        //{
        //    try
        //    {
        //        var nameOrderBy = context.Users.OrderByDescending(u => u.FirstName).ToList();
        //        if (nameOrderBy.Count != null)
        //        {
        //            return Ok(new ResponseModel<string> { Success = true, Message = "Got All the UsersName OrderBy Descending Successfully" });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<string> { Success = false, Message = "No User Found" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "Error Occured", Data = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("avgAgeOfUser")]

        //public IActionResult AvgAgeOfUsers()
        //{
        //    try
        //    {
        //        var avgAge = context.Users.ToList();
        //        double averageAge = context.Users.Average(a => DateTime.Today.Year - a.DOB.Year);
        //        if (avgAge.Count != 0)
        //        {
        //            return Ok(new ResponseModel<double> { Success = true, Message = "Got the Average Age of User Successfully" });
        //        }
        //         else
        //         {
                   
        //            return BadRequest(new ResponseModel<double> { Success = false, Message = "Error Occured" });
        //         }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "Error Occured", Data = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("youngestUser")]
        //public IActionResult YoungestUser()
        //{
        //    try
        //    {
        //       // int youngestUser = context.Users.Min(m => DateTime.Today.Year - m.DateOfBirth.Year);
        //        int youngestUser = context.Users.Min(m => DateTime.Today.Year - m.DOB.Year);
        //        if(youngestUser > 0)
        //        {
        //            return Ok(new ResponseModel<string> { Success = true, Message = $"Youngest user is: {youngestUser}" });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<string> { Success = false, Message = "No User Found" });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "Error Occured", Data = ex.Message });
        //    }

        //}


        //[HttpGet]
        //[Route("oldestUser")]
        //public IActionResult OldestUserAge()
        //{
        //    try
        //    {

        //        var oldestAgeUser = context.Users.Max(x => DateTime.Today.Year - x.DOB.Year);
        //        if (oldestAgeUser > 0)
        //        {
        //            return Ok(new ResponseModel<string> { Success = true, Message = $"Oldest user is: {oldestAgeUser}" });
        //        }
        //        else
        //        {
        //            return BadRequest(new ResponseModel<string> { Success = false, Message = "No User Found" });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { Success = false, Message = "Error Occured", Data = ex.Message });
        //    }

        //}







    }
}
