using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MiguelAngelBusto.Models;
using System.Collections.Generic;
using System.Linq;
using tl2_tp6_2024_MiguelAngelBusto.Repositorios;
using tl2_tp6_2024_MiguelAngelBusto.Models;
namespace tl2_tp6_2024_MiguelAngelBusto.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestosRepository repository;

    public PresupuestosController (){
        repository = new PresupuestosRepository();
    }

    public IActionResult Index()
    {
        return View(repository.GetPresupuestos());
    }
}