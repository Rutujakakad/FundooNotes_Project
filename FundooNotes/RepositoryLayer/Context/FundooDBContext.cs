using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions options): base(options){ }
        public DbSet<UserEntity> Users { get; set; } // Users is Table name in database:FundooDatabase 
        public DbSet<NotesEntity> Notes { get; set; } // Notes is Table name in database:FundooDatabase
        public DbSet<LabelEntity> Labels { get; set; }
        public DbSet<CollaboratorEntity> Collaborators { get; set; }
    }
}
