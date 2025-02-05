using System;
using System.Threading.Tasks;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;
        public UserController(IUserManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("Reg")]

        public IActionResult Register(RegisterModel model)
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


            //var loginemailpswrd = manager.Login(model.Email, model.Password);//
            //if (loginemailpswrd)
            //{
            //    return BadRequest(new ResponseModel<string> { Success = true, Message = "Login is successful" });
            //}

            var login = manager.Login(login);//calling the login method ()
            if (login != null)
            {
                return BadRequest(new ResponseModel<string> { Success = true, Message = "Login is successful", Data = login });
            }
        }


        //[HttpGet("ForgotPassword")]
        //public async Task<IActionResult> ForgetPassword(string email)
        //{
        //    try
        //    {
        //        ForgotPasswordModel forgotPasswordModel = manager.ForgotPassword(email);
        //        Send send = new Send();
        //        send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
        //        Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");
        //        var endPoint = await bus.GetSendEndpoint(uri);
        //        await endPoint.Send(forgotPasswordModel);
        //        return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Mail Sent Successfully", Data = "Token has been sent to your mail to reset password" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Please provide valid email !!!", Data = ex.Message });
        //    }
        //}



        
    }
}
