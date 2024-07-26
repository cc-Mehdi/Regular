using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace Regular.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TasksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("ChangeStatus")]
        public async Task<JsonResult> ChangeStatus([FromBody] TaskStatusTemp taskStatusTemp)
        {
            try
            {
                var task = _unitOfWork.TasksRepository.GetFirstOrDefault(u => u.Id == taskStatusTemp.Id);

                if (task == null)
                    return new JsonResult(new { isSuccess = false, message = "وظیفه مورد نظر یافت نشد" });

                // اگر به کاربر به صورت دستی محتوای html را تغییر دهد در این قسمت کنترل میشود که موارد غیر واقعی در دیتابیس درج نشود
                switch (taskStatusTemp.Status)
                {
                    case "انجام شده":
                    case "درحال انجام":
                    case "درحال بررسی":
                        task.TaskStatus = taskStatusTemp.Status;
                        break;
                    default:
                        return new JsonResult(new { isSuccess = false, message = "وضعیت ثبت شده نامعتبر است" });
                        break;
                }

                _unitOfWork.Save();
                return new JsonResult(new { isSuccess = true, message = $"وضعیت وظیفه به {taskStatusTemp.Status} تغییر کرد" });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.Error.WriteLine("Exception occurred: " + ex.Message);
                return new JsonResult(new { isSuccess = false, message = "عملیات با شکست مواجه شد\n" + ex.InnerException.Message.ToString() });
            }
        }


    }

    public class TaskStatusTemp
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }

}
