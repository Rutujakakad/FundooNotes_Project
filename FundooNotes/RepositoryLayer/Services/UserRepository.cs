using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;

using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{  
    public class UserRepository : IUserRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;//this is the interfae which helps you to read the data from appsettings.json

        public UserRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        //every method we create in Repository, we have to pass it in the interface of Repository
        public UserEntity Registration(RegisterModel model)// register method
        {
            UserEntity users = new UserEntity();
            users.FirstName = model.FirstName;
            users.LastName = model.LastName;
            users.DOB = model.DOB;
            users.Email = model.Email;
            users.Password =model.Password;
            users.Password = EncodePassword(model.Password);//method calling
            context.Users.Add(users);
            context.SaveChanges();

            return users;
        }

        
        public static string EncodePassword(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public bool MailExist(string email)
        {
            if (email == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string LoginEmailPassword(string email, string password)
        {
            if (!MailExist(email))
            {

                return null;
            }
            if (password == null)
            {
                return null;
            }
            if (MailExist(email))
            {
                return "Login Successful";
            }
            return email;
        }  
        
        private string GenerateJWTToken(string email, int userID)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", email),
                new Claim("UserID", userID.ToString())
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string Login(LoginModel login) 
        {
            var Checkmail = this.context.Users.FirstOrDefault(a => a.Email == login.Email && a.Password == login.Password);
            if (Checkmail == null)
            {
                string token = GenerateJWTToken(Checkmail.Email, Checkmail.UserID);//pass the parameter as you have passed in GenerateJWTToken otherwise it will give error
                return token;
            }
            return null;
        }

        public ForgotPasswordModel ForgotPassword(string email)
        {
            //var result = funDooDbContext.Users.FirstOrDefault(user => user.Email == email);
            //return (result != null) ? true: false;

            //UserEntity user = fundooDBContext.Users.ToList().Find(user => user.Email == email);

            UserEntity user = GetUserByEmail(email);
            if (user == null)
            {
                ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
                forgotPasswordModel.UserId = user.UserID;
                forgotPasswordModel.Email = user.Email;
                forgotPasswordModel.Token = GenerateJWTToken(user.Email, user.UserID);
                return forgotPasswordModel;
            }
            else
            {
                throw new Exception("User Not Exist for requested email !!!");
            }
            
        }





    }
}
