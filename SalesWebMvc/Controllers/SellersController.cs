using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        //INICIO INJEÇÃO DE DEPENDENCIA
        //1 ---> declarar uma dependencia para o SellerServices
        private readonly SellerService _sellerService;
        //2 ---> criar o construtor
        public SellersController (SellerService sellerService)
        {
            _sellerService = sellerService;
        }
        //FIM INJEÇÃO DE DEPENDENCIA
        //3 ---> implementar a chamada do FindAll
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();//retorna a lista de seller
            return View(list);//exibe a lista
        }

        public IActionResult Create()
        {
            return View();
        }
        //metodo para inserir no bd
        [HttpPost] //necessário colocar esta "anotação" para indicar que a ação do método abaixo é de POST e não de GET
        [ValidateAntiForgeryToken]//anotação de proteção
        public IActionResult Create(Seller seller)
        {//no insere o vendedor passado no metodo no SellerService
            _sellerService.Insert(seller);
            //para retornar para a página index
            //return RedirectToAction("Index"); --> poderia ser assim, mas caso mude o nome do index, teria que mudar aqui também
            return RedirectToAction(nameof(Index));
        }
    }
}