using Lista_de_Compras___Projeto_LabSW;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrabalhoLab.Xamarin
{
    public partial class App : Application
    {
        public GestorListas Gestor { get; set; }
        public App()
        {
            Gestor = new GestorListas();
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
