using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService (SalesWebMvcContext contex)
        {
            _context = contex;
        }

        //operação para encontrar as vendas no intervalo de datas passados no parametro
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; //aqui ele atribui para o result todas as vendas
            if (minDate.HasValue) //se a data minima possui um valor
            {
                result = result.Where(x => x.Date >= minDate.Value);//result passa a ter as vendas da data minima pra frente
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);//result passa a ter as vendas da data minima pra frente e da data máxima para atrás
            }
            return await result.Include(x => x.Seller).Include(x => x.Seller.Department).OrderByDescending(x => x.Date).ToListAsync(); //retorna o resultado da pesquisa incluindo o vendedor, departamento do vendedor e ordenado por data
        }

        //como está agrupoando no return, necessário informar para listar po IGouping passando os "parametros" de agrupamento por ordem, ou seja, primeiro por Departamento e depois pelas vendas
        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj; //aqui ele atribui para o result todas as vendas
            if (minDate.HasValue) //se a data minima possui um valor
            {
                result = result.Where(x => x.Date >= minDate.Value);//result passa a ter as vendas da data minima pra frente
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);//result passa a ter as vendas da data minima pra frente e da data máxima para atrás
            }
            return await result.Include(x => x.Seller).Include(x => x.Seller.Department).OrderByDescending(x => x.Date).GroupBy(x => x.Seller.Department).ToListAsync(); //retorna o resultado da pesquisa incluindo o vendedor, departamento do vendedor e ordenado por data
        }

    }
}
