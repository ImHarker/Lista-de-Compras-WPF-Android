using Lista_de_Compras___Projeto_LabSW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace TrabalhoLab.Xamarin {
    public partial class MainPage : ContentPage {
        private App app;
        public MainPage() {
            app = App.Current as App;
            InitializeComponent();
            try {
                app.Gestor.LoadListas();
            } catch (System.IO.FileNotFoundException e) {
            }


            app.Gestor.ListaNova += atualizaListaa;
            app.Gestor.ListaAtualizada += atualizaLista;
            app.Gestor.ListaRemovida += atualizaLista;
            listview.ItemsSource = app.Gestor.GestorList;

        }
        private void atualizaListaa(Lista lista) {
            listview.ItemsSource = null;
            listview.ItemsSource = app.Gestor.GestorList;
            app.Gestor.UpdateTimestamp();
            app.Gestor.SaveListas();

        }
        private void atualizaLista() {
            listview.ItemsSource = null;
            listview.ItemsSource = app.Gestor.GestorList;
            app.Gestor.UpdateTimestamp();
            app.Gestor.SaveListas();
        }
        async private void MainListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            int index = e.ItemIndex;
            app.Gestor.ListaAtual = app.Gestor.GestorList[index];
            await Navigation.PushAsync(new Categorias());

            ((ListView)sender).SelectedItem = null;


        }

        async private void Button_Clicked(object sender, EventArgs e) {

            string answer = await DisplayPromptAsync("Criar Lista", "Nome da Lista");
            if (answer != null)
                try {
                    app.Gestor.AdicionarLista(answer);
                } catch (ValorInvalidoException err) {
                    await DisplayAlert("Erro!", err.Message, "Ok");
                }

        }

        private void ApagarMenuItem_Clicked(object sender, EventArgs e) {
            var mi = ((MenuItem)sender);
            var list = mi.CommandParameter as Lista;
            var index = (listview.ItemsSource as List<Lista>).IndexOf(list);
            app.Gestor.ApagarLista(index);

        }

        async private void RenomearMenuItem_Clicked(object sender, EventArgs e) {
            string answer = await DisplayPromptAsync("Renomear Lista", "Nome da Lista");
            var mi = ((MenuItem)sender);
            var list = mi.CommandParameter as Lista;
            var index = (listview.ItemsSource as List<Lista>).IndexOf(list);
            if (answer != null)
                try {
                    app.Gestor.RenomearLista(index, answer);
                } catch (ValorInvalidoException err) {

                    await DisplayAlert("Erro!", err.Message, "Ok");
                }

        }

        async private void Settings_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new Settings());
        }

        async private void Refresh_Clicked(object sender, EventArgs e) {
            try {
                XDocument doc = app.Gestor.UpdateListaGETRequest("admin");
                XDocument local = new XDocument(app.Gestor.ListaToXML());
                XDocument server = new XDocument(doc);
                local.Element("ListasDeCompras").Attribute("timestamp").Remove();
                server.Element("ListasDeCompras").Attribute("timestamp").Remove();
                if (local.ToString().GetHashCode() == server.ToString().GetHashCode())
                    await DisplayAlert("Informação!", "A lista já se encontra atualizada!", "OK");
                else {
                    if (app.Gestor.Timestamp > app.Gestor.GetTimestamp(doc)) {
                        var a1 = await DisplayAlert("Atenção!", "O servidor tem uma versão de uma data anterior! Deseja, mesmo assim, atualizar a lista?", "Yes", "No");
                        if (a1) {
                            app.Gestor.UpdateLista(doc);
                            return;
                        }
                        var a2 = await DisplayAlert("Atenção!", "Deseja atualizar a lista do servidor?", "Yes", "No");
                        if (a2) {
                            app.Gestor.UpdateListaPOSTRequest("admin");
                            return;
                        }
                    }
                    if (app.Gestor.Timestamp < app.Gestor.GetTimestamp(doc)) {
                        var a3 = await DisplayAlert("Atenção!", "O servidor tem uma versão mais recente! Deseja atualizar a lista?", "Yes", "No");
                        if (a3) {
                            app.Gestor.UpdateLista(doc);
                            return;
                        }
                        var a4 = await DisplayAlert("Atenção!", "Deseja sobrepor a lista do servidor?", "Yes", "No");
                        if (a4) {
                            app.Gestor.UpdateListaPOSTRequest("admin");
                        }
                    }
                }

            } catch (ValorInvalidoException err) {
                await DisplayAlert(err.Message, "Erro!", "Ok");
            }
        }
    }
}