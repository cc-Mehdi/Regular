using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Regular.Pages.Manage
{
    public class ManageModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public Users User { get; set; }

        public ManageModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            var cookie = Request.Cookies["UserId"];
            if(cookie == null)
            {
                Response.Redirect("/Customer/Login-Register/Login-Register");
            }
            else
            {
                User = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == int.Parse(cookie));
            }
        }
    }
}
