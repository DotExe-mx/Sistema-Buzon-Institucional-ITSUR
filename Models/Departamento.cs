using System;
using System.ComponentModel.DataAnnotations;
namespace SIS_QSF.Models
{
    public class Departamento
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo no puede quedar vacio.")]
        public string Nombre { get; set; }
    }
}
