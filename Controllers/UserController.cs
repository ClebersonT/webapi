using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using webapi.Services;

namespace webapi.Controllers{
    [Route("users")]
    public class UserController : Controller{
        [HttpGet]
        [Route("")]
        //so vou permitir apenas gerentes vendo todos os usuários
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get ([FromServices] DataContext context){
            var users = await context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        //[Authorize(Roles="manager")]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model){
            //verificando dados validos
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try{
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return model;
            }catch(Exception){
                return BadRequest(new {message = "Não foi possível criar o usuário, tente novamente!"});
            }
        }
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put([FromServices] DataContext context, int id, [FromBody] User model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != model.Id)
                return NotFound(new{ message = "Usuário não encontrado"});
            
            try{
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }catch(Exception){
                return BadRequest(new { message = "Não foi possivel atualizar usuário"}); 
            }
        }

        [HttpPost]
        [Route("login")]
        //dynamic pois as vezes retorna um user ou retorna mensagem
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model){
            var user = await context.Users.AsNoTracking().Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefaultAsync();
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválido!"});
            
            var token = TokenService.GenerateToken(user);
            return new{
                user = user,
                token = token
            };
            
        }
    }
}