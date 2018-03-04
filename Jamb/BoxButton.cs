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
    public partial class BoxButton : Button
    {
        public static List<BoxButton> Boxes = new List<BoxButton>();

        public static List<BoxButton> NonCallBox = new List<BoxButton>();
        public int MaxSizeNonCallBox = 39; // 3 * 13 (Up,Down,Free * numbers,min-max,hands)

        public Value Value { get; set; }
        public Direction Direction { get; set; }
        public Category Category { get; set; }

        public int Points { get; set; }

        public bool Checked = false;
        private static bool CallsEnabled = false;

        public delegate void SimpleDelegate();
        public delegate void PointDelegate(BoxButton box);
        public event SimpleDelegate OnCallSelected;
        public static event SimpleDelegate OnOnlyCallsLeft;
        public event PointDelegate OnPointChanged;
        

        public BoxButton()
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
            foreach (BoxButton box in Boxes.Where(box => box.Direction == Direction.Call))
            {
                box.Enabled = false;
            }
            this.Enabled = true;
            OnCallSelected?.Invoke();
        }

        private void Box_Click(object sender, EventArgs e)
        {
            if (CallsEnabled)
            {
                EnableSelectedCall();
                CallsEnabled = false;
                this.Box_MouseEnter(sender,e);
            }
            else
            {
                if (tempClosed.Count > 0)
                {
                    ReturnTempClosed();
                }
                if (this.Direction != Direction.Call)
                {
                    NonCallBox.Add(this);
                    if (NonCallBox.Count >= 39)
                    {
                        OnOnlyCallsLeft?.Invoke();
                    }
                }
                this.Enabled = false;
                Checked = true;
                Game.NextRound();              
                SetPoints();
                OnPointChanged?.Invoke(this);
                UnlockNext();
            }
        }

        private void SetPoints()
        {
            try
            {
                Points = int.Parse(this.Text);
            }
            catch(Exception e)
            {
                Points = Rules.EvaluateBoxValue(this.Value);
            }
            this.Text = Points.ToString();
        }


        public void Unlock()
        {
            this.Enabled = true;
        }

        public void UnlockNext()
        {
            foreach (BoxButton box in Boxes)
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

        public static void DisableCalls()
        {
            CallsEnabled = false;
            ReturnTempClosed();
        }

        private static void ReturnTempClosed()
        {
            foreach (BoxButton box in Boxes.Where(box => box.Direction == Direction.Call))
            {
                box.Enabled = false;
            }
            foreach (BoxButton box in tempClosed)
            {
                box.Enabled = true;
            }
            tempClosed.Clear();
        }

        private static List<BoxButton> tempClosed = new List<BoxButton>();

        public static void EnableCalls()
        {
            foreach (BoxButton box in Boxes)
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
                Points = Rules.EvaluateBoxValue(this.Value);
                this.Text = Points.ToString();
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
