﻿using System.ComponentModel.DataAnnotations;

namespace OrtizAbrahamSprint3.Data.Entities
{
    public class WeekDays
    {
        [Key]
        public int WeekDaysID { get; set; }
        public string WeekDaysName { get; set; }
    }
}
