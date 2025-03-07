using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PruebaExamen.Models;
using PruebaExamen.Repositories;

namespace PruebaExamen.Controllers
{
    public class HospitalController : Controller
    {
        private RepositoryHospital repo;
        public HospitalController(RepositoryHospital repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Departamentos()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }
        public async Task<IActionResult> Details
            (int? posicion, int idDepartamento)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            
            ModelEmpleadoDepartamento model =
                await this.repo.GetEmpleadoDepartamentoOutAsync(posicion.Value, idDepartamento);
            Departamento departamento =
                await this.repo.FindDepartamentoAsync(idDepartamento);

            ViewData["DEPTSELECCIONADO"] = departamento;
            ViewData["REGISTROS"] = model.NumeroRegistros;
            ViewData["DEPARTAMENTO"] = idDepartamento;

            int siguiente = posicion.Value + 1;
            if (siguiente > model.NumeroRegistros)
            {
                siguiente = model.NumeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["UTLIMO"] = model.NumeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return View(model.Empleados);
        }
        public async Task<IActionResult> _EmpleadoDepartamentoOutPartial
            (int? posicion, int idDepartamento)
        {
            if (posicion == null)
            {
                posicion = 1;
                return View();
            }
            else
            {
                ModelEmpleadoDepartamento model = await
                    this.repo.GetEmpleadoDepartamentoOutAsync(posicion.Value, idDepartamento);
                Departamento departamento = await this.repo.FindDepartamentoAsync(idDepartamento);
                ViewData["DEPARTAMENTOSELECCIONADO"] = departamento;
                ViewData["REGISTROS"] = model.NumeroRegistros;
                ViewData["DEPTNO"] = idDepartamento;

                int siguiente = posicion.Value + 1;
                if (siguiente > model.NumeroRegistros)
                {
                    siguiente = model.NumeroRegistros;
                }
                int anterior = posicion.Value - 1;
                if (anterior < 1)
                {
                    anterior = 1;
                }
                ViewData["ULTIMO"] = model.NumeroRegistros;
                ViewData["SIGUIENTE"] = siguiente;
                ViewData["ANTERIOR"] = anterior;
                ViewData["POSICION"] = posicion;
                return PartialView("_EmpleadoDepartamentoOutPartial", model.Empleados);
            }
        }

    }
}
