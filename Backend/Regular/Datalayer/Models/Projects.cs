﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
    public class Projects
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام پروژه")]
        [Required(ErrorMessage = "لطفا {0} را صحیح وارد کنید")]
        [MaxLength(300, ErrorMessage = "مقدار {0} بیش از حد مجاز طولانی است")]
        [DataType(DataType.Text)]
        public string ProjectName { get; set; }

        [Display(Name = "تعداد وظایف پروژه")]
        public int TasksCount { get; set; }

        [Display(Name = "کد مسئول")]
        public int UserId { get; set; }

        [Display(Name = "مسئول")]
        [ForeignKey("Users")]
        public Users User { get; set; }
    }
}
