using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_Compras___Projeto_LabSW
{
    public class ValorInvalidoException : Exception
    {
       public  ValorInvalidoException(string erro) : base(erro)
        {
        }
    }
}
