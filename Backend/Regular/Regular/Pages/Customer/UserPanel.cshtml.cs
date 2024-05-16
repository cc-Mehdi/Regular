using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer
{
    public class UserPanelModel : PageModel
    {
        private Users logedInUser;
        private readonly IUnitOfWork _unitOfWork;

        public UserPanelModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            isUserLogin();
        }

        private void isUserLogin()
        {
            if (Request.Cookies["loginToken"] == null)
                Response.Redirect("/Customer/Login-Register");
            else
            {
                string loginToken = Request.Cookies["loginToken"];
                logedInUser = _unitOfWork.LoginsLogRepository.GetFirstOrDefault(u => u.LoginToken == loginToken).User;
            }
                
        }   
    }
}
