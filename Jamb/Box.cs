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

        public Box()
        {
            InitializeComponent();
            Boxes.Add(this);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void Box_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            //this.Text = "asd";
            UnlockNext();
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

        public void EnableCalls()
        {

        }

        private void Box_MouseEnter(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.Text = EvaluateBoxValue().ToString();
            }
        }

        private void Box_MouseLeave(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                this.Text = "";
            }            
        }

        private int EvaluateBoxValue()
        {
            switch (this.Value)
            {
                case Value.One:
                    return Dice.SumNumbers(1);                   
                case Value.Two:
                    return Dice.SumNumbers(2);                   
                case Value.Three:
                    return Dice.SumNumbers(3);                   
                case Value.Four:
                    return Dice.SumNumbers(4);
                case Value.Five:
                    return Dice.SumNumbers(5);                   
                case Value.Six:
                    return Dice.SumNumbers(6);                    
                case Value.Max:
                    return Dice.SumAll();                    
                case Value.Min:
                    return Dice.SumAll();                   
                case Value.Pair:
                    return Dice.SumPair();
                case Value.Straight:
                    return Dice.SumStraight();
                case Value.Full:
                    return Dice.SumFull();
                case Value.Poker:
                    return Dice.SumPoker();
                case Value.Yamb:
                    return Dice.SumYamb();
                default:
                    break;
            }
            return 0;
        }
    }
}
