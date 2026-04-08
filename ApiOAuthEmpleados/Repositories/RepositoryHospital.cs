using ApiOAthEmpleados.Data;
using ApiOAthEmpleados.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOAthEmpleados.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int id)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == id);
        }

        public async Task<List<Empleado>> GetCompisAsync(int idDept)
        {
            return await this.context.Empleados.Where(e => e.IdDepartamento == idDept).ToListAsync();
        }

        public async Task<Empleado> LogInEmpleadoAsync(string apellido, int idEmpleado)
        {
            return await this.context.Empleados.Where(e => e.Apellido == apellido && e.IdEmpleado == idEmpleado).FirstOrDefaultAsync();
        }
    }
}
