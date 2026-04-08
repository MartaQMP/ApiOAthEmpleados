using ApiOAthEmpleados.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOAthEmpleados.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options): base(options) {}

        public DbSet<Empleado> Empleados { get; set; }
    }
}
