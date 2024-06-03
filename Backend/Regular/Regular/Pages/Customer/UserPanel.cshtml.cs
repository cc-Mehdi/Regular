﻿using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Regular.Pages.Customer
{
    public class UserPanelModel : PageModel
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

        public UserPanelModel(IUnitOfWork unitOfWork)
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
            int organizationId = OrganizationsList.Count == 0 ? 0 : OrganizationsList[0].Id;
            ProjectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == organizationId).ToList();
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
            Organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == OrganizationsList[0].Id);
            return new JsonResult(OrganizationsList);
        }

        // add new project
        public async Task<JsonResult> OnPostAddProjectAsync()
        {
            isUserLogin();

            var title = Request.Form["Title"];
            var image = Request.Form.Files["ImageName"];
            var employees = Request.Form["Employees"].ToList();
            var orgId = Request.Form["orgId"];
            var organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == int.Parse(orgId));

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
            ProjectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == organizationId).ToList();
            return new JsonResult(ProjectsList);
        }

        public async Task<JsonResult> OnGetGetProjectsByFilter(string filterParameter, string orgId)
        {
            isUserLogin();
            var organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == int.Parse(orgId));

            if (filterParameter == null)
                ProjectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == organization.Id).ToList();
            else
                ProjectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == organization.Id && u.Title.Contains(filterParameter)).ToList();
            return new JsonResult(ProjectsList);
        }

        public async Task<JsonResult> OnGetGetTasksByOrganizationId(int organizationId)
        {
            TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u=> u.Project.OrganizationId == organizationId).ToList();
            return new JsonResult(TasksList);
        }

        public async Task<JsonResult> OnGetGetTasksByFilter(string filterParameter, string orgId)
        {
            isUserLogin();

            if (filterParameter == null)
                TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.Project.OrganizationId == int.Parse(orgId)).ToList();
            else
                TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.Project.OrganizationId == int.Parse(orgId) && u.Title.Contains(filterParameter)).ToList();
            return new JsonResult(TasksList);
        }


        // add new task
        public async Task<JsonResult> OnPostAddTaskAsync()
        {
            isUserLogin();

            var title = Request.Form["title"].ToString();
            var priority = Request.Form["priority"].ToString();
            var assigneeId = Request.Form["Employees"].ToString();
            var assignTo = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == int.Parse(assigneeId)).FullName.ToString();
            var estimateTime = Request.Form["estimateTime"].ToString();
            var remainingTime = Request.Form["remainingTime"].ToString();
            var loggedTime = Request.Form["loggedTime"].ToString();
            var description = Request.Form["description"].ToString();
            var projectId = Request.Form["ProjectId"].ToString();
            var reporterId = loggedInUser.Id;
            var organizationId = Request.Form["orgId"];

            // Validate the input data (simple validation example)
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(priority) || string.IsNullOrEmpty(assignTo) ||
                string.IsNullOrEmpty(estimateTime) || string.IsNullOrEmpty(remainingTime) || string.IsNullOrEmpty(loggedTime) || string.IsNullOrEmpty(description))
            {
                return new JsonResult(new { err = "لطفا فیلدها را با دقت پر کنید" });
            }

            try
            {
                var project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == int.Parse(projectId));
                // Convert data to model for sending to database
                var newItem = new Tasks
                {
                    Title = title,
                    Priority = priority,
                    Assignto = assignTo,
                    AssigntoId = int.Parse(assigneeId),
                    EstimateTime = estimateTime,
                    RemainingTime = remainingTime,
                    LoggedTime = loggedTime,
                    Description = description,
                    ProjectId = int.Parse(projectId),
                    Project = project,
                    ReporterId = reporterId
                };

                _unitOfWork.TasksRepository.Add(newItem);
                _unitOfWork.Save();

                TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.Project.OrganizationId == int.Parse(organizationId)).ToList();
                return new JsonResult(TasksList);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, return error response, etc.)
                return new JsonResult(new { err = "خطایی در انجام عملیات وجود دارد" });
            }
        }

        public async Task<JsonResult> OnGetGetEmployeesByOrganizationId(int organizationId)
        {
            UsersList = _unitOfWork.EmployeeInvitesRepository.GetAllByFilter(u => u.OrganizationId == organizationId && u.InviteStatus == "accepted").Select(u=> u.User).ToList();
            return new JsonResult(UsersList);
        }
    }
}
