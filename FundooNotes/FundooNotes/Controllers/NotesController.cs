using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLayer.Models;
using GreenPipes.Caching;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
//using RepositoryLayer.Migrations;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase // ControllerBase is used for OK and BadRequest
    {
        private readonly INotesManager notesManager;
        private readonly ILogger<NotesController> logger;
        private readonly IDistributedCache distributedCache;
        private readonly FundooDBContext funDooDBContext;
        private readonly IDistributedCache _cache;
        //private readonly object distributedCache;

        public NotesController(INotesManager notesManager, ILogger<NotesController> logger, IDistributedCache distributedCache, IDistributedCache _cache, FundooDBContext funDooDBContext )
        {
            this.notesManager = notesManager;
            this.logger = logger;
            this.distributedCache = distributedCache;
            this._cache = _cache;
            this.funDooDBContext = funDooDBContext;
        }

        [Authorize]
        [HttpPost] 
        [Route("createNote")]
        public IActionResult CreateNote(NotesModel notesModel)// here we are only passing the notesModel not the UserID because USerID id fetch from the token
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("getNotes")]
        public IActionResult NotesGet()
        {
            try
            {
                int userID = int.Parse(User.FindFirst("UserID").Value);

                List<NotesEntity> notes = notesManager.GetNotesByUserID(userID);

                if (notes.Count > 0)
                {
                    return Ok(new { Success = true, Message = "Notes retrieved Successfully", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Notes not found" });
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateNotes")]
        public ActionResult<NotesEntity> UpdateNote(int NotesId, NotesModel updatedNotes)
        {
            try
            {

                var userId = User.FindFirst("UserID");// first it will claim for the UserId
                if (userId == null)
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "User Is Not Authorized" });
                }
                int userID = Convert.ToInt32(userId.Value);
                NotesEntity notes = notesManager.NotesUpdate(NotesId, userID, updatedNotes);
                if (notes != null)
                {
                    //throw new Exception("Error Occured");
                    logger.LogInformation("NoteUpdated");
                    return Ok(new { Success = true, Message = "Notes updated successfully", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "Note Not Found Or Not Authorized" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Error Occured", Data = ex.Message }); //throw ex;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("deleteNotes")]
        public ActionResult<NotesEntity> DeleteNotes(int NotesId)
        {
            try
            {
                bool isDeleted = notesManager.NotesDelete(NotesId);

                if (isDeleted)
                {
                    return Ok(new ResponseModel<bool>{ Success = true, Message = $"Notes with ID {NotesId} deleted successfully.", Data = isDeleted });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Notes Not Delete Successfully" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateAchive")]
        public ActionResult ArchiveNote(int NotesId)
        {
            try
            {
                var result = notesManager.UpdateAchieveStatus(NotesId);
                if (result)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Note Archive Status updated Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Notes Not Found" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize]
        [HttpPut]
        [Route("updateTogglePin")]
        public ActionResult TogglePinNote(int NotesId)
        {
            try
            {
                var result = notesManager.UpdateTogglePinStatus(NotesId);
                if (result)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Note pin Status updated Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "Notes Not Found" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("moveToTrash")]
        public ActionResult TrashNote(int NotesId)
        {
            try
            {
                var result = notesManager.UpdateTrashStatus(NotesId);
                if (!result)
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note not found", Data = result });
                }
                else
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Note trash status updated successfully", Data = result });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("updateColor")]
        public ActionResult UpdateColor(int NotesId, string color)
        {
            try
            {
                var result = notesManager.UpdateColor(NotesId, color);
                if (result)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Color Is Updated Successfully", Data = result});
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note Not Found", Data = result});
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        [Authorize]
        [HttpPut]
        [Route("updateImage")]
        public ActionResult UpdateImage(int NotesId, string image)
        {
            try
            {
                var result = notesManager.UpdateImage(NotesId, image);
                if (result)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Image Is Updated Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note Not Found", Data = result });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Authorize]
        [HttpPut]
        [Route("updateReminder")]
        public ActionResult UpdateReminder(int NotesId, DateTime reminder)
        {
            try
            {
                var result = notesManager.UpdateReminder(NotesId, reminder);
                if (result)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = "Reminder Is Updated Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Note Not Found", Data = result });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string SerializedNotesList;
            var NotesList = new List<NotesEntity>();
            //var RedisNotesList = await DistributedCacheEntryExtensions.GetAsync(cacheKey);
            var RedisNotesList = await _cache.GetAsync(cacheKey);
            if (RedisNotesList != null)
            {
                SerializedNotesList = Encoding.UTF8.GetString(RedisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(SerializedNotesList);
            }
            else
            {
                NotesList = funDooDBContext.Notes.ToList();
                SerializedNotesList = JsonConvert.SerializeObject(NotesList);
                RedisNotesList = Encoding.UTF8.GetBytes(SerializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                await distributedCache.SetAsync(cacheKey, RedisNotesList, options);
            }
            return Ok(NotesList);
        }





    }
    //----------------------------------------------------Review Session++++++++++++++++++++


    //[HttpGet]
    //[Route("GetNotes")]
    //public IActionResult GetNotesByTitleAndDescription(string fetchTitle, string fetchDescription)
    //{
    //    try
    //    {
    //        var notes = funDooDBContext.Notes.Where(n => n.Title.Contains(fetchTitle) && n.Description.Contains(fetchDescription)).ToList();
    //        if (notes.Any())
    //        {
    //            return Ok(new ResponseModel<NotesEntity> { Success = true, Message = "No notes found" });
    //        }
    //        else
    //        {
    //            return BadRequest(new { success = false, message = "Notes retrieved successfully", data = notes });
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}         


    //[HttpGet]
    //[Route("CountOfNotes")]
    //public IActionResult ReturnCountOfNotes(int UserID)
    //{
    //    try
    //    {
    //        var returnCount = funDooDBContext.Notes.Count(n => n.UserID == UserID);
    //        if (returnCount > 0)
    //        {
    //            return Ok(new ResponseModel<int> { Success = true, Message = $"Total no of count User Has: {returnCount}" });
    //        }
    //        else
    //        {
    //            return BadRequest(new ResponseModel<int> { Success = false, Message = "Total no of count not found" });
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }


    //}






}




    

