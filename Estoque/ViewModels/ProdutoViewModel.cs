using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_sis_evenda.Estoque.ViewModels
{
    public class ProdutoVendasViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Codigo é obrigatório!")]
        [MaxLength(5, ErrorMessage = "O campo Codigo deve ter 5 caracteres!"), MinLength(5, ErrorMessage = "O campo Codigo deve ter 5 caracteres!")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório!")]
        [MaxLength(20, ErrorMessage = "O campo Nome deve ter no máximo 20 caracteres!"), MinLength(3, ErrorMessage = "O campo Nome deve ter no mínimo 3 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Preco é obrigatório!")]
        [Range(0, 9999999.99, ErrorMessage = "Entre um valor válido para o preço >= 0.00")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo Quantidade é obrigatório!")]
        [Range(0, int.MaxValue, ErrorMessage = "Entre um valor válido para o campo Quantidade >= 0")]
        public int Quantidade { get; set; }
    }
}
