using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage.Friends
{
    public class FriendsListModel : PageModel
    {
        int userId = 0;
        public IEnumerable<DataLayer.Models.Friends> Friends { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public FriendsListModel(IUnitOfWork unitOfWork)
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
                Friends = _unitOfWork.FriendsRepository.GetAll().Where(u => u.UserId1 == userId).ToList();
            }
        }
    }
}
