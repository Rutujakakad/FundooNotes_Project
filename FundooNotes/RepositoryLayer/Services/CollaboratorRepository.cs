using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;

namespace RepositoryLayer.Services
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
         private readonly FundooDBContext fundooDBContext;
        private CollaboratorEntity collaborator;

        public CollaboratorRepository(FundooDBContext fundooDBContext)
        {
            this.fundooDBContext = fundooDBContext;
        }

        public CollaboratorEntity AddCollaborator(string Email, int NotesId, int UserID)
        {
            var collaborator = new CollaboratorEntity();
            collaborator.Email = Email;
            collaborator.NotesId = NotesId;
            collaborator.UserID = UserID;
           
            fundooDBContext.Collaborators.Add(collaborator);
            fundooDBContext.SaveChanges();

            return collaborator;


             ////check collaborator exist or not
            //var CollaboratingUser = fundooDBContext.Users.FirstOrDefault(u => u.Email == Email);
            //if (CollaboratingUser == null)
            //{
            //    return null;
            //}
            //    //get note
            //var MyNote = fundooDBContext.Notes.FirstOrDefault(a => a.NotesId == NotesId && a.UserID == UserID);
            //if (MyNote == null)
            //{ 
            //    return null;
            //}
            //if (CollaboratingUser.UserID == UserID) //to prevents self-collaboration
            //{
            //    return null;
            //}

            ////check collaborator alrady exist or not
            //var collaborator = fundooDBContext.Collaborators.FirstOrDefault(a => a.Email == Email && a.NotesId == NotesId);
            //if (collaborator != null)
            //{
            //    return collaborator; // Collaborator already exists
            //}


            //if(collaborator == null)
            //{
            //    CollaboratorEntity collaboratorEntity = new CollaboratorEntity();
            //    collaboratorEntity.Email = Email;
            //    collaboratorEntity.NotesId = NotesId;
            //    collaboratorEntity.UserID = CollaboratingUser.UserID;

            //    fundooDBContext.Collaborators.Add(collaboratorEntity);
            //    fundooDBContext.SaveChanges();

            //}

            //return collaborator;
        }
    
            
            
        

        public List<CollaboratorEntity> GetCollaboratorsByUserId(int userId)
        {
            var listOfCollaborators = fundooDBContext.Collaborators.Where(c => c.UserID == userId).ToList();
            return listOfCollaborators;
        }

        public bool DeleteCollaborator(int CollaboratorId)
        {

            var existingCollaborator = fundooDBContext.Collaborators.FirstOrDefault(n => n.CollaboratorId == CollaboratorId);
            if (existingCollaborator == null)
            {
                return false;
            }

            fundooDBContext.Collaborators.Remove(existingCollaborator);
            fundooDBContext.SaveChanges();

            return true;
        }


    }
}
