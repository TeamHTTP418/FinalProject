using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Providers.Entities;

namespace ReadyPlayerSite.Models
{
    public enum AdminActionType
    {
        AddPoints,
        RemovePoints,
        FreezeAccount,
        UnfreezeAccount
    }
    public class AdminAction : IModel
    {
        public override int ID { get; set; }

        public AdminActionType type { get; set; }
        public TaskType? modifyTarget { get; set; }
        public int? value { get; set; }

        public DateTime when { get; set; }
        public string reason { get; set; }

        public int playerID { get; set; }
        public virtual Player player { get; set; }

        public int userID { get; set; }
        public virtual User user { get; set; }
    }
}
