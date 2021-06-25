using System;
using System.ComponentModel.DataAnnotations;
namespace SIS_QSF.Models
{
    public class Solicitud
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "El campo no puede quedar vacio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo no puede quedar vacio.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Numero de Movil invalido!.")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo no puede quedar vacio.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo no puede quedar vacio.")]
       
        public string Descripcion { get; set; }
        [Display(Name ="Subir tu evidencia")]
        public string ImagenPath { get; set; }

        public Departamento Depa {get;set;}
    }
}
