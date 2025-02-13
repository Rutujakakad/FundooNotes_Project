using System;
using System.Collections.Generic;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace FundooNotes.Controllers
{
    [Route("api/Controller")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelManager labelManager;

        public LabelController(ILabelManager labelManager)
        {
            this.labelManager = labelManager;
        }

        [Authorize]
        [HttpPost("addLabel")]
        // [Route("addLabel")]
        public ActionResult AddLabel( int NotesId, string LabelName)
        {
            try
            {
                //  int NotesId = Convert.ToInt32(NotesId.Value); 

                int UserId = int.Parse(User.FindFirst("UserID").Value);// userId fetch from token
                LabelEntity labelEntity = labelManager.AddLabel(UserId, NotesId, LabelName);
                if (labelEntity != null)
                {
                    return Ok(new ResponseModel<LabelEntity> { Success = true, Message = "Label Added Successfully", Data = labelEntity });
                }
                else
                {
                    return BadRequest(new ResponseModel<LabelEntity> { Success = true, Message = "Label Not Created" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        [Authorize]
        [HttpGet("getLabel")]
        public IActionResult GetLabel()
        {
            try
            {
                int userID = int.Parse(User.FindFirst("UserID").Value);

                List<LabelEntity> labels = labelManager.GetLabelByUserID(userID);

                if (labels.Count > 0)
                {
                    return Ok(new ResponseModel<List<LabelEntity>>{ Success = true, Message = "Labels retrieved Successfully", Data = labels });
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
        [HttpPut("updateLabel")]
        public ActionResult<LabelEntity> LabelUpdate(int labelId,string labelName,int NotesId)
        {
            try
            {
                var userIdClaim = User.FindFirst("UserID");// first it will claim for the UserId
                if (userIdClaim == null)
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Success = false, Message = "User Is Not Authorized" });
                }
               
                int UserID = Convert.ToInt32(userIdClaim.Value);
                LabelEntity labels= labelManager.UpdateLabel(labelId, labelName, UserID, NotesId);
                if (labels != null)
                {
                    
                    return Ok(new ResponseModel<LabelEntity>{ Success = true, Message = "Labels updated successfully", Data = labels });
                }
                else
                {
                    return BadRequest(new ResponseModel<LabelEntity> { Success = false, Message = "Label Not Found Or Not Authorized" });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpDelete("deleteLabel")]
        public ActionResult<LabelEntity> DeleteLabel(int LabelId)
        {
            try
            {
                bool isDeleted = labelManager.DeleteLabel(LabelId);

                if (isDeleted)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = $"Label with ID {LabelId} deleted successfully.", Data = isDeleted });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Label Not Deleted" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }









    }
}
