using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class LabelManager :ILabelManager
    {
        private readonly ILabelRepository label;
        public LabelManager(ILabelRepository label)
        {
            this.label = label;
        }

        public LabelEntity AddLabel(int UserID, int NotesId, string LabelName)
        {
            return label.AddLabel(UserID, NotesId, LabelName);
        }

        public List<LabelEntity> GetLabelByUserID(int UserID)
        {
            return label.GetLabelByUserID(UserID);
        }

        public LabelEntity UpdateLabel(int LabelId, string LabelName, int UserID, int NotesId)
        {
            return label.UpdateLabel(LabelId, LabelName, UserID, NotesId);
        }

        public bool DeleteLabel(int LabelId)
        {
            return label.DeleteLabel(LabelId);
        }
    }
}
















