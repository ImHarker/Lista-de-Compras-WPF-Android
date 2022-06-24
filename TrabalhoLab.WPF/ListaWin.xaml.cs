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
using System.Windows.Shapes;

namespace Lista_de_Compras___Projeto_LabSW {
    /// <summary>
    /// Interaction logic for Lista.xaml
    /// </summary>
    public partial class ListaWin : Window {
        private App app;

        List<ListBox> categorias = new List<ListBox>();
        int hcount = -1;


        public ListaWin() {
            InitializeComponent();
            app = App.Current as App;
            btn_adicionar.Content = "Adicionar\nCategoria";
            btn_renomear.Content = "Renomear\nCategoria";
            btn_apagar.Content = " Eliminar\nCategoria";
            btn_adicionar.HorizontalContentAlignment = HorizontalAlignment.Center;
            btn_adicionar.VerticalContentAlignment = VerticalAlignment.Center;
            btn_renomear.HorizontalContentAlignment = HorizontalAlignment.Center;
            btn_renomear.VerticalContentAlignment = VerticalAlignment.Center;
            btn_apagar.HorizontalContentAlignment = HorizontalAlignment.Center;
            btn_apagar.VerticalContentAlignment = VerticalAlignment.Center;
            app.Gestor.ListaAtual.CategoriaAdicionada += AdicionarCategoria;
            app.Gestor.ListaAtual.CategoriaAtualizada += AtualizarCategoria;
            app.Gestor.ListaAtual.CategoriaRemovida += AtualizarCategoria;
        }
        private void AtualizarCategoria() {
            foreach (Categoria c in app.Gestor.ListaAtual.Categorias) {
                c.ItemAdicionado -= AtualizarCategoria;
                c.ItemRemovido -= AtualizarCategoria;
            }
            categorias.Clear();
            hcount = -1;
            grid.Children.Clear();

            foreach (Categoria c in app.Gestor.ListaAtual.Categorias) {
                LoadCategoria(c);
            }

        }
        private void AdicionarCategoria(Categoria cat) {
            LoadCategoria(cat);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            App.Current.MainWindow.Show();
        }

        private void btn_adicionar_Click(object sender, RoutedEventArgs e) {
            ListNameDialog listNameDialog = new ListNameDialog();
            listNameDialog.btn_adi.Content = "Adicionar";
            listNameDialog.txtbx_title.Text = "Categoria";
            if (listNameDialog.ShowDialog() == true) {
                try
                {
                    app.Gestor.ListaAtual.AdicionarCategoria(listNameDialog.txtbx_title.Text);
                }
                catch (ValorInvalidoException err)
                {
                    MessageBox.Show(err.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClickAlterar(object sender, EventArgs e) {
            for (int i = 0; i < categorias.Count; i++) {
                int index = categorias[i].SelectedIndex;
                if (index != -1) {
                    Item it = app.Gestor.ListaAtual.Categorias[i].Items[index];
                    ItemWindow itemWindow = new ItemWindow();
                    itemWindow.combobx_cat.SelectedIndex = i;
                    itemWindow.txtbx_nome.Text = it.Descricao;
                    itemWindow.txtbx_qtd.Text = it.Qtd;
                    foreach (Categoria c in app.Gestor.ListaAtual.Categorias)
                        itemWindow.combobx_cat.Items.Add(c.Nome);
                        if (itemWindow.ShowDialog() == true)
                        {
                            Item item = new Item();
                            item.Qtd = itemWindow.txtbx_qtd.Text;
                            item.Descricao = itemWindow.txtbx_nome.Text;
                    try
                    {
                            item.ValidaDados();
                            app.Gestor.ListaAtual.Categorias[i].RemoveItem(index);
                            app.Gestor.ListaAtual.Categorias[itemWindow.combobx_cat.SelectedIndex].AddItem(item);
                    }catch(ValorInvalidoException err)
                    {
                        MessageBox.Show(err.Message , "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                        }

                }
            }


        }
        private void ClickApagar(object sender, EventArgs e) {
            for (int i = 0; i < categorias.Count; i++) {
                int index = categorias[i].SelectedIndex;
                if (index != -1) {
                    app.Gestor.ListaAtual.Categorias[i].RemoveItem(index);
                }
            }
        }

        private void lstbx_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            ListBox lista = (ListBox)sender;
            for (int i = 0; i < categorias.Count; i++) {
                if (categorias[i] == lista) {
                    ItemWindow itemWindow = new ItemWindow();
                    itemWindow.combobx_cat.SelectedIndex = i;
                    foreach (Categoria c in app.Gestor.ListaAtual.Categorias)
                        itemWindow.combobx_cat.Items.Add(c.Nome);
                    if (itemWindow.ShowDialog() == true) {
                        Item item = new Item();
                        item.Qtd = itemWindow.txtbx_qtd.Text;
                        item.Descricao = itemWindow.txtbx_nome.Text;
                        try
                        {
                            item.ValidaDados();
                            app.Gestor.ListaAtual.Categorias[itemWindow.combobx_cat.SelectedIndex].AddItem(item);
                        }
                        catch (ValorInvalidoException err)
                        {
                            MessageBox.Show(err.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }


                }
            }

        }

        private void btn_apagar_Click(object sender, RoutedEventArgs e) {
            if (app.Gestor.ListaAtual.Categorias.Count > app.Gestor.ListaAtual.CategoriasPermanentes)
            {
                ApagarCategoria apagarCategoria = new ApagarCategoria();
                apagarCategoria.Show();
            }
            else
                MessageBox.Show("Ainda não criou nenhuma categoria.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
           
            }
        

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            for (int i = 0; i < app.Gestor.ListaAtual.Categorias.Count; i++)
                LoadCategoria(app.Gestor.ListaAtual.Categorias[i]);

        }

        private void LoadCategoria(Categoria cat) {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuApagar = new MenuItem();
            MenuItem menuAlterar = new MenuItem();

            menuAlterar.Header = "Alterar";
            menuApagar.Header = "Apagar";
            menuAlterar.Click += new RoutedEventHandler(ClickAlterar);
            menuApagar.Click += new RoutedEventHandler(ClickApagar);
            contextMenu.Items.Add(menuAlterar);
            contextMenu.Items.Add(menuApagar);

            ListBox list = new ListBox();
            Label categorialbl = new Label();
            int i = categorias.Count % 5;
            if (i == 0) hcount++;
            categorialbl.Width = 200;
            categorialbl.FontSize = 20;
            categorialbl.Content = cat.Nome;
            categorialbl.HorizontalAlignment = HorizontalAlignment.Left;
            categorialbl.VerticalAlignment = VerticalAlignment.Top;
            categorialbl.Margin = new Thickness(46 * (i + 1) + i * 200, (hcount + 1) * 100 + hcount * 300 - 50, 0, 0);
            categorialbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            list.HorizontalAlignment = HorizontalAlignment.Left;
            list.VerticalAlignment = VerticalAlignment.Top;
            list.Margin = new Thickness(46 * (i + 1) + i * 200, (hcount + 1) * 100 + hcount * 300, 0, 0);
            list.Width = 200;
            list.Height = 300;
            list.MouseDoubleClick += new MouseButtonEventHandler(lstbx_MouseDoubleClick);
            list.ContextMenu = contextMenu;
            categorias.Add(list);
            grid.Children.Add(list);
            grid.Children.Add(categorialbl);
            scroll_view.Content = grid;
            cat.ItemAdicionado += AtualizarCategoria;
            cat.ItemRemovido += AtualizarCategoria;

            foreach (Item item in cat.Items) {
                CheckBox? chk = new CheckBox();
                if (chk != null) {
                    chk.Content = item.Qtd + " de " + item.Descricao;
                    chk.IsChecked = item.Comprado;
                    chk.FontSize = 18;
                    chk.Height = 25;
                    chk.VerticalContentAlignment = VerticalAlignment.Center;
                    chk.Checked += ChangeCompradoStatus;
                    chk.Unchecked += ChangeCompradoStatus;
                    list.Items.Add(chk);
                }
                list.SelectedIndex = -1;
            }
        }

        private void ChangeCompradoStatus(object sender, RoutedEventArgs e) {
            CheckBox chk = (CheckBox)sender;
            for (int i = 0; i < categorias.Count; i++)
                for (int j = 0; j < categorias[i].Items.Count; j++) {
                    if (categorias[i].Items[j] == sender) {
                        app.Gestor.ListaAtual.Categorias[i].Items[j].Comprado = (bool)chk.IsChecked;
                    }
                }
        }

        private void btn_renomear_Click(object sender, RoutedEventArgs e) {
            if (app.Gestor.ListaAtual.Categorias.Count > app.Gestor.ListaAtual.CategoriasPermanentes)
            {
                RenomearCategoria renomear = new RenomearCategoria();
                renomear.Show();
            }
            else
                MessageBox.Show("Ainda não criou nenhuma categoria.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
