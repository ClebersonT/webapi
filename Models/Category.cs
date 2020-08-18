//dataAnnotations ajuda no mapeamento e a gerar o banco da forma correta, tamanho de campo, chaves, etc
using System.ComponentModel.DataAnnotations;
//tudo referente aos esquemas do SQLSERVER, nome de tabela, nome de campo, tipo de dado etc
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models{

    /*posso mudar o nome da tabela caso queira
    [Table("Categoria")] por exemplo, ao deixar sem a tabela terá o nome de Category mesmo
    mesma coisa com as colunas
    [Column("Cat_ID")] e ao deixar em branco pegará a nomeclatura de acordo com as props
    tipo de campo
    [DataType("nvarchar")]*/
    
    public class Category{
        [Key]
        public int Id {get; set;}

        //informações passadas ao data Annotation
        //posso deixar somente required, porem colocará uma mensagem padrão e em ingles!
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Title {get; set;}
    }
}