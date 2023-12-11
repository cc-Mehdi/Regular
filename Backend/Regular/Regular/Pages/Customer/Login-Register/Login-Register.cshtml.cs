using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Web;

namespace Regular.Pages.Customer.Login_Register
{
    [BindProperties]
    public class Login_RegisterModel : PageModel
    {
        public Users User { get; set; }
        private readonly IUnitOfWork _unitOfWork;

        public Login_RegisterModel(IUnitOfWork unitOfWork)
        {
            User = new();
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(Users user)
        {
            if (user.Id == 1) //Login
            {
                var checkUser = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                if (checkUser != null)
                {
                    var cookie = Request.Cookies["UserId"];

                    //set cookie
                    var cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTime.Now.AddDays(30);
                    cookieOptions.Path = "/";
                    Response.Cookies.Append("UserId", checkUser.Id.ToString(), cookieOptions);

                    return RedirectToPage("/Privacy");
                }
                else
                {
                    //Show error message (user not exist)
                }
            }
            else //Register
            {
                if(user.Email != "" && user.Password != "" && user.Username != "")
                {
                    var userEmails = _unitOfWork.UsersRepository.GetAll().Where(u => u.Email == user.Email).ToList();
                    if(userEmails.Count == 0)
                    {
                        user.FirstName = user.LastName = "";
                        _unitOfWork.UsersRepository.Add(user);
                        _unitOfWork.Save();

                        var checkUser = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Email == user.Email);

                        //set cookie
                        var cookieOptions = new CookieOptions();
                        cookieOptions.Expires = DateTime.Now.AddDays(1);
                        cookieOptions.Path = "/";
                        Response.Cookies.Append("UserId", checkUser.Id.ToString(), cookieOptions);

                        return RedirectToPage("/Customer/Manage/Manage.cshtml");
                    }
                    else
                    {
                        //show error message (user exist with the same email address
                    }
                }
            }

            return RedirectToPage(user);
        }

    }
}
