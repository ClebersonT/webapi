using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

//Endpoint = URL
//https://localhost:5001
//http://localhost:5000
namespace webapi.Controllers{
    [Route("v1/categories")]

    //Foi incluido os Tasks apartir da versão 6, para tra balhar de forma assincrona
    //não trava a thread principal da aplicação
    //tornando a aplicação muito mais rapida - programação paralela
    public class CategoryController : ControllerBase{

        //por padrão se eu não espor o verbo, automaticamente ele é do tipo get
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        //ActionResult => traz o resultado no formato que a tela espera
        //async => basicamente cria threads paralelas para a execução, não travando assim a aplicação
        //O Task pode receber uma tipagem, no caso foi o tipo Category
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        //desabilita cache
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)];
        public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context){
            //AsNoTracking => realiza uma leitura mais rápida possível e "joga" na tela
            //toda vez que executamos um .ToList ele executa a query, então toda e qualquer query terá que vir antes do .ToListAsync
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        //tudo que entra com chaves é encarado como parâmetro
        //:int => estou restringindo o que esta entrando como parametro, nesse caso se entrar um string, a api trará um 404 ao invés de buscar o solicitado
        [Route("{id:int}")]
        [AllowAnonymous]
        //para passar para o metodo é so criar um parâmetro
        public async Task<ActionResult<Category>> GetById(int id, [FromServices]DataContext context){
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return Ok(category);
        }
        
        [HttpPost]
        [Route("")]
        //funcíonarios
        [Authorize(Roles = "employeed")]
        public async Task<ActionResult<List<Category>>> Post([FromBody]Category model, [FromServices]DataContext context){

            //verificando se as validações que foram passadas no model estão de acordo
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try{
                //adicionando uma categoria, mesma forma para SqlServer,Postgres etc
                context.Categories.Add(model);
                //salvando as mudanças de forma assincrona... aguardar até que as mudanças sejam salvas
                //Na Hora que realiza a op SaveChanges => gera um id automático =>  ja preenche o Id do model
                await context.SaveChangesAsync();
                return Ok(model);
            }catch{
                return BadRequest(new{message = "Não foi possível criar uma categoria, tente novamente"});
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        //funcíonarios
        [Authorize(Roles = "employeed")]
        public async Task<ActionResult<List<Category>>> Put(int id, [FromBody] Category model, [FromServices]DataContext context){
            //verifica se Id informado é o mesmo do modelo
            if (id != model.Id)
                //objetos dinâmicos dentro do C#, ao inves de só mostrar um 404, posso exibir uma mensagem na tela
                return NotFound(new { message = "Categoria não encontrada"});
            //verifica se os dados são validos
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try{
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model) ;
            }
            catch(DbUpdateConcurrencyException){
                //concorrência =>  ao tentar atualizar um registro, esse mesmo registro sendo atualizado ao mesmo tempo
                return BadRequest(new { message = "Houve uma concorrência ao atualizar este registro"});
            }
            catch(Exception){
                return BadRequest(new { message = "Não foi possível atualizar a categoria, tente novamente" });
            }
        
        }

        [HttpDelete]
        [Route("{id:int}")]
        //funcíonarios
        [Authorize(Roles = "employeed")]
        public async Task<ActionResult<List<Category>>> Delete(int id, [FromServices]DataContext context){
            /*FirstOrDefaultAsync           => Busca uma categoria dada uma expressão
            caso ache mais de uma categoria => ele pega a primeira
            se ele não achar nada           => irá retornal null
            e se tiver apenas uma           => irá pegar somente uma*/
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category == null)
                return NotFound(new{message = "Categoria não encontrada"});
        
        try{
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso" });
        }
        catch (Exception){
            return BadRequest(new{message = "Não foi possível remover a categoria, tente novamente"});
        }
        }
    }
}