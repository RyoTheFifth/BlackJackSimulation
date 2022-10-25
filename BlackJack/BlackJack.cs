using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack;

namespace BlackJack
{
    internal class BlackJackMastermind
    {
        static List<decimal> mastermind = new List<decimal>();
        static void Main(string[] args)
        {
            new BlackJackSimulation();
        }
        public static void Next(decimal lossRate)
        {
            mastermind.Add(lossRate);
            Console.WriteLine("totalLossRate: " + Average() * 100);
            if(Console.ReadLine() != null)
            {
            }
        }
        private static decimal Average()
        {
            decimal sum = 0;
            foreach (decimal value in mastermind)
            {
                sum = sum + value;
            }
            return sum / mastermind.Count;
        }
    }
    internal class BlackJackSimulation
    {
        static int simCount = 1;
        static int totalmoney = 10000;
        public BlackJackSimulation()
        {
            simCount = 1;
            totalmoney = 10000;
            BlackJackGame.DataInitialization();
            new BlackJackGame(Trunp.PickRandom());
        }
        public static void Stand(int playerCardSum ,int bet, int parentCard, int cloneFlag)
        {
            List<int> parentCards = new List<int>();
            parentCards.Add(parentCard);
            int sum = SumParent(parentCards);
            if(sum > 21)
            {
                Win(bet, cloneFlag);
            } else if(playerCardSum > sum)
            {
                Win(bet, cloneFlag);
            } else if (playerCardSum < sum)
            {
                Lose(bet, cloneFlag);
            } else
            {
                Win(0, cloneFlag);
            }

        }
        public static void Win(int bet ,int cloneFlag)
        {
            totalmoney = totalmoney + bet;
            End(cloneFlag);
        }
        public static void Lose(int bet, int cloneFlag)
        {
            totalmoney = totalmoney - bet;
            End(cloneFlag);
        }
        public static void End(int cloneFlag)
        {
            Console.WriteLine(totalmoney);
            if (simCount < 1000)
            {
                if(cloneFlag == 0)
                {
                    simCount++;
                    Console.WriteLine("BlackJackSimulation has occured " + (simCount - 1) + " times");
                    new BlackJackGame(Trunp.PickRandom());
                }
            } else
            {
                decimal lossRate = (decimal)(10000 - totalmoney) / (simCount * 10000);
                Console.WriteLine(lossRate.ToString("P4"));
                BlackJackMastermind.Next(lossRate);
            }
        }
        private static int SumParent(List<int> parentCards)
        {
            int sum = 0;
            for(int i = 0; i < 1;)
            {
                if (sum < 17)
                {
                    parentCards.Add(Trunp.PickRandom());
                    sum = Trunp.Sum(parentCards);
                } else 
                {
                        i = 1;
                }
            }
            for (int j = 0; j < Trunp.aceNum(parentCards); j++)
            {
                if (sum > 21)
                {
                    sum = sum - 10;
                }
            }
            Console.Write("parentCards: ");
            Trunp.DisplayCards(parentCards);
            return sum;
        }
    }
    internal class BlackJackGame
    {
        private static int[,] sumStrategy = new int[10, 10];
        private const string sumStrategyData = 
            "1111111111" +
            "1222211111" +
            "2222222211" +
            "2222222221" +
            "1144411111" +
            "4444411111" +
            "4444411111" +
            "4444441111" +
            "4444441111" +
            "4444444444";
        private static int[,] aceStrategy = new int[10, 10];
        private const string aceStrategyData =
            "1112211111" +
            "1112211111" +
            "1122211111" +
            "1122211111" +
            "1222211111" +
            "4222244111" +
            "4444444444" +
            "4444444444" +
            "4444444444" +
            "4444444444";
        private static int[,] pairStrategy = new int[10, 10];
        private const string pairStrategyData =
            "3333333333" +
            "3333331111" +
            "1333331111" +
            "1113311111" +
            "2222222211" +
            "3333331111" +
            "3333331111" +
            "3333333111" +
            "3333343344" +
            "4444444444";
        List<int> playerCards = new List<int>();
        static int end = 0;
        int act = 0;
        int actRemaining = 2;
        int bet = 0;
        int parentCard = 0;
        int ph = 0;
        int cloneFlag = 0;
        public static void DataInitialization()
        {
            SetData(sumStrategy, sumStrategyData);
            SetData(aceStrategy, aceStrategyData);
            SetData(pairStrategy, pairStrategyData);
        }
        public BlackJackGame(int parentCard)
        {
            act = 2;
            bet = 100;
            this.parentCard = parentCard;
            Console.WriteLine("parentCard: " + parentCard);
            playerCards.Add(Trunp.PickRandom());
            playerCards.Add(Trunp.PickRandom());
            playerCards.Sort();
            if (Trunp.IDtoValue(playerCards[0]) == 11 && Trunp.IDtoValue(playerCards[1]) == 10)
            {
                Console.WriteLine("BlackJack!!");
                end++;
                BlackJackSimulation.Win((int)(bet * 0.5), cloneFlag);
            }
            else
            {
                ChooseAct(1);
            }
        }
        public BlackJackGame(int parentCard, int act, int splitCard, int cloneFlag)
        {
            Console.WriteLine("end: " + end);
            this.cloneFlag = cloneFlag;
            actRemaining = act;
            bet = 100;
            this.parentCard = parentCard;
            playerCards.Add(splitCard);
            playerCards.Sort();
            ChooseAct(2);
        }
        private void ChooseAct(int type)
        {
            Console.WriteLine("end: " + end);
            Console.Write("playerCard: ");
            Trunp.DisplayCards(playerCards);
            if (actRemaining > 0)
            {
                if (actRemaining == 1)
                {
                    actRemaining--;
                }
                switch(type)
                {
                    case 1:
                        if (playerCards[0] == playerCards[1])
                        {
                            Console.WriteLine("pairStrategy");
                            act = pairStrategy[Trunp.IDtoValue(playerCards[0]) - 2, Trunp.IDtoValue(parentCard) - 2];
                        }
                        else if (Trunp.aceNum(playerCards) > 0)
                        {
                            act = aceStrategy[Trunp.IDtoValue(playerCards[1]) - 2, Trunp.IDtoValue(parentCard) - 2];
                        }
                        else
                        {
                            act = sumStrategy[SumColumn(SumPlayer()), Trunp.IDtoValue(parentCard) - 2];
                        }
                        break;
                    case 2:
                            act = sumStrategy[SumColumn(SumPlayer()), Trunp.IDtoValue(parentCard) - 2];
                        break;
                }

                Console.WriteLine("act: " + act);
                switch (act)
                {
                    case 1:
                        ph = Trunp.PickRandom();
                        Console.WriteLine("got: " + ph);
                        playerCards.Add(ph);
                        CheckBust(0);
                        break;
                    case 2:
                        bet = bet * 2;
                        ph = Trunp.PickRandom();
                        Console.WriteLine("got: " + ph);
                        playerCards.Add(ph);
                        end++;
                        BlackJackSimulation.Stand(SumPlayer(), bet, parentCard, cloneFlag);
                        break;
                    case 3:
                        playerCards.RemoveAt(1);
                        if(playerCards[0] == 1)
                        {
                            act = 1;
                        }
                        new BlackJackGame(parentCard,act,playerCards[0],1);
                        ChooseAct(2);
                        break;
                    default:
                        end++;
                        BlackJackSimulation.Stand(SumPlayer(),bet,parentCard, cloneFlag);
                        break;
                }
            } else
            {
                end++;
                BlackJackSimulation.Stand(SumPlayer(), bet, parentCard, cloneFlag);
            }
        }
        private int SumPlayer()
        {
            int sum = Trunp.Sum(playerCards);

            for (int j = 0; j < Trunp.aceNum(playerCards); j++)
            {
                if (sum > 21)
                {
                    sum = sum - 10;
                }
            }
            return sum;
        }
        private void CheckBust(int type)
        {
            if(SumPlayer() > 21)
            {
                end++;
                BlackJackSimulation.Lose(bet, cloneFlag);
            } else
            {
                if(type == 0)
                {
                    ChooseAct(2);
                }
            }
        }
        private int SumColumn(int i)
        {
            if(i < 9)
            {
                return 0;
            } else
            {
                switch(i)
                {
                    case 9: return 1;
                    case 10: return 2;
                    case 11: return 3;
                    case 12: return 4;
                    case 13: return 5;
                    case 14: return 6;
                    case 15: return 7;
                    case 16: return 8;
                    default: return 9; 
                }
            }
        }
        private static void SetData(int[,] l, string data)
        {
            for(int i = 0; i < 10; i++)
            {
                for(var j = 0; j < 10; j++)
                {
                    l[i,j] = int.Parse(data.Substring(i * 10 + j,1));
                }
            }
        }
    }
    internal class Trunp
    {
        public static int PickRandom()
        {
            Random random = new Random();
            int i = random.Next(1, 14);
            return i;
        }
        public static int IDtoValue(int i)
        {
            switch (i)
            {
                case 1: return 11;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 10;
                case 12: return 10;
                case 13: return 10;
                default: return 0;
            }
        }
        public static int Sum(List<int> l)
        {
            int sum = 0;
            foreach (int a in l)
            {
                sum = sum + IDtoValue(a);
            }
            return sum;
        }
        public static int aceNum(List<int> l)
        {
            int num = 0;
            foreach (int i in l)
            {
                if (i == 1)
                {
                    num++;
                }
            }
            return num;
        }
        public static void DisplayCards(List<int> l)
        {
            foreach (int i in l)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }
    }
}
