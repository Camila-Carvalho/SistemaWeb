using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>(); //aqui declaramos uma coleção do tipo vendedores e instanciamos com uma lista de vendedores

        public Department() { }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {//retorna a soma da lista de vendedores, para a soma foi passado o método que soma as vendas de cada vendedor na classe vendedor
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
