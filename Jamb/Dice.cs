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
    public partial class Dice : Button
    {

        public int  Number { get; private set; }
        public bool Locked { get; private set; }
        public static List<Dice> Dices = new List<Dice>();
        private static Random random = new Random();

        private static int rollNumber = 0;
        public static int RollNumber
        {
            get { return rollNumber; }
            set
            {
                rollNumber = value;
                if (rollNumber > 3)
                {
                    rollNumber = 0;
                    UnlockAll();
                }
            }
        }

        public Dice()
        {
            InitializeComponent();
            Dices.Add(this);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void Lock()
        {
            this.Locked = this.Locked ? false : true;
            SetColor();           
        }

        public static void UnlockAll()
        {
            foreach (Dice dice in Dices)
            {
                dice.Enabled = true;
                dice.Locked = false;
                dice.SetColor();
            }
        }

        public void SetColor()
        {
            this.BackColor = this.Locked ? Color.Red : Color.LightGray;
        }

        public void Roll()
        {
            this.Number = random.Next(1,7);
            this.Text = this.Number.ToString();
        }

        private void Dice_Click(object sender, EventArgs e)
        {
            Lock();
        }

        public static void RollAll()
        {
            foreach (Dice dice in Dice.Dices)
            {
                if (!dice.Locked)
                {
                    dice.Roll();
                }
            }
            RollNumber++;
        }


        
    }
}
