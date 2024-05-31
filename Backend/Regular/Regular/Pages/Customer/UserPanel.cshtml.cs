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

            TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.ReporterId == loggedInUser.Id || u.AssigntoId == loggedInUser.Id).ToList();
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

        public async Task<JsonResult> OnGetGetProjectsByFilter(string filterParameter, string orgTitle)
        {
            isUserLogin();
            var organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.OwnerId == loggedInUser.Id && u.Title == orgTitle);

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

        public async Task<JsonResult> OnGetGetTasksByFilter(string filterParameter, string orgTitle)
        {
            isUserLogin();
            var organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.OwnerId == loggedInUser.Id && u.Title == orgTitle);

            if (filterParameter == null)
                TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.Project.OrganizationId == organization.Id).ToList();
            else
                TasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.Project.OrganizationId == organization.Id && u.Title.Contains(filterParameter)).ToList();
            return new JsonResult(ProjectsList);
        }


        // add new task
        public async Task<JsonResult> OnPostAddTaskAsync()
        {
            isUserLogin();

            var title = Request.Form["title"].ToString();
            var priority = Request.Form["priority"].ToString();
            var assignTo = Request.Form["assignTo"].ToString();
            var assigneeId = Convert.ToInt32(Request.Form["Employees"].ToString()); // Assuming only one employee is selected
            var estimateTime = Request.Form["estimateTime"].ToString();
            var remainingTime = Request.Form["remainingTime"].ToString();
            var loggedTime = Request.Form["loggedTime"].ToString();
            var description = Request.Form["description"].ToString();
            var projectId = Convert.ToInt32(Request.Form["ProjectId"].ToString()); // Assuming projectId is passed as a hidden input
            var reporterId = loggedInUser.Id;
            var organizationId = Request.Form["orgId"];

            // Validate the input data (simple validation example)
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(priority) || string.IsNullOrEmpty(assignTo) ||
                string.IsNullOrEmpty(estimateTime) || string.IsNullOrEmpty(remainingTime) || string.IsNullOrEmpty(loggedTime) || string.IsNullOrEmpty(description))
            {
                return new JsonResult(new { err = "All fields are required." });
            }

            try
            {
                // Convert data to model for sending to database
                var newItem = new Tasks
                {
                    Title = title,
                    Priority = priority,
                    Assignto = assignTo,
                    AssigntoId = assigneeId,
                    EstimateTime = estimateTime,
                    RemainingTime = remainingTime,
                    LoggedTime = loggedTime,
                    Description = description,
                    ProjectId = projectId,
                    ReporterId = reporterId
                };

                _unitOfWork.TasksRepository.Add(newItem);
                _unitOfWork.Save();

                var tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.Project.OwnerId == loggedInUser.Id && u.Project.OrganizationId == organizationId).ToList();
                return new JsonResult(tasksList);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, return error response, etc.)
                return new JsonResult(new { err = "An error occurred while adding the task." });
            }
        }
    }
}
