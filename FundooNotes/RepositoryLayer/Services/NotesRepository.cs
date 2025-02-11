using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class NotesRepository : INotesRepository
    {
        private readonly FundooDBContext funDooDBContext;

        public NotesRepository(FundooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public NotesEntity CreateNote(int UserID, NotesModel notesModel)// add/POST method:create new data
        {
            NotesEntity notesEntity = new NotesEntity();
            notesEntity.Title = notesModel.Title;
            notesEntity.Description = notesModel.Description;
            notesEntity.CreatedAt = DateTime.Now;
            notesEntity.UpdateAt = DateTime.Now;
            notesEntity.UserID = UserID;

            funDooDBContext.Notes.Add(notesEntity);
            funDooDBContext.SaveChanges();

            return notesEntity;
        }

        public List<NotesEntity> GetNotesByUserID(int UserID)
        {
            var listOfNotes = funDooDBContext.Notes.Where(note => note.UserID == UserID).ToList();
            return listOfNotes;
        }

        public NotesEntity NotesUpdate(int NotesId, int UserID, NotesModel notesModel)//PUT method
        {
            NotesEntity updateNotes = funDooDBContext.Notes.FirstOrDefault(a =>  a.NotesId == NotesId && a.UserID == UserID);
            if (updateNotes == null) return null;
            
                updateNotes.Title = notesModel.Title;
                updateNotes.Description = notesModel.Description;
                updateNotes.UpdateAt = DateTime.Now;

                funDooDBContext.Notes.Update(updateNotes);
                funDooDBContext.SaveChanges();

                return updateNotes;
            
           
        }

        public bool NotesDelete(int NotesId)
        {
            
            var existingNotes = funDooDBContext.Notes.FirstOrDefault(n => n.NotesId == NotesId);
            if (existingNotes == null)
            {
                return false; 
            }

            funDooDBContext.Notes.Remove(existingNotes);
            funDooDBContext.SaveChanges();

            return true; 
        }


        public bool UpdateAchieveStatus(int NotesId)
        {
            var note = funDooDBContext.Notes.FirstOrDefault(a => a.NotesId == NotesId);
            if (note == null) return false;

            note.IsArchive = !note.IsArchive;
            note.UpdateAt = DateTime.Now;

            funDooDBContext.SaveChanges();
            return true;
        }

        public bool UpdateTogglePinStatus(int NotesId)
        {
            var note = funDooDBContext.Notes.FirstOrDefault(f => f.NotesId == NotesId);
            if (note == null) return false;

            note.IsPin = !note.IsPin;
            note.UpdateAt = DateTime.Now;

            funDooDBContext.SaveChanges();
            return true;
        }

        public bool UpdateTrashStatus(int NotesId)
        {
            var note = funDooDBContext.Notes.FirstOrDefault(f => f.NotesId == NotesId);
            if (note == null) return false;

            note.IsTrash = !note.IsTrash;
            note.UpdateAt = DateTime.Now;

            funDooDBContext.SaveChanges();
            return true;
        }

        public bool UpdateColor(int NotesId, string color)
        {
            var notes = funDooDBContext.Notes.FirstOrDefault(n => n.NotesId == NotesId);
            if (notes == null)
            {
                return false;
            }
            notes.Color = color;
            notes.UpdateAt = DateTime.Now;
            funDooDBContext.SaveChanges();
            return true;
        }

        public bool UpdateImage(int NotesId, string image)
        {
            var notes = funDooDBContext.Notes.FirstOrDefault(x => x.NotesId == NotesId);
            if (notes == null)
            {
                return false;
            }
            notes.Image = image;
            notes.UpdateAt = DateTime.Now;
            funDooDBContext.SaveChanges();
            return true;
        }

        public bool UpdateReminder(int NotesId, DateTime reminder)
        {
            var notes = funDooDBContext.Notes.FirstOrDefault(y => y.NotesId == NotesId);
            if (notes == null)
            {
                return false;
            }
            notes.Reminder = reminder;
            notes.UpdateAt = DateTime.Now;
            funDooDBContext.SaveChanges();
            return true;
        }


    }





}

   

