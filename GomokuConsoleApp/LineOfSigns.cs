using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuConsoleApp
{
    struct LineOfSigns
    {
        public int StartOfLineX { get; set; } 
        public int StartOfLineY { get; set; }
        public int CountOfSigns { get; set; }
        public char SignOfLine { get; set; }
        public bool IsNotClosed { get; set; }
    }
}
