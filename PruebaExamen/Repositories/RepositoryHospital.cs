using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PruebaExamen.Data;
using PruebaExamen.Models;
using System.Data;
using System.Diagnostics.Metrics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PruebaExamen.Repositories
{
    #region
    //create or alter procedure SP_EMPLEADO_DEPARTAMENTO_OUT
    //(@posicion int, @deptno int, @registros int out)
    //as
    //--todos los registro de un departamento
    //select @registros = COUNT(EMP_NO) from EMP
    //where DEPT_NO = @deptno
    //SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO from
    //(select CAST(ROW_NUMBER() over(order by APELLIDO) as int) AS POSICION,
    //EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
    //from EMP
    //where DEPT_NO = @deptno) as query
    //where query.POSICION = @posicion
    //go

    //exec SP_EMPLEADO_DEPARTAMENTO_OUT 1, 10, 0
    #endregion
    public class RepositoryHospital
    {
        private HospitalContext context;
        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }
        public async Task<Empleado> FindEmpleadoAsync
            (int idEmpleado)
        {
            Empleado empleado = await this.context.Empleados
                .Where(x => x.IdEmpleado == idEmpleado)
                .FirstOrDefaultAsync();
            return empleado;
        }
        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            var departamentos = await this.context.Departamentos.ToListAsync();
            return departamentos;
        }
        public async Task<Departamento> FindDepartamentoAsync
            (int idDepartamento)
        {
            var departamento = await this.context.Departamentos
                .Where(x => x.IdDepartamento == idDepartamento)
                .FirstOrDefaultAsync();
            return departamento;
        }
        public async Task<ModelEmpleadoDepartamento> GetEmpleadoDepartamentoOutAsync
            (int? posicion, int deptNo)
        {
            string sql = "SP_EMPLEADO_DEPARTAMENTO_OUT @posicion, @deptno, @registros OUT";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamDeptNo = new SqlParameter("@deptno", deptNo);
            SqlParameter pamRegistros = new SqlParameter("@registros", 0);
            pamRegistros.Direction = ParameterDirection.Output;

             var consulta = 
                this.context.Empleados.FromSqlRaw(sql, pamPosicion, pamDeptNo, pamRegistros);


            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = int.Parse(pamRegistros.Value.ToString());
            return new ModelEmpleadoDepartamento
            {
                NumeroRegistros = registros,
                Empleados = empleados
            };
        }
    }
}
