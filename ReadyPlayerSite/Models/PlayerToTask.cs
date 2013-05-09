using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadyPlayerSite.Models
{
    public class PlayerToTask
    {
        [Key]
        public int ID { get; set; }


        public int playerID { get; set; }
        public virtual Player player {get; set; }

        public int taskID { get; set; }
        public virtual Task task { get; set; }

        [DisplayName("Completion Date")]
        public DateTime when { get; set; }

        public static PlayerToTask GetPTT(Player player, Task task){
            return new PlayerToTask { player = player, task = task, when = DateTime.Now };
        }
    }
}