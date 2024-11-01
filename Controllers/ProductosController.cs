using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MiguelAngelBusto.Models;
using System.Collections.Generic;
using System.Linq;
using tl2_tp6_2024_MiguelAngelBusto.Repositorios;

namespace tl2_tp6_2024_MiguelAngelBusto.Controllers;

public class ProductosController : Controller {

    private ProductosRepository repositorio;

    public ProductosController (){
        repositorio = new ProductosRepository();
    }

    public IActionResult Index()
    {
        return View(repositorio.GetProductos());
    }



}