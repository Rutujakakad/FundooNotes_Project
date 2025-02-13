using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class LabelRepository : ILabelRepository
    {
        private readonly FundooDBContext funDoocontext;

        public LabelRepository(FundooDBContext funDoocontext)
        {
            this.funDoocontext = funDoocontext;
        }

        public LabelEntity AddLabel(int UserID, int NotesId, string LabelName)
        {

            //UserEntity user = context.Users.ToList().Find(user => user.Email == email);
            var note = funDoocontext.Notes.FirstOrDefault(n => n.UserID == UserID && n.NotesId == NotesId);
            if (note != null)
            {
                LabelEntity labelEntity = new LabelEntity();
                labelEntity.LabelName = LabelName;
                labelEntity.NotesId = NotesId;
                labelEntity.UserID = UserID;

                funDoocontext.Labels.Add(labelEntity);
                funDoocontext.SaveChanges();

                return labelEntity;

            }
            return null;
        }

        public List<LabelEntity> GetLabelByUserID(int UserID)
        {
            var listOfLabels = funDoocontext.Labels.Where(label => label.UserID == UserID).ToList();
            return listOfLabels;
        }

        public LabelEntity UpdateLabel(int LabelId, string LabelName, int UserID, int NotesId)//PUT method
        {
            LabelEntity updateLabel = funDoocontext.Labels.FirstOrDefault(a => a.LabelId == LabelId && a.UserID == UserID);
            if (updateLabel == null) return null;

            updateLabel.LabelName = LabelName;
            updateLabel.NotesId = NotesId;

            funDoocontext.Labels.Update(updateLabel);
            funDoocontext.SaveChanges();

            return updateLabel; 

        }

        public bool DeleteLabel(int LabelId)
        {

            var existingLabel = funDoocontext.Labels.FirstOrDefault(n => n.LabelId == LabelId);
            if (existingLabel == null)
            {
                return false;
            }

            funDoocontext.Labels.Remove(existingLabel);
            funDoocontext.SaveChanges();

            return true;
        }








    }

}
