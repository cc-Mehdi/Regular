using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Regular.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TasksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            int userId = int.Parse(Request.Cookies["UserId"]);
            var tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.UserId == userId);
            return Json(new { data = tasksList });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.TasksRepository.GetFirstOrDefault(u => u.Id == id);

            var project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == objFromDb.ProjectId);
            var user = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == objFromDb.UserId);

            project.TasksCount += 1;
            user.TasksCount += 1;

            _unitOfWork.TasksRepository.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "حذف با موفقیت انجام شد" });
        }
    }
}
