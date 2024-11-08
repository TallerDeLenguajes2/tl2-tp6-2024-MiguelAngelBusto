using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_MiguelAngelBusto.Models;
using System.Collections.Generic;
using System.Linq;
using tl2_tp6_2024_MiguelAngelBusto.Repositorios;
using tl2_tp6_2024_MiguelAngelBusto.Models;

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

    [HttpGet]
    public IActionResult Crear(){
        return View(new Productos());
    }

    [HttpPost]
    public IActionResult Crear(Productos aux){
        repositorio.Create(aux);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult EditarProducto(int idProducto)
    {  
        Productos prod;
        prod = repositorio.GetByID(idProducto);
        return View(prod);
    }


    [HttpPost]
    public IActionResult EditarProducto(Productos item)
    {   
        repositorio.Update(item.IdProducto,item);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteProducto(int idProducto){
        repositorio.Delete(idProducto);
        return RedirectToAction("Index");
    }
}