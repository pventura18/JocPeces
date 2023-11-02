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

namespace JocPeces
{
    /// <summary>
    /// Lógica de interacción para WPFPausa.xaml
    /// </summary>
    public partial class WPFPausa : Window
    {
        public WPFPausa()
        {
            InitializeComponent();
        }

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult=true;
        }

        private void btnPantallaPrincipal_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this.Owner).Close();
            
            this.Close();
        }
    }
}
