using DataLayer.Models;
using DataLayer.Repository.IRepository;
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
            isUserLogin();
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
