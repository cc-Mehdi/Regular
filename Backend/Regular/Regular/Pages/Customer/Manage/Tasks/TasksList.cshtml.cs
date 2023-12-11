using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage.Tasks
{
    public class TasksListModel : PageModel
    {
        int userId = 0;
        public IEnumerable<DataLayer.Models.Tasks> Tasks { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public TasksListModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void OnGet()
        {
            var cookie = Request.Cookies["UserId"];
            if (cookie == null)
            {
                Response.Redirect("/Customer/Login-Register/Login-Register");
            }
            else
            {
                userId = int.Parse(cookie);
                Tasks = _unitOfWork.TasksRepository.GetAll().Where(u => u.UserId == userId).ToList();
            }
        }
    }
}
