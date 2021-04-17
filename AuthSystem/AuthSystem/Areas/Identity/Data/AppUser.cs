using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Areas.Identity.Data
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Nickname { get; set; }

        [PersonalData]
        [Column(TypeName = "datetime")]
        public virtual DateTime? LastLoginTime { get; set; }

        [PersonalData]
        [Column(TypeName = "datetime")]
        public virtual DateTime? RegistrationDate { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Status { get; set; }

    }
}
