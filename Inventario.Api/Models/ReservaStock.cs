
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Api.Models
{
    [Table("ReservaStock")]
    public class ReservaStock
    {
        [Key]
        public int IdReserva { get; set; }

        public int IdProducto { get; set; }

        public int IdBodega { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [RegularExpression("Pendiente|Confirmada|Cancelada")]
        public string Estado { get; set; } = "Pendiente";
    }
}
