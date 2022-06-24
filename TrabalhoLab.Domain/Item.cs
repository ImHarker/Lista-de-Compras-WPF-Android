using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_Compras___Projeto_LabSW {
    public class Item {
        public bool Comprado { get; set; }
        public string? Descricao { get; set; }
        public string? Qtd { get; set; }

        public void ValidaDados() { 
        if(String.IsNullOrEmpty(Descricao) || String.IsNullOrEmpty(Qtd) || Descricao.Length > 20 || Qtd.Length > 5) throw new ValorInvalidoException("Por favor preencha os campos devidamente");
        }
    }
}
