using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        public NotesEntity CreateNote(int UserID, NotesModel notesModel);
        public List<NotesEntity> GetNotes(int UserID);
        public NotesEntity NotesUpdate(int UserID, UpdateNotesModel updateNotesModel);
        public bool NotesDelete(int UserID, DeleteNotesModel deleteNotesModel);

    }
}
