using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Regular.Pages.Manage
{
    public class ManageModel : PageModel
    {
        private int userId = 0;
        private readonly IUnitOfWork _unitOfWork;
        public Users User { get; set; }
        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<Friends> Friends { get; set; }
        public IEnumerable<Projects> Projects { get; set; }
        public IEnumerable<Tasks> Tasks { get; set; }

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
                userId = int.Parse(cookie);
                User = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userId);
                Friends = _unitOfWork.FriendsRepository.GetAll().Where(u => u.UserId1 == userId);
                Projects = _unitOfWork.ProjectsRepository.GetAll().Where(u => u.UserId == userId);
                Tasks = _unitOfWork.TasksRepository.GetAll().Where(u => u.UserId == userId);
            }
        }
    }
}
