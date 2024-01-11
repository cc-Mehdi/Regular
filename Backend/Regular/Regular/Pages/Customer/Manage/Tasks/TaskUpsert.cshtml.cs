using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage.Tasks
{
    [BindProperties]
    public class TaskUpsertModel : PageModel
    {
        int userId = 0;
        private readonly IUnitOfWork _unitOfWork;
        public DataLayer.Models.Tasks Tasks { get; set; }
        public IEnumerable<Projects> Projects { get; set; }
        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<DataLayer.Models.Friends> Friends { get; set; }

        public TaskUpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Tasks = new();
        }

        public void OnGet(int? id = 0)
        {
            userId = int.Parse(Request.Cookies["UserId"]);
            Projects = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.UserId == userId);
            Friends = _unitOfWork.FriendsRepository.GetAllByFilter(u => u.UserId1 == userId);
            foreach (var friend in Friends)
                Users = _unitOfWork.UsersRepository.GetAllByFilter(u => u.Id == friend.UserId2);
            if (this.Users == null)
            {
                this.Users = _unitOfWork.UsersRepository.GetAll();
                this.Users = _unitOfWork.UsersRepository.GetAllByFilter(u => u.Id == userId);
            }
            else
            Users.Append(_unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userId));


            isUserLogin();
            if (userId != 0)
                if (id != 0)
                    Tasks = _unitOfWork.TasksRepository.GetFirstOrDefault(u => u.Id == id);
        }

        private void isUserLogin()
        {
            if (Request.Cookies["UserId"] == null)
                Response.Redirect("/Customer/Login-Register/Login-Register");
            else
                userId = int.Parse(Request.Cookies["UserId"]);
        }

        public async Task<IActionResult> OnPost()
        {
            var cookie = Request.Cookies["UserId"];
            int userId = int.Parse(cookie);

            var friends = _unitOfWork.FriendsRepository.GetAllByFilter(u => u.UserId1 == Tasks.UserId || u.UserId1 == userId);
            var user = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == Tasks.UserId);
            var project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == Tasks.ProjectId);

            bool isUserFriend = false;
            foreach (var friend in friends)
                if (Tasks.UserId == friend.UserId2 || Tasks.UserId == userId)
                    isUserFriend = true;
            if (!isUserFriend && Tasks.UserId != userId)
            {
                TempData["error"] = "مسئول مورد نظر یافت نشد";
                return Page();
            }


            if (project == null)
            {
                TempData["error"] = "پروژه مورد نظر یافت نشد";
                return Page();
            }


            Tasks.Project = project;
            Tasks.User = user;
            Tasks.ReporterId = userId;
            Tasks.Reporter = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userId);

            //Create
            if (Tasks.Id == 0)
            {
                project.TasksCount++;
                user.TasksCount++;
                TempData["success"] = "وظیفه با موفقیت اضافه شد";
                _unitOfWork.TasksRepository.Add(Tasks);
                _unitOfWork.Save();
            }
            else //Edit
            {
                var oldTask = _unitOfWork.TasksRepository.GetFirstOrDefault(u => u.Id == Tasks.Id);
                if (oldTask.ProjectId != Tasks.ProjectId)
                {
                    project.TasksCount++; //increase new project taskscount
                    var oldProject = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == oldTask.ProjectId);
                    oldProject.TasksCount--; //reduce old project taskcount
                }


                if (oldTask.UserId != Tasks.UserId)
                {
                    user.TasksCount++; //increase new user taskscount
                    var oldUser = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == oldTask.UserId);
                    oldUser.TasksCount--; //reduce old user taskcount
                }

                TempData["success"] = "وظیفه با موفقیت ویرایش شد";
                _unitOfWork.TasksRepository.Update(Tasks);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Manage/Tasks/TasksList");
        }
    }
}
