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
    public partial class Box : Button
    {
        public static List<Box> Boxes = new List<Box>();

        public Value Value { get; set; }
        public Direction Direction { get; set; }
        public bool Checked = false;
        private static bool CallsEnabled = false;
        public event OnClickDelegate OnBoxClick;
        public delegate void OnClickDelegate(int value);

        public Box()
        {
            InitializeComponent();
            Boxes.Add(this);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        private void EnableSelectedCall()
        {
            foreach (Box box in Boxes.Where(box => box.Direction == Direction.Call))
            {
                box.Enabled = false;
            }
            this.Enabled = true;
            CallsEnabled = false;
        }




        private void Box_Click(object sender, EventArgs e)
        {
            if (CallsEnabled)
            {
                EnableSelectedCall();
            }
            else
            {
                if (tempClosed.Count > 0)
                {
                    ReturnTempClosed();
                }
                this.Enabled = false;
                Checked = true;
                NextRound();
                UnlockNext();
            }

        }

        private void NextRound()
        {
            Dice.RollNumber = 0;
            Dice.UnlockAll();
            Dice.RollAll();
        }

        public void Unlock()
        {
            this.Enabled = true;
        }

        public void UnlockNext()
        {
            foreach (Box box in Boxes)
            {
                if (this.Direction == Direction.Down && box.Direction == Direction.Down && box.Value == Value + 1)
                {
                    box.Unlock();
                    break;
                }
                else if (box.Direction == Direction.Up && this.Direction == Direction.Up && box.Value == Value - 1)
                {
                    box.Unlock();
                    break;
                }
            }
        }

        public static void ReturnTempClosed()
        {
            foreach (Box box in Boxes.Where(box => box.Direction == Direction.Call))
            {
                box.Enabled = false;
            }
            foreach (Box box in tempClosed)
            {
                box.Enabled = true;
            }
            tempClosed.Clear();
        }

        public static List<Box> tempClosed = new List<Box>();

        public static void EnableCalls()
        {
            foreach (Box box in Boxes)
            {
                if (box.Direction == Direction.Call && box.Checked == false)
                {
                    box.Enabled = true;
                }
                else if (box.Enabled == true)
                {
                    tempClosed.Add(box);
                    box.Enabled = false;
                }
            }
            CallsEnabled = true;
        }

        private void Box_MouseEnter(object sender, EventArgs e)
        {
            if (this.Enabled && !CallsEnabled)
            {
                this.Text = Rules.EvaluateBoxValue(this.Value).ToString();
            }
        }

        private void Box_MouseLeave(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.Text = "";
            }            
        }

        private void HandleClick()
        {
            if (this.Direction == Direction.Down && Value == Value.One)
            {
                OnBoxClick?.Invoke(2);
            }
        }

        
    }
}
