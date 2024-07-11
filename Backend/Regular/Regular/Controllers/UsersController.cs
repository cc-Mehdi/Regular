using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Regular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpPost]
        public async Task<JsonResult> EdutUser([FromBody]UserTemp userTemp)
        {
            try
            {
                if (string.IsNullOrEmpty(userTemp.Id.ToString()))
                    return new JsonResult(new { err = "خطا در ارسال شناسه کاربری" });

                var user = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userTemp.Id);
                if (user == null)
                    return new JsonResult(new { err = "کاربر یافت نشد" });

                //if (image != null && image.Length > 0)
                //{
                //    var imageName = Path.GetFileName(image.FileName);
                //    var filePath = Path.Combine(_environment.WebRootPath, "images", imageName);

                //    using (var stream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await image.CopyToAsync(stream);
                //    }

                //    user.ImageName = imageName;
                //}

                user.FullName = userTemp.FullName;
                user.Rank = userTemp.Rank;
                user.Status = userTemp.Status;

                _unitOfWork.UsersRepository.Update(user);
                _unitOfWork.Save();

                return new JsonResult(new { isSuccess = true, message = "عملیات با موفقیت انجام شد" });
            }
            catch
            {
                return new JsonResult(new { isSuccess = false, message = "عملیات با شکست مواجه شد" });
            }
        }
    }

    public class UserTemp
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Rank { get; set; }
        public string Status { get; set; }
    }
}
