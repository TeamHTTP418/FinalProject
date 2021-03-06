﻿using System;
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
        private static readonly object mileLock = new object();
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        public virtual User user { get; set; }

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

        public virtual ICollection<PlayerToTask> tasksCompleted { get; set; }

        public virtual ICollection<AdminAction> adminActions { get; set; }

        public int? freezeInfoID { get; set; }
        public virtual FreezeInfo freezeInfo { get; set; }

        public int totalScore()
        {
            return attendanceScore + puzzleScore + crossCurricularScore + cooperationScore + storyScore;
        }

        public bool addPoints(TaskType type, int amount)
        {
            switch (type)
            {
                case TaskType.Attendance:
                    attendanceScore += amount;
                    break;
                case TaskType.Cooperation:
                    cooperationScore += amount;
                    break;
                case TaskType.CrossCurricular:
                    crossCurricularScore += amount;
                    break;
                case TaskType.Puzzle:
                    puzzleScore += amount;
                    break;
                case TaskType.Story:
                    storyScore += amount;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public bool addHeldPoints(TaskType type, int amount)
        {
            switch (type)
            {
                case TaskType.Attendance:
                    freezeInfo.attendanceScore += amount;
                    break;
                case TaskType.Cooperation:
                    freezeInfo.cooperationScore += amount;
                    break;
                case TaskType.CrossCurricular:
                    freezeInfo.crossCurricularScore += amount;
                    break;
                case TaskType.Puzzle:
                    freezeInfo.puzzleScore += amount;
                    break;
                case TaskType.Story:
                    freezeInfo.storyScore += amount;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public bool addTaskPoints(Task task)
        {
            bool result = false;
            if (isFrozen())
            {
                result =  addHeldPoints(task.type, task.value);
                if (result && task.isMilestone && task.numberCompleted < task.maxCompletedBonus)
                {
                    lock (mileLock)
                    {
                        if (task.numberCompleted < task.maxCompletedBonus)
                        {
                            result = addHeldPoints(task.type, task.bonusPoints.Value);
                            task.numberCompleted++;
                        }
                    }
                }
            }
            else
            {
                result = addPoints(task.type, task.value);
                if (result && task.isMilestone && task.numberCompleted < task.maxCompletedBonus)
                {
                    lock (mileLock)
                    {
                        if (task.numberCompleted < task.maxCompletedBonus)
                        {
                            result = addPoints(task.type, task.bonusPoints.Value);
                            task.numberCompleted++;
                        }
                    }
                }
            }
            return result;
        }

        public bool isFrozen()
        {
            return freezeInfo != null;
        }
    }
}