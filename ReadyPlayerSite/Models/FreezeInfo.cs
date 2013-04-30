using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyPlayerSite.Models
{
    public class FreezeInfo : IModel
    {
        public override int ID { get; set; }

        public DateTime freezeDate { get; set; }
        public string reason { get; set; }

        public int attendanceScore { get; set; }
        public int puzzleScore { get; set; }
        public int crossCurricularScore { get; set; }
        public int cooperationScore { get; set; }
        public int storyScore { get; set; }

        public int playerID { get; set; }
        public virtual Player player { get; set; }
    }
}
