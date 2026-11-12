
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        public int IdCliente { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(12,2)")]
        [Range(0, 99999999)]
        public decimal Total { get; set; }

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Pendiente";
    }
}
