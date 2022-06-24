using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lista_de_Compras___Projeto_LabSW {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private App app;
        public MainWindow() {
            app = App.Current as App;
            InitializeComponent();
            app.Gestor.ListaNova += novaLista;
            app.Gestor.ListaAtualizada += atualizarLista;
            app.Gestor.ListaRemovida += atualizarLista;
            try {
                app.Gestor.LoadListas();
            } catch (System.IO.FileNotFoundException e) {
            }
        }
        private void atualizarLista() {
            lstbx_listas.Items.Clear();
            foreach (Lista l in app.Gestor.GestorList) {
                lstbx_listas.Items.Add(l.Descricao);
            }
        }
        private void novaLista(Lista lista) {
            lstbx_listas.Items.Add(lista.Descricao);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e) {
            ListNameDialog listNameDialog = new ListNameDialog();
            if (listNameDialog.ShowDialog() == true)
                try
                {
                    app.Gestor.AdicionarLista(listNameDialog.txtbx_title.Text);
                }
                catch (ValorInvalidoException err){

                    MessageBox.Show(err.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        private void btn_alt_Click(object sender, RoutedEventArgs e) {
            if (!lstbx_listas.Items.IsEmpty) {
                if (lstbx_listas.SelectedIndex != -1) {
            ListNameDialog listNameDialog = new ListNameDialog();
                    listNameDialog.btn_adi.Content = "Alterar";
                    listNameDialog.txtbx_title.Text = lstbx_listas.SelectedItem.ToString();
                    if (listNameDialog.ShowDialog() == true)
                        try
                        {
                            app.Gestor.RenomearLista(lstbx_listas.SelectedIndex, listNameDialog.txtbx_title.Text);
                        }
                        catch (ValorInvalidoException err)
                        {

                            MessageBox.Show(err.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }
                else
                    MessageBox.Show("Por favor selecione uma Lista", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Ainda não criou nenhuma Lista.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void btn_del_Click(object sender, RoutedEventArgs e) {
            if (!lstbx_listas.Items.IsEmpty) {
                if (lstbx_listas.SelectedIndex != -1) {
                    app.Gestor.ApagarLista(lstbx_listas.SelectedIndex);
                }
                else
                    MessageBox.Show("Por favor selecione uma Lista", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Ainda não criou nenhuma Lista.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void lstbx_listas_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (lstbx_listas.SelectedIndex != -1) {
                app.Gestor.ListaAtual = app.Gestor.GestorList[lstbx_listas.SelectedIndex];
                ListaWin lista = new ListaWin();
                App.Current.MainWindow.Hide();
                lista.txtblck_Lista.Text = lstbx_listas.SelectedItem.ToString();
                lista.Title = lstbx_listas.SelectedItem.ToString();
                lista.Show();
            }
        }

        private void ClickLogin(object sender, RoutedEventArgs e) {
            LoginRegistarWindow loginRegistarWindow = new LoginRegistarWindow();
            loginRegistarWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            app.Gestor.SaveListas();
        }
    }

}
