﻿using Datalayer.Models;
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
            int organizationId = OrganizationsList.Count == 0 ? 0 : OrganizationsList[0].Id;
            ProjectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == organizationId).ToList();
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

        // add new organization
        public async Task<JsonResult> OnPostAddOrganizationAsync()
        {
            isUserLogin();

            if (!hasAccessToCreateOrganization())
                return new JsonResult(new { err = "شما به محدودیت ساخت سازمان رسیده اید!" });

            var title = Request.Form["Title"];
            var image = Request.Form.Files["ImageName"];
            var employees = Request.Form["Employees"].ToList();

            // convert data to model for sending to database
            var newItem = new Organizations
            {
                Title = title,
                ImageName = image == null ? "" : image.FileName,
                OwnerId = loggedInUser.Id,
                Owner = loggedInUser.FullName == null ? "" : loggedInUser.FullName
            };

            _unitOfWork.OrganizationsRepository.Add(newItem);
            _unitOfWork.Save();

            var OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
            return new JsonResult(OrganizationsList);
        }

        // add new project
        public async Task<JsonResult> OnPostAddProjectAsync()
        {
            isUserLogin();

            var title = Request.Form["Title"];
            var image = Request.Form.Files["ImageName"];
            var employees = Request.Form["Employees"].ToList();
            string orgTitle = Request.Form["orgTitle"];
            var organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.OwnerId == loggedInUser.Id && u.Title == orgTitle);

            // convert data to model for sending to database
            var newItem = new Projects
            {
                Title = title,
                ImageName = image == null ? "" : image.FileName,
                OwnerId = loggedInUser.Id,
                Owner = loggedInUser.FullName == null ? "" : loggedInUser.FullName,
                Organization = organization.Title,
                OrganizationId = organization.Id,
                TasksCount = 0,
                TasksStatusPercent = 0
            };

            _unitOfWork.ProjectsRepository.Add(newItem);
            _unitOfWork.Save();

            var projectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id&&u.OrganizationId==organization.Id).ToList();
            return new JsonResult(projectsList);
        }

        private bool hasAccessToCreateOrganization()
        {
            int organicationsCount = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).Count();
            if (organicationsCount < 3)
                return true;
            return false;
        }

        public async Task<JsonResult> OnGetGetProjectsByOrganizationId(int organizationId)
        {
            // Load projects based on selected organization
            ProjectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == organizationId).ToList();
            return new JsonResult(ProjectsList);
        }
    }
}
