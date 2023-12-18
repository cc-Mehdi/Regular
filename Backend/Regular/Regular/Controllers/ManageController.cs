using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Regular.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class ManageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{category}/{parameter}")]
        public IActionResult Get(string category, string parameter)
        {
            var tasksList = _unitOfWork.TasksRepository.GetAll();

            foreach(var task in tasksList)
                task.User = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == task.UserId);

            foreach (var task in tasksList)
                task.Project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == task.UserId);

            if (category == "project")
                tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u=> u.ProjectId == int.Parse(parameter));
            else if(category == "task")
            {
                switch (parameter)
                {
                    case "all":
                        tasksList = _unitOfWork.TasksRepository.GetAll();
                        break;
                    case "done":
                        tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.TaskStatus == "انجام شده");
                        break;
                    case "review":
                        tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.TaskStatus == "برای بررسی");
                        break;
                    case "inProgress":
                        tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.TaskStatus == "درحال انجام");
                        break;
                }
            }
            else if(category == "user")
                tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.UserId == int.Parse(parameter));

            return Json(new { data = tasksList });
        }
    }
}
