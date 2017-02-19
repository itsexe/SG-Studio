using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Studio.Classes.GUI
{
    class SPR_Image
    {
        public string imageName { get; set; }
        public List<Tuple<string, Size, int>> files = new List<Tuple<string, Size, int>>();

    }
}
