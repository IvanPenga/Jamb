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
        private static List<BoxButton> Boxes = new List<BoxButton>();
        private List<int> History = new List<int>();

        private static List<BoxButton> NonCallBox = new List<BoxButton>();
        private int MaxSizeNonCallBox = 39; // 3 * 13 (Up,Down,Free * numbers,min-max,hands)
        private static List<BoxButton> TemporaryClosed = new List<BoxButton>();

        public Value Value { get; set; }
        public Direction Direction { get; set; }
        public Category Category { get; set; }

        public int Points { get; set; }

        public bool Checked = false;
        public bool Open { get; set; }

        private static bool CallsEnabled = false;

        public delegate void SimpleDelegate();
        public delegate void HistoryDelegate(Direction direction, Value value, List<int> history);
        public delegate void PointDelegate(BoxButton box);
        public event SimpleDelegate OnCallSelected;
        public static event SimpleDelegate OnOnlyCallsLeft;
        public event PointDelegate OnPointChanged;
        public event HistoryDelegate OnBoxHover;

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
                box.ChangeBoxState(false);
            }
            ChangeBoxState(true);
            OnCallSelected?.Invoke();
        }

        private void Box_Click(object sender, EventArgs e)
        {
            if (Open)
            {
                if (CallsEnabled)
                {
                    EnableSelectedCall();
                    CallsEnabled = false;
                    this.Box_MouseEnter(sender, e);
                }
                else
                {
                    this.Click -= Box_Click;
                    RestoreTemporaryClosedBoxes();
                    CheckIfOnlyCallsLeft();
                    DisableBox();
                    SetHistory();
                    UnlockNext();
                    SetPoints();
                }
            }

        }

        private void SetHistory()
        {
            History = DiceButton.Dices.Select(d => d.Number).ToList();
        }

        private void DisableBox()
        {
            Checked = true;
        }

        private void CheckIfOnlyCallsLeft()
        {
            if (this.Direction != Direction.Call)
            {
                NonCallBox.Add(this);
                if (NonCallBox.Count >= MaxSizeNonCallBox)
                {
                    OnOnlyCallsLeft?.Invoke();
                }
            }
        }

        private void RestoreTemporaryClosedBoxes()
        {
            if (TemporaryClosed.Count > 0)
            {
                ReturnTempClosed();
            }
        }

        private void SetPoints()
        {
            try
            {
                Points = int.Parse(this.Text);
            }
            catch(Exception)
            {
                Points = Rules.EvaluateBoxValue(this.Value);
            }
            this.Text = Points.ToString();
            OnPointChanged?.Invoke(this);
        }


        private void Unlock()
        {
            ChangeBoxState(true);
        }

        private void UnlockNext()
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

        private void ChangeBoxState(bool isOpen)
        {
            if (isOpen)
            {
                this.Open = true;
                this.BackColor = Color.LightGray;
            }
            else
            {
                this.Open = false;
                this.BackColor = Color.Gray;
            }
        }

        private static void ReturnTempClosed()
        {
            foreach (BoxButton box in Boxes.Where(box => box.Direction == Direction.Call))
            {
                box.ChangeBoxState(false); 
            }
            foreach (BoxButton box in TemporaryClosed)
            {
                box.ChangeBoxState(true);
            }
            TemporaryClosed.Clear();
        }


        public static void EnableCalls()
        {
            foreach (BoxButton box in Boxes)
            {
                if (box.Direction == Direction.Call && box.Checked == false)
                {
                    box.ChangeBoxState(true);
                }
                else if (box.Open == true)
                {
                    TemporaryClosed.Add(box);
                    box.ChangeBoxState(false);
                }
            }
            CallsEnabled = true;
        }

        private void Box_MouseEnter(object sender, EventArgs e)
        {
            if (!Checked && !CallsEnabled && Open)
            {
                Points = Rules.EvaluateBoxValue(this.Value);
                this.Text = Points.ToString();
            }
            else if (this.Checked)
            {
                InvokeHistory();
            }
        }

        private void InvokeHistory()
        {
            if (this.History.Count > 0)
            {
                OnBoxHover?.Invoke(this.Direction,this.Value,this.History);
            }
        }

        private void Box_MouseLeave(object sender, EventArgs e)
        {
            if (!Checked)
            {
                this.Text = "";
            }            
        }


        
    }
}
