﻿using System;
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
    /// Interaction logic for ListNameDialog.xaml
    /// </summary>
    public partial class ListNameDialog : Window {
        private App app;
        public ListNameDialog() {
            InitializeComponent();
            app = App.Current as App;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            app.Gestor.UpdateTimestamp();
            this.Close();

        }


    }
}
