using ApiOAthEmpleados.Models;
using ApiOAthEmpleados.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiOAthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryHospital repo;

        public EmpleadosController(RepositoryHospital repo) 
        { 
            this.repo = repo; 
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> Get()
        {
            return await this.repo.GetEmpleadosAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> FindEmpleado(int id)
        {
            return await this.repo.FindEmpleadoAsync(id);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Empleado>> Perfil()
        {
            Claim claim = HttpContext.User.FindFirst(z => z.Type == "UserData");
            string jsonEmpleado = claim.Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            return await this.repo.FindEmpleadoAsync(empleado.IdEmpleado);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Empleado>>> Compis()
        {
            Claim claim = HttpContext.User.FindFirst(z => z.Type == "UserData");
            string jsonEmpleado = claim.Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            return await this.repo.GetCompisAsync(empleado.IdDepartamento);
        }
    }
}
