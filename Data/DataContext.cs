//representação de nosso banco de dados InMemory
//orientação de nossa aplicação em relação ao nosso banco de dados

using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data{
    public class DataContext :DbContext{
        //opções
        public DataContext(DbContextOptions<DataContext> options): base(options){

        }

        //são os DbSets - representação de nossas tabelas em memória
        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<User> Users {get; set;}
    }
}