using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class GetNotesModel
    {
        public int UserID {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
