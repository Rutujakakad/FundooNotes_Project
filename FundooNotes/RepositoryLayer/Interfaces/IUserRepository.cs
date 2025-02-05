using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    { //every method we create in Repository, we have to pass it in the interface of Repository
        public UserEntity Registration(RegisterModel model);
        
        public bool MailExist(string email);
        public string Login(LoginModel login);
        string LoginEmailPassword(string username, string password);
        public ForgotPasswordModel ForgotPassword(string email);
    }

   
}
