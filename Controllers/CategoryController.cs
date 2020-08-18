using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;

//Endpoint = URL
//https://localhost:5001
//http://localhost:5000

[Route("categories")]

//Foi incluido os Tasks apartir da versão 6, para trabalhar de forma assincrona
//não trava a thread principal da aplicação
//tornando a aplicação muito mais rapida - programação paralela
public class CategoryController : ControllerBase{

    //por padrão se eu não espor o verbo, automaticamente ele é do tipo get
    [HttpGet]
    [Route("")]
    //ActionResult => traz o resultado no formato que a tela espera
    //async => basicamente cria threads paralelas para a execução, não travando assim a aplicação
    //O Task pode receber uma tipagem, no caso foi o tipo Category
    public async Task<ActionResult<List<Category>>> Get(){
        return new List<Category>();
    }

     [HttpGet]
     //tudo que entra com chaves é encarado como parâmetro
     //:int => estou restringindo o que esta entrando como parametro, nesse caso se entrar um string, a api trará um 404 ao invés de buscar o solicitado
    [Route("{id:int}")]
    //para passar para o metodo é so criar um parâmetro
    public async Task<ActionResult<Category>> GetById(int id){
        return new Category();
    }
    
     [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Post([FromBody]Category model){
        return Ok(model);
    }

     [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put(int id, [FromBody] Category model){
        if(model.Id == id)
            return Ok(model);
        return NotFound();
    }

     [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Delete(){
        return Ok();
    }
}