using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage
{
    public class ManageProjectsModel : PageModel
    {
        int userId = 0;
        public IEnumerable<Projects> Projects { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public ManageProjectsModel(IUnitOfWork unitOfWork)
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
                Projects = _unitOfWork.ProjectsRepository.GetAll().Where(u => u.UserId == userId).ToList();
            }
        }
    }
}
