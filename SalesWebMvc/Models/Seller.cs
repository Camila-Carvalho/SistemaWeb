using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} obrigatório")]//tornar obrigatorio
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O tamanho do {0} precisa ser entre {2} e {1}")] //definir o tamanho minimo e maximo do campo
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]//para deixar o e-mail como um "link"
        [Required(ErrorMessage = "{0} obrigatório")]
        [EmailAddress(ErrorMessage = "Entre com um e-mail válido")]
        public string Email { get; set; }

        [Display(Name = "Birth Date")]//para aparecer um nome diferente no display
        [Required(ErrorMessage = "{0} obrigatório")]
        [DataType(DataType.Date)]//para definir para aparecer só a data, sem o horário
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]//para a data aparecer no formato de dia/mês/ano
        public DateTime BirthDate { get; set; }

        [Display(Name = "Base Salary")]
        [Required(ErrorMessage = "{0} obrigatório")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} precisa ser entre {1} e {2}")]
        [DisplayFormat(DataFormatString = "{0:F2}")]//para formatar e deixar o salário com duas casas decimais
        public double BaseSalary { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sale)
        {
            Sales.Add(sale);
        }

        public void RemoveSales(SalesRecord sale)
        {
            Sales.Remove(sale);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {   //retorna a lista de vendas desta classe quando forem neste intervalo de datas e soma todos os totais de venda
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
