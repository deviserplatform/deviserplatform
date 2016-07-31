using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Modules.Security.Models
{
    public class TestViewModel
    {
        [Required]        
        public string Field1 { get; set; }

        [Required]          
        public string Field2 { get; set; }
    }
}
