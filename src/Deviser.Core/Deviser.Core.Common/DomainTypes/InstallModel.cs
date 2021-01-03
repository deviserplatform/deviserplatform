using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Deviser.Core.Common.DomainTypes
{
    public enum DatabaseProvider
    {
        SqlServer = 1,
        SqlLite = 2,
        PostgreSql = 3,
        MySql = 4,
    }

    public class InstallModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Admin Email")]
        public string AdminEmail { get; set; }

        [JsonIgnore]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Admin Password")]
        public string AdminPassword { get; set; }

        [JsonIgnore]
        [Required]
        [Compare("AdminPassword")]
        [Display(Name = "Re-enter Password")]
        [DataType(DataType.Password)]
        public string AdminConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Required]
        [Display(Name = "Database Provider")]
        public DatabaseProvider DatabaseProvider { get; set; }

        [Display(Name = "Server Name")]
        public string ServerName { get; set; }

        [Required]
        [Display(Name = "Database")]
        public string DatabaseName { get; set; }

        [Display(Name = "Integrated Security")]
        public bool IsIntegratedSecurity { get; set; }

        [Display(Name = "Database Username")]
        public string DBUserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Database Password")]
        public string DBPassword { get; set; }

        [Display(Name = "Accept License")]
        public bool AcceptLicense { get; set; }
    }
}
