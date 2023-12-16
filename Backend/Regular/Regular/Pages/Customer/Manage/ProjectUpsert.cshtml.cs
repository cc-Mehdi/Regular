using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage
{
    [BindProperties]
    public class ProjectUpsertModel : PageModel
    {
        int userId = 0;
        private readonly IUnitOfWork _unitOfWork;
        public Projects Project { get; set; }

        public ProjectUpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Project = new();
        }

        public void OnGet(int? id = 0)
        {
            isUserLogin();
            if (userId != 0)
                if (id != 0)
                    Project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == id);
        }

        private void isUserLogin()
        {
            if (Request.Cookies["UserId"] == null)
                Response.Redirect("/Customer/Login-Register/Login-Register");
            else
                userId = int.Parse(Request.Cookies["UserId"]);
        }

        public async Task<IActionResult> OnPost(Projects project)
        {
            //Create
            if (project.Id == 0)
            {
                TempData["success"] = "پروژه با موفقیت اضافه شد";
                _unitOfWork.ProjectsRepository.Add(project);
                _unitOfWork.Save();
            }
            else //Edit
            {
                TempData["success"] = "پروژه با موفقیت ویرایش شد";
                _unitOfWork.ProjectsRepository.Update(project);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Manage/ProjectsList");
        }
    }
}
