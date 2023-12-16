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

        public TaskUpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Tasks = new();
        }

        public void OnGet(int? id = 0)
        {
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

            var friends = _unitOfWork.FriendsRepository.GetAllByFilter(u => u.UserId1 == userId);

            bool isUserFriend = false;
            foreach (var friend in friends)
                if (Tasks.UserId == friend.UserId2 || Tasks.UserId == userId)
                    isUserFriend = true;
            if(!isUserFriend)
            {
                TempData["error"] = "مسئول مورد نظر یافت نشد";
                return Page();
            }
            
            //Create
            if (Tasks.Id == 0)
            {
                TempData["success"] = "وظیفه با موفقیت اضافه شد";
                _unitOfWork.TasksRepository.Add(Tasks);
                _unitOfWork.Save();
            }
            else //Edit
            {
                TempData["success"] = "وظیفه با موفقیت ویرایش شد";
                _unitOfWork.TasksRepository.Update(Tasks);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Manage/Tasks/TasksList");
        }
    }
}
