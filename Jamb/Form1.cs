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
            Game.OnNextRound += Game_OnNextRound;
            Game.OnRollChanged += Game_OnRollChanged;
            
            Dice.RollAll();
        }

        private void Game_OnRollChanged()
        {
            switch (Game.RollNumber)
            {
                case 1: btnCall.Enabled = true;  break;
                case 2: btnCall.Enabled = false; break;
                case 3: Game.LastMove(); btnRoll.Enabled = false; break;
                case 4: Game.NextRound();  break;
            }

            lbRollsLeft.Text = "Rolls: " + Game.RollNumber + " / 3";
        }

        private void Game_OnNextRound()
        {
            btnCall.Enabled = true;
            btnRoll.Enabled = true;
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            Dice.RollAll();
            Game.RollNumber++;
        }

        

        private void btnCall_Click(object sender, EventArgs e)
        {
            Box.EnableCalls();
            btnCall.Enabled = false;
            btnRoll.Enabled = false;
        }

        private void OnCallSelected()
        {
            btnRoll.Enabled = true;
        }

        private void OnBoxPointsChanged(Box box)
        {

            foreach (LabelSum sumLabel in this.Controls.OfType<LabelSum>())
            {
                
                if (sumLabel.Category == Category.Numbers && box.Category == Category.Numbers)
                {
                    if (sumLabel.Direction == box.Direction)
                    {
                        sumLabel.SumNumbers(box.Points);              
                    }
                }
                if ((box.Value == Value.One && sumLabel.Category == Category.MinMax) || (sumLabel.Category == Category.MinMax && box.Category == Category.MinMax))
                {
                    if (sumLabel.Direction == box.Direction)
                    {
                        sumLabel.SumMinMax(box);
                    }
                }
                if (sumLabel.Category == Category.Hands && box.Category == Category.Hands)
                {
                    if (sumLabel.Direction == box.Direction)
                    {
                        sumLabel.AddToValue(box.Points);
                        return;
                    }
                }

            }
            lblTotal.Text = "Total: " + LabelSum.Total.ToString();

        }

    }
}
