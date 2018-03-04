using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jamb
{
    static class Game
    {
        public static event SimpleDelegate OnNextRound;
        public static event SimpleDelegate OnRollChanged;
        public delegate void SimpleDelegate();
        
        private static int rollNumber = 1;
        public static int RollNumber
        {
            get { return rollNumber; }
            set
            {
                rollNumber = value;
                OnRollChanged?.Invoke();
            }           
        }

        public static void LastMove()
        {
            DiceButton.DisableAll();
        }

        public static void NextRound()
        {
            RollNumber = 1;
            DiceButton.UnlockAll();
            DiceButton.EnableAll();
            DiceButton.RollAll();
            OnNextRound?.Invoke();
        }

        

    }
}
