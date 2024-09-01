using Paint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint
{
    public class ColorRGB
    {
        public byte red { get; set; }
        public byte green { get; set; }
        public byte blue { get; set; }

        public ColorRGB mcolor { get; set; }
        public ColorRGB colorStroke { get; set; }

        public Color clr { get; set; }
        public Color color { get; set; }
    }

}


