using Microsoft.EntityFrameworkCore;
using PruebaExamen.Models;

namespace PruebaExamen.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            :base(options){ }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
    }
}
