
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models
{
    [Table("DetallePedido")]
    public class DetallePedido
    {
        [Key]
        public int IdDetalle { get; set; }

        [Required]
        public int IdPedido { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999999)]
        public decimal PrecioUnitario { get; set; }
    }
}
