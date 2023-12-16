using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

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
            isUserLogin();
            if(userId != 0)
            Projects = _unitOfWork.ProjectsRepository.GetAll().Where(u => u.UserId == userId).ToList();
        }

        private void isUserLogin()
        {
            if (Request.Cookies["UserId"] == null)
                Response.Redirect("/Customer/Login-Register/Login-Register");
            else
                userId = int.Parse(Request.Cookies["UserId"]);
        }
    }
}
