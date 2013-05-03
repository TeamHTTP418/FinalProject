using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ReadyPlayerSite.Models
{
    public class FreezeInfo : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [DisplayName("Account Frozen At")]
        public DateTime freezeDate { get; set; }
        [DisplayName("Freeze Reason")]
        public string reason { get; set; }

        [DisplayName("Held Attendance Score")]
        public int attendanceScore { get; set; }
        [DisplayName("Held Puzzle Score")]
        public int puzzleScore { get; set; }
        [DisplayName("Held Cross-Curricular Score")]
        public int crossCurricularScore { get; set; }
        [DisplayName("Held Cooperation Score")]
        public int cooperationScore { get; set; }
        [DisplayName("Held Story Score")]
        public int storyScore { get; set; }

        public virtual Player player { get; set; }

        public void addPointsToPlayer()
        {
            player.attendanceScore += attendanceScore;
            player.cooperationScore += cooperationScore;
            player.crossCurricularScore += crossCurricularScore;
            player.puzzleScore += puzzleScore;
            player.storyScore += storyScore;
        }
    }
}
