using MVC_CodeFirst.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_CodeFirst.Models
{
    [Table("TB_M_Role")]
    public class Role : BaseModel
    {        
        public string Name { get; set; }
    }
}