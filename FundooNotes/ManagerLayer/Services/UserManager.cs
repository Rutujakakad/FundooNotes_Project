using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository user;
        private readonly FundooDBContext context;
        public UserManager(IUserRepository user, FundooDBContext context)
        {
            this.user = user;
            this.context = context;
        }
        public UserEntity Registration(RegisterModel model)
        {
            return user.Registration(model);
        }

        public bool MailExist(string email)
        {
            return user.MailExist(email);
        }

        public string LoginEmailPassword(string email, string password)
        {
            return user.LoginEmailPassword(email, password);
        }

        public string Login(LoginModel login)
        {
            return user.Login(login);
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            return user.ForgotPassword(email);
        }
    }
}
