using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace Regular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost]
        public async Task<JsonResult> EditUser([FromForm] UserTemp userTemp, [FromForm] IFormFile? Image) // Use nullable type
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
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    // Generate a unique file name
                    var imageExtension = Path.GetExtension(Image.FileName);
                    var imageName = Guid.NewGuid().ToString() + imageExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerResources", "UserProfileImages", imageName);

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


    }

    public class UserTemp
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Rank { get; set; }
        public string Status { get; set; }
    }

}
