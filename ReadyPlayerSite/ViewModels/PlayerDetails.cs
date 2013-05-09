using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadyPlayerSite.Models;

namespace ReadyPlayerSite.ViewModels
{
    public class PlayerDetails
    {
        public Player player;
        public List<PlayerToTask> milestones;
        public List<PlayerToTask> tasks;
    }
}