using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}