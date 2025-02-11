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
        public List<NotesEntity> GetNotesByUserID(int UserID);
        public NotesEntity NotesUpdate(int NotesId, int UserID, NotesModel notesModel);
        public bool NotesDelete(int NotesId);
        public bool UpdateAchieveStatus(int NotesId);
        public bool UpdateTogglePinStatus(int NotesId);

        public bool UpdateTrashStatus(int NotesId);
        public bool UpdateColor(int NotesId, string color);
        public bool UpdateImage(int NotesId, string image);
        public bool UpdateReminder(int NotesId, DateTime reminder);


    }
}
