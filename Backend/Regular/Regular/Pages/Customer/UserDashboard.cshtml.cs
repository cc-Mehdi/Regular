using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace Regular.Pages.Customer
{
    public class UserDashboardModel : PageModel
    {
        public Users loggedInUser;
        private readonly IUnitOfWork _unitOfWork;

        public List<Organizations> OrganizationsList;
        public List<Projects> ProjectsList;
        public List<Tasks> TasksList;
        public List<Users> UsersList;

        public Organizations Organization;
        public Projects Project;
        public Tasks Task;
        public Users User;

        public UserDashboardModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            loggedInUser = new();

            OrganizationsList = new();
            ProjectsList = new();
            TasksList = new();
            UsersList = new();

            Organization = new();
            Project = new();
            Task = new();
            User = new();
        }
        
        public void OnGet()
        {
            // check is user logged-in
            isUserLogin();

            TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.ReporterId == loggedInUser.Id || u.AssigntoId == loggedInUser.Id).ToList();
            OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
            int organizationId = OrganizationsList.Count == 0 ? 0 : OrganizationsList.FirstOrDefault().Id;
            ProjectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == organizationId).ToList();
            UsersList = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u => u.OrganizationId == organizationId && u.InviteStatus == "پذیرفته شد").Select(u => u.User).ToList();
        }


        public void isUserLogin()
        {
            if (Request.Cookies["loginToken"] == null)
                Response.Redirect("/Customer/Login-Register");
            else
            {
                string loginToken = Request.Cookies["loginToken"];
                int userId = _unitOfWork.LoginsLogRepository.GetFirstOrDefault(u => u.LoginToken == loginToken).UserId;
                loggedInUser = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userId);
            }
        }

        public async Task<JsonResult> OnGetGetLoggedInUser()
        {
            isUserLogin();
            return new JsonResult(loggedInUser);
        }

        // get recived employee invites by logged in user id
        public async Task<JsonResult> OnGetGetSentEmployeeInvites()
        {
            isUserLogin();
            var list = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u => u.UserId == loggedInUser.Id && u.InviteStatus == "در انتظار پاسخ").Select(u => new { u.Id, u.Organization.ImageName,  u.Organization.Title, OwnerName = u.Organization.Owner.FullName }).ToList();
            return new JsonResult(list);
        }

        // get recived employee invites COUNT by logged in user id
        public async Task<JsonResult> OnGetGetSentEmployeeInvitesCount()
        {
            isUserLogin();
            var requestsCount = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u => u.UserId == loggedInUser.Id && u.InviteStatus == "در انتظار پاسخ").Select(u => new { u.Id, u.Organization.ImageName, u.Organization.Title, OwnerName = u.Organization.Owner.FullName }).ToList().Count;
            return new JsonResult(new {count= requestsCount});
        }

        // get all organizations that logged in user work with them
        public async Task<JsonResult> OnGetGetRelationOrganizations()
        {
            isUserLogin();
            var list = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u=> u.UserId == loggedInUser.Id && u.InviteStatus == "پذیرفته شد").ToList();
            return new JsonResult(list);
        }
    }
}
