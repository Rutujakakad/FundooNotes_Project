﻿using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface ILabelManager
    {
        public LabelEntity AddLabel(int UserID, int NotesId, string LabelName);
        public List<LabelEntity> GetLabelByUserID(int UserID);
        public LabelEntity UpdateLabel(int LabelId, string LabelName, int UserID, int NotesId);
        public bool DeleteLabel(int LabelId);
    }
}
