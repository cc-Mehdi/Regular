using Datalayer.Models;
using Datalayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<JsonResult> OnGetGetOrganizationById(int id)
        {
            Organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == id);
            return new JsonResult(Organization);
        }


        // get organizations by logged in user id
        public async Task<JsonResult> OnGetGetOrganizationsByLoggedInUserId()
        {
            isUserLogin();
            OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
            return new JsonResult(OrganizationsList);
        }

        // upsert organization
        public async Task<JsonResult> OnPostUpsertOrganizationAsync()
        {
            isUserLogin();

            if (!hasAccessToCreateOrganization())
                return new JsonResult(new { err = "شما به محدودیت ساخت سازمان رسیده اید!" });

            var title = Request.Form["Title"];
            var image = Request.Form.Files["ImageName"];
            var employees = Request.Form["Employees"].ToList();
            var id = Request.Form["Id"];

            // Generate a unique file name

            // convert data to model for sending to database
            var newItem = new Organizations
            {
                Title = title,
                ImageName = "/CustomerResources/DefaultSources/OrgImage.png",
                OwnerId = loggedInUser.Id,
                Owner = loggedInUser
            };

            if (String.IsNullOrEmpty(id) || id == "0")
                _unitOfWork.OrganizationsRepository.Add(newItem);
            else
            {
                newItem.Id = int.Parse(id);
                _unitOfWork.OrganizationsRepository.Update(newItem);
            }

            _unitOfWork.Save();

            var OrganizationsList = _unitOfWork.OrganizationsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id).ToList();
            Organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == OrganizationsList[0].Id);
            return new JsonResult(OrganizationsList);
        }

        public async Task<JsonResult> OnGetGetProjectById(int id)
        {
            Project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == id);
            return new JsonResult(Project);
        }

        // upsert project
        public async Task<JsonResult> OnPostUpsertProjectAsync()
        {
            isUserLogin();

            var title = Request.Form["Title"];
            var image = Request.Form.Files["ImageName"];
            var employeesId = Request.Form["Employees"].ToList();
            var orgId = Request.Form["orgId"];
            var organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == int.Parse(orgId));
            var id = Request.Form["Id"];

            if (orgId == "0")
                return new JsonResult(new { err = "لطفا ابتدا یک سازمان ایجاد کنید" });

            var newProject = new Projects
            {
                Title = title,
                ImageName = image == null ? "" : image.FileName,
                OwnerId = loggedInUser.Id,
                Owner = loggedInUser,
                Organization = organization,
                OrganizationId = organization.Id,
                TasksCount = 0,
                TasksStatusPercent = 0
            };

            if (String.IsNullOrEmpty(id) || id == "0")
                _unitOfWork.ProjectsRepository.Add(newProject);
            else
            {
                newProject.Id = int.Parse(id);
                _unitOfWork.ProjectsRepository.Update(newProject);
            }

            _unitOfWork.Save();

            _unitOfWork.User_ProjectRepository.RemoveRange(_unitOfWork.User_ProjectRepository.GetAllByFilter(u => u.ProjectId == newProject.Id).ToList());

            // Add relations between users and projects
            foreach (var empId in employeesId)
            {
                _unitOfWork.User_ProjectRepository.Add(new User_Project() // add new relation to database
                {
                    ProjectId = newProject.Id,
                    UserId = int.Parse(empId)
                });
            }

            _unitOfWork.Save();

            var projectsList = _unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OwnerId == loggedInUser.Id && u.OrganizationId == organization.Id).ToList();
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

        //get tasks by organization id
        public async Task<JsonResult> OnGetGetTasksByOrganizationId(int organizationId)
        {
            TasksList = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.Project.OrganizationId == organizationId).ToList();
            return new JsonResult(TasksList);
        }

        public async Task<JsonResult> OnGetGetTasksByProjectId(int projectId)
        {
            TasksList = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.ProjectId == projectId).ToList();
            return new JsonResult(TasksList);
        }

        public async Task<JsonResult> OnGetGetTaskById(int id)
        {
            HttpContext.Session.SetInt32("TaskId", id);
            Task = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.Id == id).FirstOrDefault();
            return new JsonResult(Task);
        }

        // get tasks by filter
        public async Task<JsonResult> OnGetGetTasksByFilter(string filterParameter, string orgId)
        {
            isUserLogin();

            if (filterParameter == null)
                TasksList = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.Project.OrganizationId == int.Parse(orgId)).ToList();
            else
                TasksList = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.Project.OrganizationId == int.Parse(orgId) && u.Title.Contains(filterParameter)).ToList();
            return new JsonResult(TasksList);
        }


        // add new task
        public async Task<JsonResult> OnPostUpsertTaskAsync()
        {
            isUserLogin();

            var title = Request.Form["title"].ToString();
            var priority = Request.Form["priority"].ToString();
            var assigneeId = Request.Form["Employees"].ToString();
            var assignTo = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == int.Parse(assigneeId))?.FullName.ToString();
            var estimateTime = Request.Form["estimateTime"].ToString();
            var description = Request.Form["description"].ToString();
            var projectId = Request.Form["projectId"].ToString();
            var reporterId = loggedInUser.Id;
            var organizationId = Request.Form["orgId"];
            var id = Request.Form["Id"];

            // Validate the input data (simple validation example)
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(priority) || string.IsNullOrEmpty(assignTo) ||
                string.IsNullOrEmpty(estimateTime) || string.IsNullOrEmpty(description))
            {
                return new JsonResult(new { err = "لطفا فیلدها را با دقت پر کنید" });
            }

            if (projectId == "0" || string.IsNullOrEmpty(projectId))
                return new JsonResult(new { err = "پروژه مورد نظر خود را انتخاب کنید" });

            try
            {
                if (!(String.IsNullOrEmpty(id) || id == "0"))
                {
                    Task = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.Id == int.Parse(id)).FirstOrDefault();
                    Task.Project.TasksCount--;
                }

                var project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == int.Parse(projectId));

                // Convert data to model for sending to database
                var newItem = new Tasks
                {
                    Title = title,
                    Priority = priority,
                    Assignto = assignTo,
                    AssigntoId = int.Parse(assigneeId),
                    EstimateTime = estimateTime,
                    LoggedTime = "-",
                    RemainingTime = estimateTime,
                    Description = description,
                    ProjectId = int.Parse(projectId),
                    Project = project,
                    ReporterId = reporterId,
                    TaskStatus = "برای انجام",
                    TaskType = "بهبود"
                };

                

                if (String.IsNullOrEmpty(id) || id == "0")
                    _unitOfWork.TasksRepository.Add(newItem);
                else
                {
                    newItem.Id = int.Parse(id);
                    _unitOfWork.TasksRepository.Update(newItem);
                }

                newItem.Project.TasksCount++;

                _unitOfWork.Save();

                TasksList = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.Project.OrganizationId == int.Parse(organizationId)).ToList();
                return new JsonResult(TasksList);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, return error response, etc.)
                return new JsonResult(new { err = "خطایی در انجام عملیات وجود دارد" });
            }
        }


        // get employees by organization id
        public async Task<JsonResult> OnGetGetEmployeesByOrganizationId(int organizationId)
        {
            var usersList = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u => u.OrganizationId == organizationId && u.InviteStatus == "پذیرفته شد").Select(u => u.User).ToList();
            return new JsonResult(usersList);
        }

        // add new employee
        public async Task<JsonResult> OnPostAddEmployeeAsync()
        {
            isUserLogin();

            var username = Request.Form["username"].ToString();
            var orgId = Request.Form["orgId"].ToString();

            User = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Username == username);

            if(User == null)
                return new JsonResult(new { err = "کاربر مورد نظر یافت نشد" });

            if (string.IsNullOrEmpty(username))
                return new JsonResult(new { err = "لطفا فیلدها را با دقت پر کنید" });

            if (orgId == "0")
                return new JsonResult(new { err = "لطفا ابتدا یک سازمان ایجاد کنید" });

            try
            {
                var oldRequest = _unitOfWork.Organizations_UsersRepository.GetFirstOrDefault(u => u.UserId == User.Id && u.OrganizationId == int.Parse(orgId));
                if(oldRequest != null)
                    if(oldRequest.InviteStatus != "رد شده")
                        return new JsonResult(new { err = "قبلا به این کاربر درخواست داده اید" });

                var newItem = new Organizations_Users
                {
                    InviteStatus = "در انتظار پاسخ",
                    OrganizationId = int.Parse(orgId),
                    User = User,
                    UserId = User.Id
                };

                _unitOfWork.Organizations_UsersRepository.Add(newItem);
                _unitOfWork.Save();

                return await OnGetGetEmployeesByOrganizationId(int.Parse(orgId));
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, return error response, etc.)
                return new JsonResult(new { err = "خطایی در انجام عملیات وجود دارد/n" + ex.InnerException.Message.ToString() });
            }
        }

        // get employees by filter
        public async Task<JsonResult> OnGetGetEmployeesByFilter(string filterParameter, string orgId)
        {
            isUserLogin();

            if (filterParameter == null)
                UsersList = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u => u.OrganizationId == int.Parse(orgId) && u.InviteStatus == "پذیرفته شد").Select(u => u.User).ToList();
            else
                UsersList = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u => u.OrganizationId == int.Parse(orgId) && u.InviteStatus == "پذیرفته شد").Select(u => u.User).Where(u => u.FullName.Contains(filterParameter) || u.Username.Contains(filterParameter)).ToList();
            return new JsonResult(UsersList);
        }

        // get users with project id (json)
        public async Task<JsonResult> OnGetGetUsersByProjectId(int projectId)
        {
            UsersList = _unitOfWork.User_ProjectRepository.GetAllByFilterIncludeRelations(u => u.ProjectId == projectId).Select(u => u.User).ToList();
            return new JsonResult(UsersList);
        }

        // get users with project id (data)
        public void GetUsersByProjectId(int projectId)
        {
            UsersList = _unitOfWork.User_ProjectRepository.GetAllByFilterIncludeRelations(u => u.ProjectId == projectId).Select(u => u.User).ToList();
            // Remove duplicates based on Id
            UsersList = UsersList
                .GroupBy(user => user.Id)
                .Select(group => group.First())
                .ToList();
        }

        // get sent employee invites by organization id
        public async Task<JsonResult> OnGetGetSentEmployeeInvitesByOrganizationId(int organizationId)
        {
            var list = _unitOfWork.Organizations_UsersRepository.GetAllByFilterIncludeRelations(u => u.OrganizationId == organizationId && u.UserId != loggedInUser.Id).Select(u => new { u.User.Id, u.User.FullName, u.User.Username, u.User.ImageName, u.InviteStatus }).ToList();
            return new JsonResult(list);
        }

        public async Task<JsonResult> OnGetGetUserById(int userId)
        {
            User = _unitOfWork.UsersRepository.GetAllByFilter(u => u.Id == userId).FirstOrDefault();
            return new JsonResult(User);
        }

        // DELETE SECTION
        //Delete Organization
        public async Task<JsonResult> OnGetDeleteOrganization(int id)
        {
            try
            {   
                Organization = _unitOfWork.OrganizationsRepository.GetFirstOrDefault(u => u.Id == id);
                
                if(Organization == null)
                    return new JsonResult(new { errorMessage = "سازمان مورد نظر یافت نشد" });

                _unitOfWork.Organizations_UsersRepository.RemoveRange(_unitOfWork.Organizations_UsersRepository.GetAllByFilter(u => u.OrganizationId == Organization.Id));
                _unitOfWork.ProjectsRepository.RemoveRange(_unitOfWork.ProjectsRepository.GetAllByFilter(u => u.OrganizationId == Organization.Id));
                _unitOfWork.TasksRepository.RemoveRange(_unitOfWork.TasksRepository.GetAllByFilter(u => u.Project.OrganizationId == Organization.Id));
                _unitOfWork.OrganizationsRepository.Remove(Organization);
                _unitOfWork.Save();

                return new JsonResult(new { errorMessage = ""});
            }
            catch (Exception ex)
            {
                return new JsonResult(new { errorMessage = "حذف سازمان با شکست مواجه شد" });
            }
        }

        //Delete Project
        public async Task<JsonResult> OnGetDeleteProject(int id)
        {
            try
            {
                // Delete related User_Project records
                var userProjects = _unitOfWork.User_ProjectRepository.GetAllByFilter(up => up.ProjectId == id).ToList();
                _unitOfWork.User_ProjectRepository.RemoveRange(userProjects);

                // Delete related Task records
                var tasksList = _unitOfWork.TasksRepository.GetAllByFilter(u => u.ProjectId == id).ToList();
                _unitOfWork.TasksRepository.RemoveRange(tasksList);

                // Delete the Project
                var project = _unitOfWork.ProjectsRepository.GetFirstOrDefault(u => u.Id == id);
                _unitOfWork.ProjectsRepository.Remove(project);

                _unitOfWork.Save();
                return new JsonResult(new { errorMessage = "" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { errorMessage = "حذف پروژه با شکست مواجه شد" });
            }
        }

        //Delete Task
        public async Task<JsonResult> OnGetDeleteTask(int id)
        {
            try
            {
                Task = _unitOfWork.TasksRepository.GetAllByFilterIncludeRelations(u => u.Id == id).FirstOrDefault();
                Task.Project.TasksCount--;
                _unitOfWork.TasksRepository.Remove(Task);
                _unitOfWork.Save();
                return new JsonResult(new { errorMessage = "" });
            }
            catch
            {
                return new JsonResult(new { errorMessage = "حذف وظیفه با شکست مواجه شد" });
            }
        }

        //Delete Employee
        public async Task<JsonResult> OnGetDeleteEmployee(int id)
        {
            try
            {
                var employeeRelation = _unitOfWork.Organizations_UsersRepository.GetFirstOrDefault(u => u.UserId == id);
                _unitOfWork.Organizations_UsersRepository.Remove(employeeRelation);
                _unitOfWork.Save();
                return new JsonResult(new { errorMessage = "" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { errorMessage = "لغو همکاری با شکست مواجه شد" });
            }
        }


    }
}
