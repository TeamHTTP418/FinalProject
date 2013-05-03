using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [DisplayName("Action Taken")]
        public AdminActionType type { get; set; }
        [DisplayName("Score Modified")]
        public TaskType? modifyTarget { get; set; }
        [DisplayName("Amount")]
        public int? value { get; set; }

        [DisplayName("Issued At")]
        public DateTime when { get; set; }
        [DisplayName("Details")]
        public string reason { get; set; }

        [ScaffoldColumn(false)]
        public int playerID { get; set; }
        public virtual Player player { get; set; }

        [ScaffoldColumn(false)]
        public int userID { get; set; }
        public virtual User user { get; set; }

        public bool PerformAction(PlayerDbContext db)
        {
            switch (type)
            {
                case AdminActionType.AddPoints:
                    return player.addPoints(modifyTarget.Value, value.Value);
                case AdminActionType.RemovePoints:
                    return player.addPoints(modifyTarget.Value, -value.Value);
                case AdminActionType.FreezeAccount:
                    if (player.freezeInfo == null)
                    {
                        player.freezeInfo = new FreezeInfo { freezeDate = DateTime.Now, reason = reason, player = player };
                        return true;
                    }
                    break;
                case AdminActionType.UnfreezeAccount:
                    if (player.freezeInfo != null)
                    {
                        player.freezeInfo.addPointsToPlayer();
                        db.freezeInfos.Remove(player.freezeInfo);
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
    }
}
