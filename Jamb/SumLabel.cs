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
    public partial class SumLabel : Label
    {
        public Direction Direction { get; set; }
        public Category Category { get; set; }
        public  int Value { get; set; }

        public static int Total = 0;
        private int ones = -1;
        private int max = -1;
        private int min = -1;

        public SumLabel()
        {
            InitializeComponent();            
            this.Value = 0;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void SumMinMax(BoxButton box)
        {
            switch (box.Value)
            {
                case Jamb.Value.One: this.ones = box.Points; break;
                case Jamb.Value.Max: this.max  = box.Points; break;
                case Jamb.Value.Min: this.min  = box.Points; break;
            }
            if (ones != -1 && max != -1 && min != -1)
            {
                int sum = ones * (max - min);
                if (sum < 0)
                {
                    this.Value = 0;
                }
                else
                {
                    this.Value = sum;
                }
                this.Text = this.Value.ToString();
                Total += this.Value;
            }

        }

        public void AddToValue(int points)
        {
            this.Value += points;
            this.Text = this.Value.ToString();
            Total += points;
        }

        public void SumNumbers(int points)
        {
            this.Value += points;
            Total += points;
            if (this.Value >= 60 && this.Value < 90)
            {
                this.Value += 30;
                Total += 30;
            }
            this.Text = this.Value.ToString();
        }


    }
}
