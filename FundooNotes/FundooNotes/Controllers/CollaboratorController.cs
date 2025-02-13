using System;
using System.Collections.Generic;
using System.Security.Claims;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;

namespace FundooNotes.Controllers
{
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorManager collaboratorManager;

        public CollaboratorController(ICollaboratorManager collaboratorManager)
        {
            this.collaboratorManager = collaboratorManager;
        }

        [Authorize]
        [HttpPost("addCollaborator")]
        public ActionResult AddCollaborator(string Email, int NotesId, int UserID)
        {
            try
            {
                
                if ( string.IsNullOrEmpty(Email))//1st it will check for mail is exist or not
                {
                    return BadRequest(new ResponseModel<CollaboratorEntity> { Success = false, Message = "Email is Required", Data = null });
                }
                var userClaim = User.FindFirst("UserID");// then it will check user is authorized or not
                if (userClaim == null)
                {
                    return BadRequest(new ResponseModel<CollaboratorEntity> { Success = false, Message = "User is not authorized." });
                   
                }

                int authenticateduserID = Convert.ToInt32(userClaim.Value);
                UserID = authenticateduserID; //it will assigned authenticated UserID

                var addCollaborator = collaboratorManager.AddCollaborator(Email, NotesId, authenticateduserID);
                if(addCollaborator != null)
                {
                    return Ok(new ResponseModel<CollaboratorEntity> { Success = true, Message = "Collaborator added successfully", Data = addCollaborator });
                }
                else
                {
                    return BadRequest(new ResponseModel<CollaboratorEntity> { Success = false, Message = "Failed to add collaborator", Data = addCollaborator });
                }
                
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("getCollaborator")]
        public ActionResult GetCollaborator()
        {
            try
            {
                int userID = int.Parse(User.FindFirst("UserID").Value);
                List<CollaboratorEntity> collaborators = collaboratorManager.GetCollaboratorsByUserId(userID);
                if (collaborators.Count > 0)
                {
                    return Ok(new ResponseModel<List<CollaboratorEntity>> { Success = true, Message = "Collaborators retrieved successfully", Data = collaborators });
                }
                else
                {
                    return BadRequest(new ResponseModel<CollaboratorEntity> { Success = false, Message = "Collaborator not found" });
                }
            }       
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        [Authorize]
        [HttpDelete("deleteCollaborator")]
        public ActionResult DeleteCollaborator(int CollaboratorId)
        {
            try
            {
                bool isDeleted = collaboratorManager.DeleteCollaborator(CollaboratorId);

                if (isDeleted)
                {
                    return Ok(new ResponseModel<bool> { Success = true, Message = $"Collaborator with ID {CollaboratorId} deleted successfully.", Data = isDeleted });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Success = false, Message = "Collaborator Not Deleted" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
