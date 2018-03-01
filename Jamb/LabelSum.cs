using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jamb
{
    public partial class LabelSum : Label
    {
        public Direction Direction { get; set; }
        public Category Category { get; set; }
        public int Value { get; set; }

        public LabelSum()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void SetValue()
        {

        }

        public void nesto()
        {

        }


    }
}
