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
    public class NotesManager : INotesManager
    {
        private readonly INotesRepository notes;
        //private readonly FundooDBContext funDooDBContext;

        public NotesManager(INotesRepository notes)
        {
            this.notes = notes;
        }
        
        public NotesEntity CreateNote(int UserID, NotesModel notesModel)
        {
            return notes.CreateNote(UserID, notesModel);
        }

        public List<NotesEntity> GetNotes(int UserID)
        {
            return notes.GetNotes(UserID);
        }

        public NotesEntity NotesUpdate(int UserID, UpdateNotesModel updateNotesModel)
        {
            return notes.NotesUpdate(UserID, updateNotesModel);
        }

        public bool NotesDelete(int UserID, DeleteNotesModel deleteNotesModel)
        {
            return notes.NotesDelete(UserID, deleteNotesModel);
        }


    }
}
