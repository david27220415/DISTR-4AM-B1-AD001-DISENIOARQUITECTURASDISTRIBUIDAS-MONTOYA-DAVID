
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalogo.Api.Models
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        public int IdCategoria { get; set; }

        [Required]
        public int IdMarca { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999999)]
        public decimal Precio { get; set; }

        public bool Estado { get; set; } = true;

        [ForeignKey(nameof(IdCategoria))]
        public Categoria? Categoria { get; set; }

        [ForeignKey(nameof(IdMarca))]
        public Marca? Marca { get; set; }
    }
}
