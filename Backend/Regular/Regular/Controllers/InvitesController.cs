using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace Regular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitesController
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvitesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("SetInviteStatus")]
        public async Task<JsonResult> SetInviteStatus([FromBody] InviteTemp inviteTemp)
        {
            try
            {
                var invite = _unitOfWork.Organizations_UsersRepository.GetFirstOrDefault(u => u.Id == inviteTemp.Id);

                if (invite == null)
                    return new JsonResult(new { isSuccess = false, message = "درخواست یافت نشد" });
                else
                {
                    if (inviteTemp.IsAccepted)
                        invite.InviteStatus = "پذیرفته شد";
                    else
                        invite.InviteStatus = "رد شد";

                    invite.AcceptInviteDate = DateTime.Now;
                }

                _unitOfWork.Save();
                return new JsonResult(new { isSuccess = true, message = $"عملیات با موفقیت {invite.InviteStatus}" });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.Error.WriteLine("Exception occurred: " + ex.Message);
                return new JsonResult(new { isSuccess = false, message = "عملیات با شکست مواجه شد\n" + ex.InnerException.Message.ToString() });
            }
        }


    }

    public class InviteTemp
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
    }

}
