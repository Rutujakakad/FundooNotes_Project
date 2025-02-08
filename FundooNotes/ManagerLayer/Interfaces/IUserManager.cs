using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        
        public UserEntity Registration(RegisterModel model);
        public bool MailExist(string email);
        //public string LoginEmailPassword(string email, string password);
        public string Login(LoginModel login);

        public ForgotPasswordModel ForgotPassword(string email);
        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);


    }
}
