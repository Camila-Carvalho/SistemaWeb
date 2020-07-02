using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        //1 ---> registrar o serviço no startup.cs
        //2 ---> declarar dependencia para o context (que faz acesso ao banco de dados)
        private readonly SalesWebMvcContext _context;
        //readonly = somente leitura, então colocando isto este _context não pode ser alterado
        //3 ---> Criar construtor para que a injeção de dependencia possa ocorrer
        public SellerService (SalesWebMvcContext context)
        {
            _context = context;
        }
        //4 ---> implementar o FindAll retornando a lista de vendedores
        public List<Seller> FindAll()
        {//acessa a fonte de dados (_context) na tabela de vendedores (Seller) e retorna convertida para Lista (ToList)
            return _context.Seller.ToList();
        }

        //metodo para inserir um novo vendedor no bd
        public void Insert(Seller obj)
        {
            _context.Add(obj); //aqui ele adiciona o obj (vendedor)
            _context.SaveChanges();//aqui ele salva no banco
        }

    }
}
