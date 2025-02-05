using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Grnder {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
