using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Lista_de_Compras___Projeto_LabSW.Delegates;

namespace Lista_de_Compras___Projeto_LabSW {
    public class GestorListas {
        public event ListaDelegate ListaNova;
        public event VoidDelegate ListaAtualizada;
        public event VoidDelegate ListaRemovida;
        public List<Lista>? GestorList { get; set; }
        public double Timestamp { get; set; }
        public Lista ListaAtual { get; set; }

        public GestorListas() {
            GestorList = new List<Lista>();
        }

        public void AdicionarLista(string nome) {
            Lista lista = new Lista();
            if (!String.IsNullOrEmpty(nome) && nome.Length <= 25) {
                lista.Descricao = nome;
                GestorList.Add(lista);
                if (ListaNova != null) {
                    ListaNova(lista);
                }
            } else throw new ValorInvalidoException("Por favor introduza um nome válido.");
        }
        public void RenomearLista(int index, string desc) {
            if (!String.IsNullOrEmpty(desc) && desc.Length <= 30) {
                GestorList[index].Descricao = desc;
                if (ListaAtualizada != null) {
                    ListaAtualizada();
                }
            } else throw new ValorInvalidoException("Por favor introduza um nome válido.");
        }
        public void ApagarLista(int index) {
            GestorList.RemoveAt(index);
            if (ListaRemovida != null) {
                ListaRemovida();
            }
        }

        public void SaveListas() {
            XDocument doc = new XDocument();
            doc.Add(new XElement("ListasDeCompras"));
            doc.Element("ListasDeCompras").Add(new XAttribute("timestamp", Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)));
            doc.Element("ListasDeCompras").Add(new XElement("Listas"));
            XElement listas = doc.Root.Element("Listas");
            foreach (Lista lista in GestorList) {
                XElement list = new XElement("Lista");
                list.Add(new XAttribute("Descricao", lista.Descricao));
                foreach (Categoria catg in lista.Categorias) {
                    XElement categoria = new XElement("Categoria");
                    categoria.Add(new XAttribute("Nome", catg.Nome));
                    categoria.Add(new XAttribute("Permanente", catg.Permanente));
                    foreach (Item item in catg.Items) {
                        XElement produto = new XElement("Produto");
                        produto.Add(new XAttribute("Descricao", item.Descricao));
                        produto.Add(new XAttribute("Quantidade", item.Qtd));
                        produto.Add(new XAttribute("Comprado", item.Comprado));
                        categoria.Add(produto);
                    }
                    list.Add(categoria);
                }
                listas.Add(list);
            }
            doc.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Listas.xml"));

        }
        public void LoadListas() {
            XDocument doc = XDocument.Load(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Listas.xml"));
            Timestamp = Convert.ToDouble(doc.Element("ListasDeCompras").Attribute("timestamp").Value);
            XElement listas = doc.Root.Element("Listas");
            foreach (XElement lista in listas.Elements("Lista")) {
                Lista list = new Lista();
                list.Categorias.Clear();
                list.Descricao = lista.Attribute("Descricao").Value;
                foreach (XElement categoria in lista.Elements("Categoria")) {
                    Categoria catg = new Categoria(categoria.Attribute("Nome").Value, bool.Parse(categoria.Attribute("Permanente").Value));
                    foreach (XElement produto in categoria.Elements("Produto")) {
                        Item item = new Item();
                        item.Descricao = produto.Attribute("Descricao").Value;
                        item.Qtd = produto.Attribute("Quantidade").Value;
                        item.Comprado = bool.Parse(produto.Attribute("Comprado").Value);
                        catg.Items.Add(item);
                    }
                    list.Categorias.Add(catg);
                }
                GestorList.Add(list);

                if (ListaAtualizada != null) {
                    ListaAtualizada();
                }
            }
        }

        public void UpdateListaRequest(string user) {
            while (GestorList.Count != 0) {
                ApagarLista(0);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://xxx.xxx.xxx.xxx/lista?username=" + user);
            request.Method = "GET";
            request.ContentType = "application/json";
            try {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string resposta = reader.ReadToEnd();
                XDocument doc = XDocument.Parse(resposta);
                Timestamp = Convert.ToDouble(doc.Element("ListasDeCompras").Attribute("timestamp").Value); // verificar timestamps com servidor
                XElement listas = doc.Root.Element("Listas");
                foreach (XElement lista in listas.Elements("Lista")) {
                    Lista list = new Lista();
                    list.Categorias.Clear();
                    list.Descricao = lista.Attribute("Descricao").Value;
                    foreach (XElement categoria in lista.Elements("Categoria")) {
                        Categoria catg = new Categoria(categoria.Attribute("Nome").Value, bool.Parse(categoria.Attribute("Permanente").Value));
                        foreach (XElement produto in categoria.Elements("Produto")) {
                            Item item = new Item();
                            item.Descricao = produto.Attribute("Descricao").Value;
                            item.Qtd = produto.Attribute("Quantidade").Value;
                            item.Comprado = bool.Parse(produto.Attribute("Comprado").Value);
                            catg.Items.Add(item);
                        }
                        list.Categorias.Add(catg);
                    }
                    GestorList.Add(list);
                    if (ListaAtualizada != null) {
                        ListaAtualizada();
                    }
                }
                SaveListas();
            } catch (System.Net.WebException e) {
                throw new ValorInvalidoException("Não foi possível conectar ao servidor. Verifique a conexão ou tente mais tarde.");
            }
        }
    }
}
