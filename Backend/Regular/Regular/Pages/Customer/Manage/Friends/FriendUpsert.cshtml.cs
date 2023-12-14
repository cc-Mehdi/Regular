using DataLayer.Models;
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
                if (userId == Friend.UserId2)
                {
                    return Page();
                }

                IEnumerable<DataLayer.Models.Friends> firends = _unitOfWork.FriendsRepository.GetAll();
                foreach (var item in firends)
                {
                    if (item.UserId2 == Friend.UserId2)
                    {
                        return Page();
                    }
                }

                var users = _unitOfWork.UsersRepository.GetAll();
                bool isUserExist = false;
                foreach (var user in users)
                    if (Friend.UserId2 == user.Id)
                        isUserExist = true;
                if(!isUserExist)
                {
                    return Page();
                }    
             

                _unitOfWork.FriendsRepository.Add(Friend);
                _unitOfWork.Save();
            }
            else //Edit
            {
                _unitOfWork.FriendsRepository.Update(Friend);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Manage/friends/friendsList");
        }
    }
}
