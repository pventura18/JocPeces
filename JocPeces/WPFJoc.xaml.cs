using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace JocPeces
{
    /// <summary>
    /// Lógica de interacción para WPFJoc.xaml
    /// </summary>
    public partial class WPFJoc : Window
    {
        #region Atributs

        private int nFiles;
        private int nColumnes;
        private int numCorrectes;
        private double progres;
        Fitxa[,] matriuFitxes;
        Fitxa buida;
        DispatcherTimer rellotge;
        TimeSpan tempsTranscorregut;

        #endregion

        #region Constructor

        public WPFJoc(int nFiles, int nColumnes)
        {
            InitializeComponent();
            IniciarRellotge();
            this.nFiles = nFiles;
            this.nColumnes = nColumnes;
            this.matriuFitxes = new Fitxa[nFiles,nColumnes];
            this.numCorrectes = 0;

            CreaGraella();
        }

        #endregion

        #region Graella

        private void CreaGraella()
        {
           
            gridJoc.ShowGridLines = true;

            gridJoc.CreaFiles(nFiles);
            gridJoc.CreaColumnes(nColumnes);

            PosaFitxes(ArrayRandom());

            
        }

        #endregion

        #region Generació de posicions random

        private int[] ArrayRandom()
        {
            int n = nFiles * nColumnes;
            int[] array = new int[n-1];
            for (int i = 0; i < array.Length; i++) array[i] = i + 1;

            Random r = new Random();

            for(int i = 0; i < array.Length; i++)
            {
                int j = r.Next(0, array.Length);

                int aux = array[i];
                array[i] = array[j];
                array[j] = aux;
            }

            if(!EsPotResoldre(array)) array = ArrayRandom();
            return array;
        }

        private bool EsPotResoldre(int[] array)
        {
            
            int cont = 0;

            for(int i = 0; i < array.Length; i++)
            {
                for(int j = i+1; j < array.Length; j++)
                {
                    if (array[i] > array[j]) cont++;
                }
            }

            return cont % 2 == 0;
        }

        #endregion

        #region Posar Botons

        private void PosaFitxes(int[] posicions)
        {
            int n = 0;

            for (int i = 0; i < nFiles; i++)
            {
                for (int j = 0; j < nColumnes; j++)
                {
                    Fitxa fitxa = new Fitxa();

                    fitxa.Click += Fitxa_Click;

                    fitxa.NumActual = n + 1;
                    if((nFiles*nColumnes) != n + 1)
                    {
                        fitxa.NumObjectiu = posicions[n];
                        fitxa.Content = posicions[n];
                    }
                    else
                    {
                        fitxa.NumObjectiu = -1;
                        fitxa.Content = -1;
                    }
                    

                    PintaFitxa(fitxa);
                    

                    fitxa.posFila = i;
                    fitxa.posColumna = j;

                    fitxa.SetValue(Grid.RowProperty, i);
                    fitxa.SetValue(Grid.ColumnProperty, j);

                    gridJoc.Children.Add(fitxa);

                    matriuFitxes[i,j] = fitxa;

                    n++;

                }
            }

            buida = matriuFitxes[matriuFitxes.GetLength(0)-1, matriuFitxes.GetLength(1) - 1];
            buida.Visibility = Visibility.Collapsed;
            numCorrectes = Puntua();
            sbiNCorrectes.Content = numCorrectes;
            sbiPercentatgeCompletat.Content = Math.Round((double)numCorrectes / (nFiles * nColumnes - 1) * 100, 2) + " %";

        }

        #region Pintar

        private void PintaFitxa(Fitxa fitxa)
        {
            if (fitxa.NumActual == fitxa.NumObjectiu)
            {
                fitxa.Background = Brushes.Green;
            }

            else
            {
                fitxa.Background = Brushes.DarkRed;
            }
                
        }
        #endregion

        #endregion

        #region Click
        private void Fitxa_Click(object sender, RoutedEventArgs e)
        {

            Fitxa fitxa = (Fitxa)sender;
            if(buida.posFila == fitxa.posFila)
            {
                if(buida.posColumna > fitxa.posColumna)
                {
                    for(int i = buida.posColumna; i > fitxa.posColumna; i--)
                    {
                        matriuFitxes[buida.posFila, i].Content = matriuFitxes[buida.posFila, i-1].Content;
                        matriuFitxes[buida.posFila, i].NumObjectiu = matriuFitxes[buida.posFila, i-1].NumObjectiu;
                        PintaFitxa(matriuFitxes[buida.posFila, i]);
                        buida.Visibility = Visibility.Visible;
                        buida = fitxa;
                        buida.Visibility = Visibility.Collapsed;
                    }
                }
                else if (buida.posColumna < fitxa.posColumna)
                {
                    for (int i = buida.posColumna; i < fitxa.posColumna; i++)
                    {
                        matriuFitxes[buida.posFila, i].Content = matriuFitxes[buida.posFila, i + 1].Content;
                        matriuFitxes[buida.posFila, i].NumObjectiu = matriuFitxes[buida.posFila, i + 1].NumObjectiu;
                        PintaFitxa(matriuFitxes[buida.posFila, i]);
                        buida.Visibility = Visibility.Visible;
                        buida = fitxa;
                        buida.Visibility = Visibility.Collapsed;
                    }
                }
                
            }
            else if(buida.posColumna == fitxa.posColumna)
            {
                if (buida.posFila > fitxa.posFila)
                {
                    for (int i = buida.posFila; i > fitxa.posFila; i--)
                    {
                        matriuFitxes[i, buida.posColumna].Content = matriuFitxes[i - 1, buida.posColumna].Content;
                        matriuFitxes[i, buida.posColumna].NumObjectiu = matriuFitxes[i - 1, buida.posColumna].NumObjectiu;
                        PintaFitxa(matriuFitxes[i, buida.posColumna]);
                        buida.Visibility = Visibility.Visible;
                        buida = fitxa;
                        buida.Visibility = Visibility.Collapsed;
                    }
                }
                else if (buida.posFila < fitxa.posFila)
                {
                    for (int i = buida.posFila; i < fitxa.posFila; i++)
                    {
                        matriuFitxes[i, buida.posColumna].Content = matriuFitxes[i + 1, buida.posColumna].Content;
                        matriuFitxes[i, buida.posColumna].NumObjectiu = matriuFitxes[i + 1, buida.posColumna].NumObjectiu;
                        PintaFitxa(matriuFitxes[i, buida.posColumna]);
                        buida.Visibility = Visibility.Visible;
                        buida = fitxa;
                        buida.Visibility = Visibility.Collapsed;
                    }
                }
            }

            buida.NumObjectiu = -1;

            numCorrectes = Puntua();


            sbiNCorrectes.Content = numCorrectes;
            sbiPercentatgeCompletat.Content = Math.Round((double)numCorrectes / (nFiles * nColumnes - 1) * 100, 2) + " %";

            if (numCorrectes == (nFiles * nColumnes - 1)) Acabar();
        }

        #endregion

        #region Puntuació

        private int Puntua()
        {
            int cont = 0;
            for (int i = 0; i < nFiles; i++)
            {
                for (int j = 0; j < nColumnes; j++)
                {
                    if (matriuFitxes[i, j].NumActual == matriuFitxes[i, j].NumObjectiu) cont++;
                }
            }

            return cont;
        }

        #endregion

        #region Rellotge

        public void IniciarRellotge()
        {
            tempsTranscorregut = TimeSpan.Zero;
            rellotge = new DispatcherTimer();
            rellotge.Interval = TimeSpan.FromMilliseconds(100);
            rellotge.Tick += Rellotge_Tick;
            rellotge.Start();
        }
        private void Rellotge_Tick(object? sender, EventArgs e)
        {
            tempsTranscorregut = tempsTranscorregut.Add(rellotge.Interval);
            ActualitzaTextRellotge(sbiRellotge, tempsTranscorregut);
        }
        private void ActualitzaTextRellotge(StatusBarItem sbiText, TimeSpan periode)
        {
            String cadena = String.Format("{0:00}:{1:00}",
                periode.Minutes,
                periode.Seconds);
            sbiText.Content = cadena;
        }

        #endregion

        #region Acabar
        private void Acabar()
        {
            rellotge.Stop();
            MessageBox.Show($"Felicitats!\nEl teu temps és: {sbiRellotge.Content}", "Has guanyat!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.Close();
        }
        #endregion
    }


    public static class MetodesExtensio
    {

        #region Files i Columnes
        public static void CreaFiles(this Grid graella, int numFiles)
        {
            for (int fila = 0; fila < numFiles; fila++) graella.RowDefinitions.Add(new RowDefinition());
           
        }

        public static void CreaColumnes(this Grid graella, int numColumnes)
        {
            for (int columna = 0; columna < numColumnes; columna++) graella.ColumnDefinitions.Add(new ColumnDefinition());
        }

        #endregion

        
    }
}
