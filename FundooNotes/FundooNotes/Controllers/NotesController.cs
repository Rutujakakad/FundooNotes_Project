using System;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase // ControllerBase is used for OK and BadRequest
    {
        private readonly INotesManager notesManager;
       // private readonly IDistributedCache distributedCache;
       // private readonly FundooDBContext funDooDBContext;

        public NotesController(INotesManager notesManager)
        {
            this.notesManager = notesManager;
            //this.distributedCache = distributedCache;
            //this.funDooDBContext = funDooDBContext;
        }

        [Authorize]
        [HttpPost]
        [Route("createNote")]
        public ActionResult CreateNote(NotesModel notesModel)// here we are onlu passing the notesModel not the UserID because USerID id fetch from the token
        {
            try
            {
                int UserID = int.Parse(User.FindFirst("UserID").Value);
                NotesEntity notesEntity = notesManager.CreateNote(UserID, notesModel);
                if (notesEntity != null)
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "Notes Created Successfully" });
                else
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Notes not created" });

            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }

        [Authorize]
        [HttpGet]
        [Route("getNotes")]
        public ActionResult NotesGet(GetNotesModel getNotesModel)
        {
            try
            {
                int UserID = int.Parse(User.FindFirst("UserID").Value);
                var notes = notesManager.GetNotes(UserID);
                if (notes != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "Notes retrieved Successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Notes not found" });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateNotes")]
        public ActionResult<NotesEntity> UpdateNote(int UserID, UpdateNotesModel updateNotesModel)
        {
            try
            {

                var updatedNotes = notesManager.NotesUpdate(UserID, updateNotesModel);
                if (updatedNotes != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "Notes Updated Successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Success = true, Message = "Notes Not Updated Successfully" });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("deleteNotes")]
        public ActionResult<NotesEntity> DeleteNotes(int UserID,DeleteNotesModel deleteNotesModel )
        {
            try
            {
                bool isDeleted = notesManager.NotesDelete(UserID, deleteNotesModel);

                if (isDeleted)
                {
                    return Ok(new ResponseModel<NotesEntity> { Success = true, Message = $"Notes with ID {UserID} deleted successfully." });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "Notes Not Delete Successfully" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
            
        }


    }
}
