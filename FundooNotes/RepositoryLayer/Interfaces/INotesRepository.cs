using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepository
    {
        public NotesEntity CreateNote(int UserID, NotesModel notesModel);
        public List<NotesEntity> GetNotes(int UserID);
        public NotesEntity NotesUpdate(int UserID, UpdateNotesModel updateNotesModel);
        public bool NotesDelete(int UserID, DeleteNotesModel deleteNotesModel);


    }
}
