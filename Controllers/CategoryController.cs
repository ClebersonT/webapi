using Microsoft.AspNetCore.Mvc;
using webapi.Models;

//Endpoint = URL
//https://localhost:5001
//http://localhost:5000

[Route("categories")]
public class CategoryController : ControllerBase{

    //por padrão se eu não espor o verbo, automaticamente ele é do tipo get
    [HttpGet]
    [Route("")]
    public string Get(){
        return "GET";
    }

     [HttpGet]
     //tudo que entra com chaves é encarado como parâmetro
     //:int => estou restringindo o que esta entrando como parametro, nesse caso se entrar um string, a api trará um 404 ao invés de buscar o solicitado
    [Route("{id:int}")]
    //para passar para o metodo é so criar um parâmetro
    public string GetById(int id){
        return id.ToString();
    }
    
     [HttpPost]
    [Route("")]
    public Category Post([FromBody]Category model){
        return model;
    }

     [HttpPut]
    [Route("{id:int}")]
    public Category Put(int id, [FromBody] Category model){
        if(model.Id == id)
            return model;
        return null;
    }

     [HttpGet]
    [Route("{id:int}")]
    public string Delete(){
        return "DELETE";
    }
}