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
    /// Interaction logic for RenomearCategoria.xaml
    /// </summary>
    public partial class RenomearCategoria : Window {
        private App app;
        public RenomearCategoria() {
            InitializeComponent();
            app = App.Current as App;
            foreach (Categoria c in app.Gestor.ListaAtual.Categorias) {
                if (!c.Permanente)
                    combo_cat.Items.Add(c.Nome);
            }
            if (combo_cat.Items.Count == 0) combo_cat.IsEnabled = false;
            combo_cat.SelectedIndex = 0;
        }

        private void btn_apagar_Click(object sender, RoutedEventArgs e) {
            try {
                app.Gestor.UpdateTimestamp();
                app.Gestor.ListaAtual.RenomearCategoria(combo_cat.SelectedIndex + app.Gestor.ListaAtual.CategoriasPermanentes, txtbx_nome.Text);
            } catch (ValorInvalidoException err) {
                MessageBox.Show(err.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }
    }
}
