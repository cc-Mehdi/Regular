using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Regular.Pages.Manage
{
    public class ManageModel : PageModel
    {
        private int userId = 0;
        private readonly IUnitOfWork _unitOfWork;
        public Users User { get; set; }
        public Tasks Task { get; set; }
        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<Friends> Friends { get; set; }
        public IEnumerable<Projects> Projects { get; set; }
        public IEnumerable<Tasks> Tasks { get; set; }


        public int allTasksCount = 0;
        public int inProgressTasksCount = 0;
        public int doneTasksCount = 0;
        public int inPreviewTasksCount = 0;
        public int userTasksCount = 0;

        public ManageModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            var cookie = Request.Cookies["UserId"];
            if (cookie == null)
            {
                Response.Redirect("/Customer/Login-Register/Login-Register");
            }
            else
            {
                userId = int.Parse(cookie);
                User = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userId);
                Friends = _unitOfWork.FriendsRepository.GetAll().Where(u => u.UserId1 == userId).ToList();
                Projects = _unitOfWork.ProjectsRepository.GetAll().Where(u => u.UserId == userId).ToList();
                Tasks = _unitOfWork.TasksRepository.GetAll().Where(u => u.UserId == userId).ToList();

                allTasksCount = Tasks.Count();
                inProgressTasksCount = Tasks.Where(u=> u.TaskStatus == "درحال انجام").ToList().Count();
                doneTasksCount = Tasks.Where(u => u.TaskStatus == "انجام شده").ToList().Count();
                inPreviewTasksCount = Tasks.Where(u => u.TaskStatus == "برای بررسی").ToList().Count();
                userTasksCount = Tasks.Where(u => u.UserId == userId).ToList().Count();

                foreach (var item in Friends)
                    Users.Append(_unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == item.UserId2));
            }
        }
    }
}
