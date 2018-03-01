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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Dice.RollAll();
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            btnCall.Enabled = Dice.RollNumber >= 1 ? false : true;
            Dice.RollAll();            
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            Box.EnableCalls();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbRollsLeft.Text = "" + Dice.RollNumber;
        }
    }
}
