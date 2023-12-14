using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage.Tasks
{
    [BindProperties]
    public class TaskUpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DataLayer.Models.Tasks Tasks { get; set; }

        public TaskUpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Tasks = new();
        }

        public void OnGet(int? id = 0)
        {
            if (id != 0)
                Tasks = _unitOfWork.TasksRepository.GetFirstOrDefault(u => u.Id == id);
        }

        public async Task<IActionResult> OnPost()
        {
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
