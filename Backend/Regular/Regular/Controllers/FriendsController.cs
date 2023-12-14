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
            var friendsList = _unitOfWork.FriendsRepository.GetAll();
            return Json(new { data = friendsList });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.FriendsRepository.GetFirstOrDefault(u => u.Id == id);

            _unitOfWork.FriendsRepository.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
