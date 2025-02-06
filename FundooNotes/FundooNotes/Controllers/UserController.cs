using System;
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
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;
        private readonly IBus _bus;
        public UserController(IUserManager manager, IBus bus)
        {
            this.manager = manager;
            _bus = bus;
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
                    return BadRequest(new ResponseModel<bool> { Success = true, Message = "Mail Exist", Data = true });
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
            if (login != null)
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
        public IActionResult ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            try
            {
                if(resetPasswordModel.Password == resetPasswordModel.ConfirmPassword)
                {
                    
                    if(manager.ResetPassword(Email, resetPasswordModel))
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
            catch(Exception ex)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Please provide valid password !!!", Data = ex.Message });
            }
        }




    }
}
