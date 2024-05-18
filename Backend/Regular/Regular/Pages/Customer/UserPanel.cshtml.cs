using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Regular.Pages.Customer
{
    public class UserPanelModel : PageModel
    {
        private Users loggedInUser;
        private readonly IUnitOfWork _unitOfWork;

        public List<Organizations> OrganizationsList;
        public List<Projects> ProjectsList;
        public List<Tasks> TasksList;
        public List<Users> EmployeesList;

        public Organizations Organization;
        public Projects Projects;
        public Tasks Tasks;
        public Users Employee;

        public UserPanelModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            loggedInUser = new();

            OrganizationsList = new();
            ProjectsList = new();
            TasksList = new();
            EmployeesList = new();

            Organization = new();
            Projects = new();
            Tasks = new();
            Employee = new();
        }

        public void OnGet()
        {
            // check is user logged-in
            isUserLogin();

            OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
            var allorgs = _unitOfWork.OrganizationsRepository.GetAll();
        }

        private void isUserLogin()
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

        public JsonResult OnPostInsertOrganization(Organizations organization)
        {
            isUserLogin();
            // add new organization
            organization.Owner = loggedInUser.FullName == null? "" : loggedInUser.FullName;
            organization.OwnerId = loggedInUser.Id;
            organization.ImageName = "";
            _unitOfWork.OrganizationsRepository.Add(organization);
            _unitOfWork.Save();


            OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
            return new JsonResult(OrganizationsList);
        }

        public async Task<JsonResult> OnPostAddOrganizationAsync()
        {
            isUserLogin();


            var title = Request.Form["Title"];
            var image = Request.Form.Files["ImageName"];
            var employees = Request.Form["Employees"].ToList();

            // اینجا داده‌ها را به مدل تبدیل کنید و به دیتابیس اضافه کنید
            var newItem = new Organizations
            {
                Title = title,
                ImageName = image == null ? "" : image.FileName,
                OwnerId = loggedInUser.Id,
                Owner = loggedInUser.FullName == null ? "" : loggedInUser.FullName
            // سایر خصوصیات مانند تصویر و همکاران را تنظیم کنید
        };

            _unitOfWork.OrganizationsRepository.Add(newItem);
            _unitOfWork.Save();

            var OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
            return new JsonResult(OrganizationsList);
        }

        public JsonResult OnGetBindOrganizations()
        {
            var data = new List<Organizations>();
            OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();

            foreach(var org in OrganizationsList)
                data.Add(org);

            return new JsonResult(data);
        }
    }
}
