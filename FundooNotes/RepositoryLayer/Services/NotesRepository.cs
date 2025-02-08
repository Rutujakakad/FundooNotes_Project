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

        public NotesEntity CreateNote(int UserID, NotesModel notesModel)// add method
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

        public List<NotesEntity> GetNotes(int UserID)
        {
            var listOfNotes = funDooDBContext.Notes.Where(x => x.UserID == UserID).ToList();
            return listOfNotes;
        }

        public NotesEntity NotesUpdate(int UserID, UpdateNotesModel updateNotesModel)
        {
            var updateNotes = funDooDBContext.Notes.FirstOrDefault(a =>  a.UserID == UserID);
            if (updateNotes == null)
            {
                updateNotes.UserID = updateNotesModel.UserID;
                updateNotes.Title = updateNotesModel.Title;
                updateNotes.Description = updateNotesModel.Description;
                updateNotes.UpdateAt = DateTime.Now;

                funDooDBContext.Notes.Update(updateNotes);
                funDooDBContext.SaveChanges();

                return updateNotes;
            }
            else
            {
                throw new Exception("UserID Does Not Exist");
            }
        }

        public bool NotesDelete(int UserID, DeleteNotesModel deleteNotesModel)
        {
            
            var existingNotes = funDooDBContext.Notes.FirstOrDefault(n => n.UserID == UserID);
            if (existingNotes == null)
            {
                return false; 
            }

            funDooDBContext.Notes.Remove(existingNotes);
            funDooDBContext.SaveChanges();

            return true; 
        }

    }





}

   

