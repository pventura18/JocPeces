using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace JocPeces
{
    internal class Fitxa : Button
    {
        public int NumActual {  get; set; }
        public int NumObjectiu { get; set; }
        public int posFila { get; set; }
        public int posColumna { get; set; }

    }
}
