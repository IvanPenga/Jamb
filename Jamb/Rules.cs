using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jamb
{
    static class Rules
    {
        private const int PairsExtraPoints = 10;
        private const int FullExtraPoints = 30;
        private const int PokerExtraPoints = 40;
        private const int YambExtraPoints = 50;

        public static int SumAll()
        {
            return DiceButton.Dices.Sum(dice => dice.Number);
        }

        //sum numbers of the same kind
        public static int SumNumbers(int number)
        {
            return DiceButton.Dices.Where(dice => dice.Number == number).Sum(dice => dice.Number);
        }

        public static int SumPair()
        {
            int sum = 0;
            int pairs = 0;
            for (int i = 1; i < 7; i++)
            {
                int count = DiceButton.Dices.Count(dice => dice.Number == i);
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
                return sum + PairsExtraPoints;
            else
                return 0;
        }

        public static int SumStraight()
        {
            if (DiceButton.Dices.Exists(dice => dice.Number == 2))
                if (DiceButton.Dices.Exists(dice => dice.Number == 3))
                    if (DiceButton.Dices.Exists(dice => dice.Number == 4))
                        if (DiceButton.Dices.Exists(dice => dice.Number == 5))
                            if (DiceButton.Dices.Exists(dice => dice.Number == 6))
                                return 45;
                            else if (DiceButton.Dices.Exists(dice => dice.Number == 1))
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
                int cnt = DiceButton.Dices.Count(dice => dice.Number == i);
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
                return sum + FullExtraPoints;
            }
            return 0;
        }

        public static int SumPoker()
        {
            for (int i = 1; i < 7; i++)
            {
                int cnt = DiceButton.Dices.Count(dice => dice.Number == i);
                if (cnt == 4 || cnt == 5)
                {
                    return (4 * i) + PokerExtraPoints;
                }
            }
            return 0;
        }

        public static int SumYamb()
        {
            for (int i = 1; i < DiceButton.Dices.Count; i++)
            {
                if (DiceButton.Dices[i].Number != DiceButton.Dices[0].Number)
                {
                    return 0;
                }
            }
            return SumAll() + YambExtraPoints;
        }

        public static int EvaluateBoxValue(Value value)
        {
            switch (value)
            {
                case Value.One:
                    return Rules.SumNumbers(1);
                case Value.Two:
                    return Rules.SumNumbers(2);
                case Value.Three:
                    return Rules.SumNumbers(3);
                case Value.Four:
                    return Rules.SumNumbers(4);
                case Value.Five:
                    return Rules.SumNumbers(5);
                case Value.Six:
                    return Rules.SumNumbers(6);
                case Value.Max:
                    return Rules.SumAll();
                case Value.Min:
                    return Rules.SumAll();
                case Value.Pair:
                    return Rules.SumPair();
                case Value.Straight:
                    return Rules.SumStraight();
                case Value.Full:
                    return Rules.SumFull();
                case Value.Poker:
                    return Rules.SumPoker();
                case Value.Yamb:
                    return Rules.SumYamb();
                default:
                    break;
            }
            return 0;
        }
    }
}
