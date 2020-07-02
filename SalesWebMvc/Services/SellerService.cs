using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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
        //método para encontrar o ID do vendedor
        public Seller FindById(int id)
        {//retorna do banco o seller e também foi acrescendato o include para ele fazer um JOIN com a tabela de departamento
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id); //primeiro encontra o id na tabela seller do bd
            _context.Seller.Remove(obj);//aqui ele remove
            _context.SaveChanges();//aqui ele salva
        }

        //update
        public void Update(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }
            //try foi colocado para caso de algum erro referente ao banco de dados
            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException e){
                throw new DbConcurrencyException(e.Message);
            }
            catch(NotFoundException e)
            {
                throw new NotFoundException(e.Message);
            }
        }
    }
}
