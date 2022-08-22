using Lista_de_Compras___Projeto_LabSW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Lista_de_Compras___Projeto_LabSW.Delegates;

namespace TrabalhoLab.Xamarin {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Categorias : ContentPage {
        private App app;
        public Categorias() {
            app = App.Current as App;
            InitializeComponent();
            Title = app.Gestor.ListaAtual.Descricao;
            app.Gestor.ListaAtual.CategoriaAdicionada += atualizarCategoriass;
            app.Gestor.ListaAtual.CategoriaAtualizada += atualizarCategorias;
            app.Gestor.ListaAtual.CategoriaRemovida += atualizarCategorias;
            listview.ItemsSource = app.Gestor.ListaAtual.Categorias;

        }
        protected override void OnAppearing() {
            base.OnAppearing();
            atualizarCategorias();
        }
        private void atualizarCategoriass(Categoria categoria) {
            listview.ItemsSource = null;
            listview.ItemsSource = app.Gestor.ListaAtual.Categorias;
            app.Gestor.SaveListas();


        }
        private void atualizarCategorias() {
            listview.ItemsSource = null;
            listview.ItemsSource = app.Gestor.ListaAtual.Categorias;
            app.Gestor.SaveListas();

        }
        async private void MainListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            int index = e.ItemIndex;
            Categoria categoria = (Categoria)e.Item;
            await Navigation.PushAsync(new Items(categoria));

            ((ListView)sender).SelectedItem = null;


        }

        async private void Button_Clicked(object sender, EventArgs e) {

            string answer = await DisplayPromptAsync("Criar Categoria", "Nome da Categoria");
            if (answer != null)
                try {
                    app.Gestor.ListaAtual.AdicionarCategoria(answer);
                } catch (ValorInvalidoException err) {
                    await DisplayAlert("Erro!", err.Message, "Ok");
                }

        }

        async private void ApagarMenuItem_Clicked(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            var cat = mi.CommandParameter as Categoria;
            var index = (listview.ItemsSource as List<Categoria>).IndexOf(cat);
            if (!cat.Permanente)
                app.Gestor.ListaAtual.ApagarCategoria(index);
            else await DisplayAlert("Erro!", "Não é possível remover categorias permanentes", "Ok");

        }

        async private void RenomearMenuItem_Clicked(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            var cat = mi.CommandParameter as Categoria;
            var index = (listview.ItemsSource as List<Categoria>).IndexOf(cat);
            if (!cat.Permanente) {
                string answer = await DisplayPromptAsync("Renomear Categoria", "Nome da Categoria");
                if (answer != null)
                    try {
                        app.Gestor.ListaAtual.RenomearCategoria(index, answer);
                    } catch (ValorInvalidoException err) {

                        await DisplayAlert("Erro!", err.Message, "Ok");
                    }

            } else await DisplayAlert("Erro!", "Não é possível renomear categorias permanentes", "Ok");

        }


    }
}