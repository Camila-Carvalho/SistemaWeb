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
        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }
        //4 ---> implementar o FindAll retornando a lista de vendedores
        public async Task<List<Seller>> FindAllAsync()
        {//acessa a fonte de dados (_context) na tabela de vendedores (Seller) e retorna convertida para Lista (ToList)
            return await _context.Seller.ToListAsync();
        }

        //metodo para inserir um novo vendedor no bd
        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj); //aqui ele adiciona o obj (vendedor)
            await _context.SaveChangesAsync();//aqui ele salva no banco
        }
        //método para encontrar o ID do vendedor
        public async Task<Seller> FindByIdAsync(int id)
        {//retorna do banco o seller e também foi acrescendato o include para ele fazer um JOIN com a tabela de departamento
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id); //primeiro encontra o id na tabela seller do bd
            _context.Seller.Remove(obj);//aqui ele remove
            await _context.SaveChangesAsync();//aqui ele salva
        }

        //update
        public async Task UpdateAsync(Seller obj)
        {
            if (!await _context.Seller.AnyAsync(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }
            //try foi colocado para caso de algum erro referente ao banco de dados
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
