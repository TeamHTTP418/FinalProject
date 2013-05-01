using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;

namespace ReadyPlayerSite.Models
{
    public class Player : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [ScaffoldColumn(false)]
        public int userID { get; set; }
        public virtual User user { get; set; }

        [DisplayName("EID")]
        [RegularExpression(@"\d{9}")]
        public string eid { get; set; }

        [DisplayName("Attendance Score")]
        public int attendanceScore { get; set; }
        [DisplayName("Puzzle Score")]
        public int puzzleScore { get; set; }
        [DisplayName("Cross-Curricular Score")]
        public int crossCurricularScore { get; set; }
        [DisplayName("Cooperation Score")]
        public int cooperationScore { get; set; }
        [DisplayName("Story Score")]
        public int storyScore { get; set; }

        public virtual ICollection<Task> milestonesCompleted { get; set; }
        public virtual ICollection<Task> tasksCompleted { get; set; }

        public virtual ICollection<AdminAction> adminActions { get; set; }

        public int? freezeInfoID { get; set; }
        public virtual FreezeInfo freezeInfo { get; set; }

        public int TotalScore()
        {
            return attendanceScore + puzzleScore + crossCurricularScore + cooperationScore + storyScore;
        }

        public bool addTaskPoints(Task task)
        {
            switch (task.type)
            {
                case TaskType.Attendance:
                    attendanceScore += task.value;
                    break;
                case TaskType.Cooperation:
                    cooperationScore += task.value;
                    break;
                case TaskType.CrossCurricular:
                    crossCurricularScore += task.value;
                    break;
                case TaskType.Puzzle:
                    puzzleScore += task.value;
                    break;
                case TaskType.Story:
                    storyScore += task.value;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public bool isFrozen()
        {
            return freezeInfoID.HasValue;
        }
    }
}