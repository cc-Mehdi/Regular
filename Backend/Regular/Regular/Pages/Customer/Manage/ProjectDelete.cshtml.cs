using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage
{
    [BindProperties]
    public class ProjectDeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public Projects Project { get; set; }

        public ProjectDeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(int id)
        {
            Project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == id);
        }

        public async Task<IActionResult> OnPost()
        {
            //Get selected coffee for remove
            var projectFromDb = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == Project.Id);

            if (projectFromDb != null)
            {
                _unitOfWork.ProjectsRepository.Remove(projectFromDb);
                //Save db
                _unitOfWork.Save();

                return RedirectToPage("/Customer/Manage/ProjectsList");
            }
            return Page();
        }
    }
}
