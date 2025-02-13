using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface ICollaboratorManager
    {
        public CollaboratorEntity AddCollaborator(string Email, int NotesId, int UserID);
        public List<CollaboratorEntity> GetCollaboratorsByUserId(int userId);
        public bool DeleteCollaborator(int CollaboratorId);
    }
}
