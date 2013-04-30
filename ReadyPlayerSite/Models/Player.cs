using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;

namespace ReadyPlayerSite.Models
{
    public class Player : IModel
    {
        public override int ID { get; set; }

        public int userID { get; set; }
        public virtual User user { get; set; }

        public string eid { get; set; }

        public int attendanceScore { get; set; }
        public int puzzleScore { get; set; }
        public int crossCurricularScore { get; set; }
        public int cooperationScore { get; set; }
        public int storyScore { get; set; }

        public virtual ICollection<Task> milestonesCompleted { get; set; }
        public virtual ICollection<Task> tasksCompleted { get; set; }

        public virtual ICollection<AdminAction> adminActions { get; set; }

       // public bool frozen { get; set; } //TODO: Check if needed
        public int? freezeInfoID { get; set; }
        public virtual FreezeInfo freezeInfo { get; set; }

        public int TotalScore()
        {
            return attendanceScore + puzzleScore + crossCurricularScore + cooperationScore + storyScore;
        }

    }
}