using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollaboratorId {  get; set; }
        public string Email {  get; set; }


        [ForeignKey("CollaboratorNote")]
        public int NotesId {  get; set; }

        [ForeignKey("CollaboratorUser")]
        public int UserID {  get; set; }

        [JsonIgnore]
        public virtual NotesEntity CollaboratorNote { get; set; }

        [JsonIgnore]
        public virtual UserEntity CollaboratorUser { get; set; }
    }
}
