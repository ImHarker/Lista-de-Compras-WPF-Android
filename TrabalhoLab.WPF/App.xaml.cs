using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Lista_de_Compras___Projeto_LabSW {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public GestorListas Gestor { get; set; }

        public App() {
            Gestor = new GestorListas();
        }

    }
}
