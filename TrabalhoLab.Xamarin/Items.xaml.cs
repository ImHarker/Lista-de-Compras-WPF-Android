using Lista_de_Compras___Projeto_LabSW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrabalhoLab.Xamarin {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Items : ContentPage {
        private App app;
        private Categoria cat;
        public Items(Categoria catg) {
            InitializeComponent();
            this.cat = catg;
            Title = cat.Nome;
            app = App.Current as App;
            listview.ItemsSource = cat.Items;

            cat.ItemAdicionado += atualizarItems;
            cat.ItemRemovido += atualizarItems;

        }

        private void atualizarItems() {
            listview.ItemsSource = null;
            listview.ItemsSource = cat.Items;
            app.Gestor.UpdateTimestamp();
            app.Gestor.SaveListas();


        }

        private void MainListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            ((ListView)sender).SelectedItem = null;


        }
        async private void Button_Clicked(object sender, EventArgs e) {
            Item item = new Item();
            string answer = await DisplayPromptAsync("Criar Item", "Quantidade do Item");
            if (answer != null) {
                item.Qtd = answer;
                answer = await DisplayPromptAsync("Criar Item", "Nome do Item");
                if (answer != null) {
                    item.Descricao = answer;
                    try {
                        item.ValidaDados();
                        cat.AddItem(item);
                    } catch (ValorInvalidoException err) {
                        await DisplayAlert("Erro!", err.Message, "Ok");
                    }
                }
            }
        }
        async private void AlteraCategoriaMenuItem_Clicked(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as Item;
            var index = (listview.ItemsSource as List<Item>).IndexOf(item);
            List<string> cats = new List<string>();
            foreach (Categoria cat in app.Gestor.ListaAtual.Categorias)
                cats.Add(cat.Nome);
            string[] catgs = cats.ToArray();
            string answer = await DisplayActionSheet("Alterar Categoria", "Cancel", null, catgs);
            if (answer != "Cancel") {
                cat.RemoveItem(index);
                app.Gestor.ListaAtual.Categorias.Find(x => x.Nome.Equals(answer)).AddItem(item);
            }

        }
        private void ApagarMenuItem_Clicked(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as Item;
            var index = (listview.ItemsSource as List<Item>).IndexOf(item);
            cat.RemoveItem(index);

        }

        async private void RenomearMenuItem_Clicked(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            var item = mi.CommandParameter as Item;
            var index = (listview.ItemsSource as List<Item>).IndexOf(item);

            Item nitem = new Item();
            string answer = await DisplayPromptAsync("Alterar Item", "Quantidade do Item");
            if (answer != null) {
                nitem.Qtd = answer;
                answer = await DisplayPromptAsync("Alterar Item", "Nome do Item");
                if (answer != null) {
                    nitem.Descricao = answer;
                    try {
                        nitem.Comprado = item.Comprado;
                        nitem.ValidaDados();
                        cat.RemoveItem(index);
                        cat.AddItem(nitem);
                    } catch (ValorInvalidoException err) {

                        await DisplayAlert("Erro!", err.Message, "Ok");
                    }
                }
            }


        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e) {
            app.Gestor.UpdateTimestamp();
            app.Gestor.SaveListas();

        }
    }
}