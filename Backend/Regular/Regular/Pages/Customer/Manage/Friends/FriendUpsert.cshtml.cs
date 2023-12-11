using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage.Friends
{
    [BindProperties]
    public class FriendUpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DataLayer.Models.Friends Friend { get; set; }

        public FriendUpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Friend = new();
        }

        public void OnGet(int? id = 0)
        {
            if (id != 0)
                Friend = _unitOfWork.FriendsRepository.GetFirstOrDefault(u => u.Id == id);
        }

        public async Task<IActionResult> OnPost()
        {
            var cookie = Request.Cookies["UserId"];
            int userId = int.Parse(cookie);
            Friend.UserId1 = userId;
            //Create
            if (Friend.Id == 0)
            {
                _unitOfWork.FriendsRepository.Add(Friend);
                _unitOfWork.Save();
            }
            else //Edit
            {
                _unitOfWork.FriendsRepository.Update(Friend);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Manage/Friends/FirendsList");
        }
    }
}
