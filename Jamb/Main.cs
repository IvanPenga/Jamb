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
    public partial class Main : Form
    {

        private bool CallsEnabled = false;
        private bool OnlyCallsLeft = false;

        public Main()
        {
            InitializeComponent();
            Game.OnNextRound += Game_OnNextRound;
            Game.OnRollChanged += Game_OnRollChanged;
            BoxButton.OnOnlyCallsLeft += BoxButton_OnOnlyCallsLeft;
            
            DiceButton.RollAll();
        }

        private void BoxButton_OnOnlyCallsLeft()
        {
            OnlyCallsLeft = true;
        }

        private void Game_OnRollChanged()
        {
            switch (Game.RollNumber)
            {
                case 1: btnCall.Enabled = true; break;
                case 2: btnCall.Enabled = false; break;
                case 3: Game.LastMove(); btnRoll.Enabled = false; break;
                case 4: Game.NextRound();  break;
            }

            lbRollsLeft.Text = "Rolls: " + Game.RollNumber + " / 3";
        }

        private void Game_OnNextRound()
        {
            btnCall.Enabled = true;
            EnableRollButton();
        }

        private void btnRoll_Click(object sender, EventArgs e)
        {
            DiceButton.RollAll();
            Game.RollNumber++;
        }

        

        private void btnCall_Click(object sender, EventArgs e)
        {
            if (!CallsEnabled)
            {
                EnableCalls();
            }
            else
            {
                DisableCalls();
            }
        }

        private void EnableCalls()
        {
            CallsEnabled = true;
            BoxButton.EnableCalls();
            btnRoll.Enabled = false;
        }

        private void DisableCalls()
        {
            CallsEnabled = false;
            BoxButton.DisableCalls();
            EnableRollButton();
        }

        private void EnableRollButton()
        {
            if (OnlyCallsLeft)
                btnRoll.Enabled = false;
            else
                btnRoll.Enabled = true;
        }

        private void OnCallSelected()
        {
            btnRoll.Enabled = true;
            btnCall.Enabled = false;
            CallsEnabled = false;
        }

        private void OnBoxPointsChanged(BoxButton box)
        {

            foreach (SumLabel sumLabel in this.Controls.OfType<SumLabel>())
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
            lblTotal.Text = "Total: " + SumLabel.Total.ToString();

        }

    }
}
