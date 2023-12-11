using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Regular.Pages.Customer.Manage.Friends
{
    [BindProperties]
    public class FriendDeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DataLayer.Models.Friends Friend { get; set; }

        public FriendDeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(int id)
        {
            Friend = _unitOfWork.FriendsRepository.GetFirstOrDefault(u => u.Id == id);
        }

        public async Task<IActionResult> OnPost()
        {
            //Get selected coffee for remove
            var firendFromDb = _unitOfWork.FriendsRepository.GetFirstOrDefault(u => u.Id == Friend.Id);

            if (firendFromDb != null)
            {
                _unitOfWork.FriendsRepository.Remove(firendFromDb);
                //Save db
                _unitOfWork.Save();

                return RedirectToPage("/Customer/Manage/Friends/FriendsList");
            }
            return Page();
        }
    }
}
