using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage.Tasks
{
    [BindProperties]
    public class TaskDeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DataLayer.Models.Tasks Tasks { get; set; }

        public TaskDeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(int id)
        {
            Tasks = _unitOfWork.TasksRepository.GetFirstOrDefault(u => u.Id == id);
        }

        public async Task<IActionResult> OnPost()
        {
            //Get selected coffee for remove
            var taskFromDb = _unitOfWork.TasksRepository.GetFirstOrDefault(u => u.Id == Tasks.Id);

            if (taskFromDb != null)
            {
                _unitOfWork.TasksRepository.Remove(taskFromDb);
                //Save db
                _unitOfWork.Save();

                return RedirectToPage("/Customer/Manage/Tasks/TasksList");
            }
            return Page();
        }
    }
}
