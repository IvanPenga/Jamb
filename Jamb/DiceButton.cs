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
    public partial class DiceButton : Button
    {
        public int Number { get; private set; }
        private bool Locked { get; set; }
        public static List<DiceButton> Dices = new List<DiceButton>();
        private static Random random = new Random();

        public DiceButton()
        {
            InitializeComponent();
            Dices.Add(this);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public static void DisableAll()
        {
            foreach (DiceButton dice in Dices)
            {
                dice.Enabled = false;
            }
        }

        public static void EnableAll()
        {
            foreach (DiceButton dice in Dices)
            {
                dice.Enabled = true;
            }
        }

        public void Lock()
        {
            this.Locked = this.Locked ? false : true;
            SetColor();           
        }

        public static void UnlockAll()
        {
            foreach (DiceButton dice in Dices)
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
            foreach (DiceButton dice in DiceButton.Dices)
            {
                if (!dice.Locked)
                {
                    dice.Roll();
                }
            }
        }
        

        
    }
}
