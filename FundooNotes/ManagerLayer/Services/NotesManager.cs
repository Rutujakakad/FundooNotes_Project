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

        public List<NotesEntity> GetNotesByUserID(int UserID)
        {
            return notes.GetNotesByUserID(UserID);
        }

        public NotesEntity NotesUpdate(int NotesId, int UserID, NotesModel notesModel)
        {
            return notes.NotesUpdate(NotesId, UserID, notesModel);
        }

        public bool NotesDelete(int NotesId)
        {
            return notes.NotesDelete(NotesId);
        }
        public bool UpdateAchieveStatus(int NotesId)
        {
            return notes.UpdateTogglePinStatus(NotesId);
        }
        public bool UpdateTogglePinStatus(int NotesId)
        {
            return notes.UpdateTogglePinStatus( NotesId);
        }

        public bool UpdateTrashStatus(int NotesId)
        {
            return UpdateTrashStatus(NotesId);
        }

        public bool UpdateColor(int NotesId, string color)
        {
            return notes.UpdateColor(NotesId, color);
        }
        public bool UpdateImage(int NotesId, string image)
        {
            return notes.UpdateImage(NotesId, image);
        }
        public bool UpdateReminder(int NotesId, DateTime reminder)
        {
            return notes.UpdateReminder(NotesId, reminder);
        }

    }
}
