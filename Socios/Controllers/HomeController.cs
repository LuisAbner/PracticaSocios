using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Socios.Models;
using Socios.Services;
using System.Diagnostics;

namespace Socios.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDepartamentoRepo _dtoRepo;
        private readonly IAsociadosRepo _asoRepo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IDepartamentoRepo departamentoRepo, IAsociadosRepo asociadosRepo)
        {
            _dtoRepo = departamentoRepo;
            _logger = logger;
            _asoRepo = asociadosRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel vm = new IndexViewModel();
            vm.departamentos = await _dtoRepo.GetAll();
            vm.asociados = await _asoRepo.GetAll();
            if (vm.departamentos is not null)
            {
                vm.DepartamentosSelect = vm.departamentos.Select(
                    x => new SelectListItem { Text = x.Nombre, Value = x.IdDepartamento.ToString() }
                ).ToList();
                
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<PartialViewResult> UpdateSelectAso()
        {
            IndexViewModel vm = new IndexViewModel();
            vm.departamentos = await _dtoRepo.GetAll();
            vm.asociados = await _asoRepo.GetAll();
            if (vm.departamentos is not null)
            {
                vm.DepartamentosSelect = vm.departamentos.Select(
                    x => new SelectListItem { Text = x.Nombre, Value = x.IdDepartamento.ToString() }
                ).ToList();                
            }
            return PartialView("_select", vm);
        }
        [HttpPost]
        public async Task<PartialViewResult> UpdateSelectAsoAdd()
        {
            IndexViewModel vm = new IndexViewModel();
            vm.departamentos = await _dtoRepo.GetAll();
            vm.asociados = await _asoRepo.GetAll();
            if (vm.departamentos is not null)
            {
                vm.DepartamentosSelect = vm.departamentos.Select(
                    x => new SelectListItem { Text = x.Nombre, Value = x.IdDepartamento.ToString() }
                ).ToList();                
            }
            return PartialView("_selectAdd", vm);
        }


        [HttpPost]
        public async Task<PartialViewResult> AddDto(Departamento departamento)
        {
            IndexViewModel vm = new IndexViewModel();

            var reponse = await _dtoRepo.Save(departamento);
            vm.departamentos = await _dtoRepo.GetAll();
            if (reponse > 0)
            {
                vm.TypeResponse = 1;
            }
            return PartialView("_tableDto", vm);
        }

        [HttpPost]
        public async Task<PartialViewResult> AddAso(Asociados aso)
        {
            IndexViewModel vm = new IndexViewModel();

            var reponse = await _asoRepo.Save(aso);
            vm.asociados = await _asoRepo.GetAll();
            if (reponse > 0)
            {
                vm.TypeResponse = 1;
            }
            return PartialView("_tableAso", vm);
        }
        [HttpPost]
        public async Task<PartialViewResult> UpdateTabelAso()
        {
            IndexViewModel vm = new IndexViewModel();
            vm.asociados = await _asoRepo.GetAll();
            return PartialView("_tableAso", vm);
        }

        [HttpPost]
        public async Task<PartialViewResult> DeleteDto(int id)
        {
            IndexViewModel vm = new IndexViewModel();

            await _dtoRepo.DeleteById(id);
            vm.departamentos = await _dtoRepo.GetAll();

            vm.TypeResponse = 1;

            return PartialView("_tableDto", vm);
        }
        [HttpPost]
        public async Task<PartialViewResult> DeleteAso(int id)
        {
            IndexViewModel vm = new IndexViewModel();

            await _asoRepo.DeleteById(id);
            vm.asociados = await _asoRepo.GetAll();

            vm.TypeResponse = 1;

            return PartialView("_tableAso", vm);
        }
        [HttpPost]
        public async Task<PartialViewResult> EditDto(Departamento departamento)
        {
            IndexViewModel vm = new IndexViewModel();

            await _dtoRepo.Update(departamento);
            vm.departamentos = await _dtoRepo.GetAll();

            vm.TypeResponse = 1;

            return PartialView("_tableDto", vm);
        }
        [HttpPost]
        public async Task<PartialViewResult> EditSalarioDto(int id, double porcentaje)
        {
            IndexViewModel vm = new IndexViewModel();

            await _dtoRepo.UpdateSalario(id,(porcentaje/100));
            vm.departamentos = await _dtoRepo.GetAll();
            vm.TypeResponse = 1;
            return PartialView("_tableDto", vm);
        }

        [HttpPost]
        public async Task<PartialViewResult> EditAso(Asociados asociados)
        {
            IndexViewModel vm = new IndexViewModel();

            await _asoRepo.Update(asociados);
            
            vm.asociados = await _asoRepo.GetAll();

            vm.TypeResponse = 1;

            return PartialView("_tableAso", vm);
        }

        [HttpPost]
        public async Task<Departamento> GetDto(int id)
        {
            return await _dtoRepo.GetById(id);
        }
        [HttpPost]
        public async Task<Asociados> GetAso(int id)
        {
            return await _asoRepo.GetById(id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}