using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class CollaboratorManager : ICollaboratorManager
    {
        private readonly ICollaboratorRepository icollaborator;
        public CollaboratorManager(ICollaboratorRepository icollaborator)
        {
            this.icollaborator = icollaborator;
        }

        public CollaboratorEntity AddCollaborator(string Email, int NotesId, int UserID)
        {
            return icollaborator.AddCollaborator(Email, NotesId, UserID);
        }
        public List<CollaboratorEntity> GetCollaboratorsByUserId(int userId)
        {
            return icollaborator.GetCollaboratorsByUserId(userId);
        }
        public bool DeleteCollaborator(int CollaboratorId)
        {
            return icollaborator.DeleteCollaborator(CollaboratorId);
        }
            
    }
}
