﻿using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utilities;

namespace Regular.Pages.Customer
{
    public class Login_RegisterModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public Users User { get; set; }

        public Login_RegisterModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            User = new();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(Users user)
        {
            if(user.Email != null && user.Password != null)
            {
                // Generate login log
                string loginToken = Guid.NewGuid().ToString();

                if (user.Id == 1) //login
                {
                    var currentUser = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Email == user.Email);
                    if (currentUser != null)
                    {
                        if (user.Email == currentUser.Email && user.Password == currentUser.Password)
                        {

                            _unitOfWork.LoginsLogRepository.Add(new LoginsLog()
                            {
                                UserId = currentUser.Id,
                                User = currentUser,
                                LoginToken = loginToken,
                                IsSignOut = false,
                                LogTime = DateTime.Now
                            });
                            _unitOfWork.Save();

                            //Set cookie
                            Helper.SetCookie(HttpContext, "loginToken", loginToken);

                            TempData["success"] = $"{currentUser.FullName} خوش آمدید!";
                            return Redirect("/Customer/UserPanel");
                        }
                        else
                            TempData["error"] = "ایمیل یا کلمه عبور اشتباه است!";
                    }
                    else
                        TempData["error"] = "کاربری با ایمیل مورد نظر یافت نشد!";
                }
                else //register
                {
                    if(user.FullName != null && user.Email != null && user.Password != null)
                    {
                        var currentUser = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Email == user.Email);
                        if(currentUser == null)
                        {
                            //save to database
                            user.ImageName = "/CustomerResources/DefaultSources/UserImage.jpg";
                            user.Username = user.FullName.Replace(" ", "") + "_" + Utilities.Helper.GenerateUniqueNumber();
                            user.Status = "وضعیت : آزاد";
                            user.Rank = "سطح : متوسط";
                            user.PublicId = Guid.NewGuid().ToString();
                            _unitOfWork.UsersRepository.Add(user);
                            _unitOfWork.Save();

                            //find user
                            currentUser = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Email == user.Email);

                            LoginsLog newLog = new LoginsLog()
                            {
                                UserId = currentUser.Id,
                                User = currentUser,
                                LoginToken = loginToken,
                                IsSignOut = false,
                                LogTime = DateTime.Now,
                            };
                            _unitOfWork.LoginsLogRepository.Add(newLog);

                            _unitOfWork.Save();

                            //Set cookie
                            Helper.SetCookie(HttpContext, "loginToken", loginToken);

                            TempData["success"] = "ثبت نام با موفقیت انجام شد";
                            return Redirect("/Customer/UserPanel");
                        }
                        else
                            TempData["error"] = "شما قبلا ثبت نام کرده اید!";
                    }
                    else
                        TempData["error"] = "فیلدها را با دقت پر کنید!";
                }

            }
            else
                TempData["error"] = "فیلدها را با دقت پر کنید!";



            return Page();
        }
    }
}
