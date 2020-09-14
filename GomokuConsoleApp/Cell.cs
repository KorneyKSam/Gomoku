using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuConsoleApp
{
    struct Cell
    {
        public int OpponentLine { get; set; } 
        public int OpponentPoints { get; set; }
        public int MyLine { get; set; }
        public int myPoints { get; set; }
    }
}
