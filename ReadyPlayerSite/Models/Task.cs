using System;
using System.Collections.Generic;
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
        public override int ID { get; set; }
        public string name { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public TaskType type { get; set; }
        public int value { get; set; }

        //Milestone Options
        public bool isMilestone { get; set; }
        public int? bonusPoints { get; set; }
        public int? numberCompleted { get; set; }
        public int? maxCompletedBonus { get; set; }
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
