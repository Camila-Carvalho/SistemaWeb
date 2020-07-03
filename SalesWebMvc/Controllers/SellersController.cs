﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        //INICIO INJEÇÃO DE DEPENDENCIA
        //1 ---> declarar uma dependencia para o SellerServices
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        //2 ---> criar o construtor
        public SellersController (SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
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
            //para carregar os departamentos
            var departments = _departmentService.FindAll();//para buscar do BD todos os departamentos
            var viewModel = new SellerFormViewModel { Departments = departments }; //aqui é para ele iniciar o departamento com a lista de departamentos
            return View(viewModel);
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

        //mensagem para confirmar a remoção GET
        public IActionResult Delete(int? id)//o ponto de interrogação significa que opcional
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não informado."});//retorna redirecionando para a página de erro passando o argumento de mensagem
            }

            var obj = _sellerService.FindById(id.Value);//como o id é opcional, deve-se colocar o value
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não existe!" });
            }
            return View(obj);
        }
        //DELETE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);//remove o vendedor com o id passado
            return RedirectToAction(nameof(Index));//depois de remover redireciona para a página principal de vendedor
        }

        //DETALHES GET, mesma lógica do GetDelete
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido." });
            }

            var obj = _sellerService.FindById(id.Value);//como o id é opcional, deve-se colocar o value
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não existe!" });
            }
            return View(obj);
        }
        //função de editar GET
        public IActionResult Edit(int? id)
        {//testar se o id é null
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido." });
            }
            //testar se ele existe no db
            var obj = _sellerService.FindById(id.Value);
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não existe!" });
            }
            //abrir a tela de edição carregando os departamentos e povoando a caixa de edição
            List<Department> departments = _departmentService.FindAll();

            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não corresponde." });
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            /*como as duas exceções abaixo pertencem a ApplicationException e possuem a mesma mensagem, pode-se colocar tudo em apenas um catch.
            catch(NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }*/
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        //Ação de erro
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Messege = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

    }
}