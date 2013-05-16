using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReadyPlayerSite.Models
{
    [Table("Users")]
    public class User : IModel
    {
        public User()
        {
            admin = false;
        }

        [ScaffoldColumn(false)]
        [Key]
        public override int ID { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "The Username is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The username must be between 3 and 100 characters")]
        public String username { get; set; }

        [ScaffoldColumn(false)]
        public bool admin { get; set; }
    }
}