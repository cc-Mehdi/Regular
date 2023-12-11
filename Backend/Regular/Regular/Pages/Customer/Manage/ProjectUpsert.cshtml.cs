using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage
{
    [BindProperties]
    public class ProjectUpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public Projects Project { get; set; }

        public ProjectUpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Project = new();
        }

        public void OnGet(int? id = 0)
        {
            if (id != 0)
                Project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == id);
        }

        public async Task<IActionResult> OnPost(Projects project)
        {
            //Create
            if(project.Id == 0)
            {
                _unitOfWork.ProjectsRepository.Add(project);
                _unitOfWork.Save();
            }
            else //Edit
            {
                _unitOfWork.ProjectsRepository.Update(project);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Manage/ProjectsList");
        }
    }
}
