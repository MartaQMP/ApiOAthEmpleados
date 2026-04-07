using ApiOAthEmpleados.Models;
using ApiOAthEmpleados.Repositories;
using ApiOAuthEmpleados.Helpers;
using ApiOAuthEmpleados.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace ApiOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryHospital repo;
        private HelperActionOAuthService helper;

        public AuthController(RepositoryHospital repo, HelperActionOAuthService helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> LogIn(LogInModel model)
        {
            Empleado empleado = await this.repo.LogInEmpleadoAsync(model.UserName, int.Parse(model.Password));
            if(empleado == null)
            {
                return Unauthorized();
            }else
            {
                // DEBEMOS CREAR UNAS CREDENCIALES PARA NUESTRO TOKEN
                SigningCredentials credentials = new SigningCredentials(this.helper.GetTokenKey(), SecurityAlgorithms.HmacSha256);
                // EL TOKEN SE GENERA CON UNA CLASE Y DEBEMOS ALMACENAR LOS DATOS DE ISSUER, CREDENTIALS...
                JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    notBefore: DateTime.UtcNow
                );
                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });

            }
        }
    }
}
