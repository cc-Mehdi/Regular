﻿using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Friends
    {
        [Key]
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
    }
}
