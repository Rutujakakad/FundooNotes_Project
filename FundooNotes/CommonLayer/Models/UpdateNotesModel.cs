using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class UpdateNotesModel
    {
        public int UserID { get; set; }
        public string Title {  get; set; }
        public string Description { get; set; }
        public string UpdatedAt {  get; set; }
        
    }
}
