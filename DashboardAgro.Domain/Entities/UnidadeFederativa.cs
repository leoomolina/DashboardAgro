using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardAgro.Domain.Entities
{
    public class UnidadeFederativa
    {
        public int Id { get; set; }
        public string SiglaUF { get; set; }
        public string NomeUF { get; set; }
        public Regiao Regiao { get; set; }
    }
}
