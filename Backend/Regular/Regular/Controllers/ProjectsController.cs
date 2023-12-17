using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Regular.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class ProjectsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            int userId = int.Parse(Request.Cookies["UserId"]);
            var projectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u=> u.UserId == userId);
            return Json(new { data = projectsList });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == id);

            _unitOfWork.ProjectsRepository.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "حذف با موفقیت انجام شد" });
        }
    }
}
