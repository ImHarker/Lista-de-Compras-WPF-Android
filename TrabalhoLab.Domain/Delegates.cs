using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_Compras___Projeto_LabSW {
    public class Delegates {
        public delegate void ListaDelegate(Lista lista);
        public delegate void CategoriaDelegate(Categoria categoria);
        public delegate void ItemDelegate(Item item);
        public delegate void VoidDelegate();
    }
}
