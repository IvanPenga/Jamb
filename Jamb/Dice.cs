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
        public static int RollNumber { get; set; }

        public Dice()
        {
            InitializeComponent();
            RollNumber = 0;
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

        public static void RollUnlocked()
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

        public static int SumAll()
        {
            return Dices.Sum(dice => dice.Number);
        }

        //sum numbers of the same kind
        public static int SumNumbers(int number)
        {
            return Dices.Where(dice => dice.Number == number).Sum(dice => dice.Number);
        }
        
        public static int SumPair()
        {
            int sum = 0;
            int pairs = 0;
            for (int i = 1; i < 7; i++)
            {
                int count = Dices.Count(dice => dice.Number == i);
                if (count == 4 || count == 5)
                {
                    return 4 * i;
                }
                if (count >= 2)
                {
                    sum += i * 2;
                    pairs++;
                }
            }
            if (pairs == 2)
                return sum;
            else
                return 0;
        }
        
        public static int SumStraight()
        {
            if (Dices.Exists(dice => dice.Number == 2))
                if (Dices.Exists(dice => dice.Number == 3))
                    if (Dices.Exists(dice => dice.Number == 4))
                        if (Dices.Exists(dice => dice.Number == 5))
                            if (Dices.Exists(dice => dice.Number == 6))
                                return 45;
                            else if (Dices.Exists(dice => dice.Number == 1))
                                return 35;
            return 0;
        }
        
        public static int SumFull()
        {
            int sum = 0;
            bool two = false;
            bool three = false;
            for (int i = 1; i < 7; i++)
            {
                int cnt = Dices.Count(dice => dice.Number == i);
                if (cnt == 5)
                {
                    return SumAll();
                }
                if (cnt == 2 && two == false)
                {
                    two = true;
                    sum += 2 * i;
                }
                if (cnt == 3 && three == false)
                {
                    three = true;
                    sum += 3 * i;
                }
            }
            if (two && three)
            {
                return sum;
            }
            return 0;
        }
        
        public static int SumPoker()
        {
            for (int i = 1; i < 7; i++)
            {
                int cnt = Dices.Count(dice => dice.Number == i);
                if (cnt == 4 || cnt == 5)
                {
                    return 4*i;
                }
            }
            return 0;
        }
        
        public static int SumYamb()
        {
            for (int i = 1; i < Dices.Count; i++)
            {
                if (Dices[i].Number != Dices[0].Number)
                {
                    return 0;
                }
            }
            return SumAll();
        }
        
    }
}
