using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lista_de_Compras___Projeto_LabSW.Delegates;

namespace Lista_de_Compras___Projeto_LabSW {
    public class Lista {

        public string? Descricao { get; set; }
        public List<Categoria>? Categorias { get; set; }
        public int CategoriasPermanentes { get; set; }

        public event CategoriaDelegate CategoriaAdicionada;
        public event VoidDelegate CategoriaAtualizada;
        public event VoidDelegate CategoriaRemovida;

        public Lista() {
            Categorias = new List<Categoria>();
            Categorias.Add(new Categoria("Sem Categoria", true));
            Categorias.Add(new Categoria("Padaria", true));
            Categorias.Add(new Categoria("Bazar", true));
            Categorias.Add(new Categoria("Mercearia", true));
            Categorias.Add(new Categoria("Frios e Laticínios", true));
            Categorias.Add(new Categoria("Limpeza", true));
            Categorias.Add(new Categoria("Ferragens", true));
            Categorias.Add(new Categoria("Bricolage", true));
            Categorias.Add(new Categoria("Automóveis", true));
            CategoriasPermanentes = Categorias.Count;
        }

        public void AdicionarCategoria(string nome)
        {
            if (!String.IsNullOrEmpty(nome) && nome.Length <= 20)
            {
                if (Categorias.Find(x => x.Nome.Equals(nome)) == null)
                {
                    Categoria cat = new Categoria(nome);
                    Categorias.Add(cat);
                    if (CategoriaAdicionada != null)
                    {
                        CategoriaAdicionada(cat);
                    }
                    return;
                }
            }
            throw new ValorInvalidoException("Por favor introduza um nome válido.");
        }

        public void ApagarCategoria(int selectedIndex)
        {
            if (Categorias[selectedIndex].Items.Count > 0)
            {
                foreach (Item i in Categorias[selectedIndex].Items)
                {
                    Categorias[0].Items.Add(i);
                }
            }
            Categorias.RemoveAt(selectedIndex);
            if (CategoriaRemovida != null)
            {
                CategoriaRemovida();
            }
        }
        public void RenomearCategoria(int selectedIndex, string nome)
        {
            if (!String.IsNullOrEmpty(nome) && nome.Length <= 20)
            {
                if (Categorias.Find(x => x.Nome.Equals(nome)) == null)
                {
                    Categorias[selectedIndex].Nome = nome;
                    if (CategoriaAtualizada != null)
                    {
                        CategoriaAtualizada();
                    }
                    return;
                }
            }
            throw new ValorInvalidoException("Por favor introduza um nome válido.");
        }
    }
}