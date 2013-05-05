using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ReadyPlayerSite.Models
{
    public enum TaskType
    {
        Attendance,
        Puzzle,
        CrossCurricular,
        Cooperation,
        Story
    };
    public class Task : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }
        [DisplayName("Task Name")]
        public string name { get; set; }
        [DisplayName("Start Date")]
        public DateTime? start { get; set; }
        [DisplayName("End Date")]
        public DateTime? end { get; set; }
        [DisplayName("Type")]
        public TaskType type { get; set; }
        [DisplayName("Point Value")]
        public int value { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
        [DisplayName("Unique ID Token")]
        public string token { get; set; }
        [DisplayName("Solution Keyword")]
        public string solution { get; set; }

        //Milestone Options
        public bool isMilestone { get; set; }
        [DisplayName("Bonus Points")]
        public int? bonusPoints { get; set; }
        public int? numberCompleted { get; set; }
        [DisplayName("Number of Bonus Awards")]
        public int? maxCompletedBonus { get; set; }
        [DisplayName("Icon")]
        public string iconName { get; set; }


        public bool isAvailable()
        {
            //There is a starting date and it has not occurred yet
            if (start.HasValue && start.Value > DateTime.Now) 
            {
                return false;
            }
            //There is an ending date and it has already occurred
            if (end.HasValue && end.Value < DateTime.Now)
            {
                return false;
            }

            return true;
        }

    }
}
