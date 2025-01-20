using API.Models.Enum;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.POCO
{   
    /// <summary>
    /// POCO model of Users
    /// </summary>
    public class USR01
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int R01F01 { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string R01F02 { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string R01F03 { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string R01F04 { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        public EnmRole R01F05 { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        [IgnoreOnUpdate]
        public DateTime R01F06 { get; set; } = DateTime.Now;

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime R01F07 { get; set; } = DateTime.Now;
    }
}