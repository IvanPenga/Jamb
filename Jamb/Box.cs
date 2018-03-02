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
        public Category Category { get; set; }

        public int Points { get; set; }

        public bool Checked = false;
        private static bool CallsEnabled = false;

        public delegate void SimpleDelegate();
        public delegate void PointDelegate(Box box);
        public event SimpleDelegate OnCallSelected;
        public event PointDelegate OnPointChanged;


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
            OnCallSelected?.Invoke();
        }




        private void Box_Click(object sender, EventArgs e)
        {
            if (CallsEnabled)
            {
                EnableSelectedCall();
                this.Box_MouseEnter(sender,e);
            }
            else
            {
                if (tempClosed.Count > 0)
                {
                    ReturnTempClosed();
                }
                this.Enabled = false;
                Checked = true;
                Game.NextRound();
                Points = int.Parse(this.Text);
                OnPointChanged?.Invoke(this);
                UnlockNext();
            }
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

        private static List<Box> tempClosed = new List<Box>();

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


        
    }
}
