﻿using DataLayer.Models;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Regular.Pages.Manage
{
    public class ManageModel : PageModel
    {
        private int userId = 0;
        private readonly IUnitOfWork _unitOfWork;
        public Users User { get; set; }
        public Tasks Task { get; set; }
        public List<Users> Users { get; set; }
        public IEnumerable<Friends> Friends { get; set; }
        public IEnumerable<Projects> Projects { get; set; }
        public IEnumerable<Tasks> Tasks { get; set; }


        public int allTasksCount = 0;
        public int inProgressTasksCount = 0;
        public int doneTasksCount = 0;
        public int inPreviewTasksCount = 0;
        public int userTasksCount = 0;

        public ManageModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            isUserLogin();
            if(userId != 0)
            {
                User = _unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == userId);
                Friends = _unitOfWork.FriendsRepository.GetAll().Where(u => u.UserId1 == userId).ToList();
                Projects = _unitOfWork.ProjectsRepository.GetAll().Where(u => u.UserId == userId).ToList();
                Tasks = _unitOfWork.TasksRepository.GetAll().Where(u => u.UserId == userId).ToList();

                allTasksCount = Tasks.Count();
                inProgressTasksCount = Tasks.Where(u => u.TaskStatus == "درحال انجام").ToList().Count();
                doneTasksCount = Tasks.Where(u => u.TaskStatus == "انجام شده").ToList().Count();
                inPreviewTasksCount = Tasks.Where(u => u.TaskStatus == "برای بررسی").ToList().Count();
                userTasksCount = Tasks.Where(u => u.UserId == userId).ToList().Count();

                Users = new List<Users>();
                foreach (var friend in Friends)
                    Users.Add(_unitOfWork.UsersRepository.GetFirstOrDefault(u => u.Id == friend.UserId2));
            }
        }

        private void isUserLogin()
        {
            if (Request.Cookies["UserId"] == null)
            {
                Response.Redirect("/Customer/Login-Register/Login-Register");
            }
            else
                userId = int.Parse(Request.Cookies["UserId"]);
        }
    }
}
