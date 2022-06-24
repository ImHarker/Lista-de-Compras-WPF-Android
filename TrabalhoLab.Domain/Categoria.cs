using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lista_de_Compras___Projeto_LabSW.Delegates;

namespace Lista_de_Compras___Projeto_LabSW {
    public class Categoria {
        public event VoidDelegate ItemAdicionado;
        public event VoidDelegate ItemRemovido;
        public Categoria(string nome, bool perm = false) {
            Nome = nome;
            Items = new List<Item>();
            Permanente = perm;
        }

        public string? Nome { get; set; }
        public bool Permanente { get; set; }
        public List<Item>? Items { get; set; }

        public void AddItem(Item item) {
            Items.Add(item);

            if (ItemAdicionado != null) {
                ItemAdicionado();
            }

        }
        public void RemoveItem(int index) {
            Items.RemoveAt(index);
            if (ItemRemovido != null) {
                ItemRemovido();
            }

        }
    }
}
