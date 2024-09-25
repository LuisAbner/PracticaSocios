using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Socios.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Departamento> departamentos {get;set;} = new List<Departamento>();
        public IEnumerable<Asociados> asociados {get;set;} = new List<Asociados>();
        public List<SelectListItem> DepartamentosSelect{get;set;}= new List<SelectListItem>();
        public int TypeResponse {get;set;}
    }
}