using System.ComponentModel.DataAnnotations;

namespace webapi.Models{
    public class Product{   
        [Key]
        public int Id {get; set;}

        //informações passadas ao data Annotation
        //posso deixar somente required, porem colocará uma mensagem padrão e em ingles!
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Title {get; set;}

        [MaxLength(1024, ErrorMessage = "Este campo deve conter no maximo 1024 caracteres")]
        public string Description {get; set;}

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        //Range pois sendo decimal o valor terá que ser maio que o indicado abaixo
        //int.MaxValue é o maio preço que seu produto pode chegar
        [Range(1, int.MaxValue,ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Price {get; set;}

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Range(1, int.MaxValue,ErrorMessage = "Categoria inválida")]
        public int CategoryId {get; set;}

        public Category Category {get; set;}
    }
}