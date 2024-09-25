using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socios.Models
{
    public class Asociados
    {
        public int IdAsociado{get;set;}
        public string Nombre {get;set;}="";
        public string Apellido {get;set;} ="";
        public double Salario {get;set;}
        public int DepartamentoId{get;set;}
        public string NombreDepartamento {get;set;}="";
        
    }
}