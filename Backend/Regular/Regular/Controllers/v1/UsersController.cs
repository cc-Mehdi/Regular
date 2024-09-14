using Azure.Core;
using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Regular.Controllers.v1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class UsersController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment = null)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("Edit")]
        public async Task<JsonResult> EditUser([FromForm] UserTemp userTemp, [FromForm] IFormFile? Image)
        {
            try
            {
                if (userTemp == null || string.IsNullOrEmpty(userTemp.Id.ToString()))
                    return new JsonResult(new { isSuccess = false, message = "خطا در ارسال شناسه کاربری" });

                var user = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userTemp.Id);
                if (user == null)
                    return new JsonResult(new { isSuccess = false, message = "کاربر یافت نشد" });

                if (Image != null && Image.Length > 0)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(user.ImageName))
                    {
                        var oldImagePath = _webHostEnvironment.WebRootPath + "/" + Path.Combine("wwwroot", "CustomerResources", "UserProfileImages", user.ImageName);
                        if (File.Exists(oldImagePath))
                            File.Delete(oldImagePath);
                    }

                    // Generate a unique file name
                    var imageExtension = Path.GetExtension(Image.FileName);
                    var imageName = Guid.NewGuid().ToString() + imageExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerResources", "UserProfileImages", imageName);

                    // Check if file is valid image
                    if (imageExtension != ".png" && imageExtension != ".jpg" && imageExtension != ".gif")
                        return new JsonResult(new { isSuccess = false, message = "فایل معتبر نیست" });

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    user.ImageName = "/CustomerResources/UserProfileImages/" + imageName;
                }

                user.FullName = userTemp.FullName;
                user.Rank = userTemp.Rank;
                user.Status = userTemp.Status;

                _unitOfWork.UsersRepository.Update(user);
                _unitOfWork.Save();

                return new JsonResult(new { isSuccess = true, message = "عملیات با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.Error.WriteLine("Exception occurred: " + ex.Message);
                return new JsonResult(new { isSuccess = false, message = "عملیات با شکست مواجه شد" });
            }
        }

        [HttpGet("isUserLogin")]
        public bool isUserLogin(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies["loginToken"] == null)
                return false;
            return true;
        }

        [HttpGet("LoggedInUser")]
        public Users LoggedInUser(HttpContext httpContext)
        {
            string loginToken = Helper.GetCookie(httpContext, "loginToken").ToString();
            Users user = new Users();
            if(isUserLogin(httpContext))
            {
                var log = _unitOfWork.LoginsLogRepository.GetAllByFilterIncludeRelations(u => u.LoginToken == loginToken).FirstOrDefault();
                if(log != null)
                    user = log.User;
            }
            return user;
        }
    }

    public class UserTemp
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Rank { get; set; }
        public string Status { get; set; }
    }
}
