using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Regular.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class FriendsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FriendsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            int userId = int.Parse(Request.Cookies["UserId"]);
            var friendsList = _unitOfWork.FriendsRepository.GetAllByFilter(u => u.UserId1 == userId);
            return Json(new { data = friendsList });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.FriendsRepository.GetFirstOrDefault(u => u.Id == id);

            _unitOfWork.FriendsRepository.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "حذف با موفقیت انجام شد" });
        }
    }
}
