using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context;

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //metodo para retornar todos os departamentos
        public async Task<List<Department>> FindAllAsync()
        {//necessário colocar o await para avisar o compilador que esta chamada é assincrona
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }

    }
}
