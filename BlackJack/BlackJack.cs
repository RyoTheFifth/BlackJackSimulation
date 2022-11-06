using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack;

namespace BlackJack
{
    internal class BlackJackSimulation
    {
        public static int maxSim = 10000;
        static int simCount = 1;
        static int totalmoney = 10000;
        public static bool finished = true;
        static void Main(string[] args)
        {
            Console.WriteLine("何回シミュレーションしますか？");
            maxSim = int.Parse(Console.ReadLine());
            simCount = 1;
            totalmoney = 10000;
            for (int i = 0; i < 1;)
            {
                if (finished == true)
                {
                    finished = false;
                    new BlackJackGame(Trunp.PickRandom());
                }
            }
        }
        public static void Stand(int playerCardSum ,int bet, int parentCard, int cloneFlag)
        {
            List<int> parentCards = new List<int>();
            parentCards.Add(parentCard);
            int sum = SumParent(parentCards);
            if(sum > 21)
            {
                End(bet, cloneFlag);
            } else if(playerCardSum > sum)
            {
                End(bet, cloneFlag);
            } else if (playerCardSum < sum)
            {
                End(bet * -1, cloneFlag);
            } else
            {
                End(0, cloneFlag);
            }

        }
        public static void End(int bet, int cloneFlag)
        {
            totalmoney = totalmoney + bet;
            Console.WriteLine(totalmoney);
            if (simCount < maxSim)
            {
                if(cloneFlag == 0)
                {
                    simCount++;
                    Console.WriteLine("BlackJackSimulation has occured " + (simCount - 1) + " times");
                    finished = true;
                }
            } else
            {
                decimal lossRate = (decimal)1 - (decimal)(10000 - totalmoney) / (simCount * 100);
                Console.WriteLine(lossRate.ToString("P6"));
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
        //Basic strategy sample
        private int[,] basicStrategy = new int[29, 10] {
            {1,1,1,1,1,1,1,1,1,1},
            {1,2,2,2,2,1,1,1,1,1},
            {2,2,2,2,2,2,2,2,1,1},
            {2,2,2,2,2,2,2,2,2,1},
            {1,1,4,4,4,1,1,1,1,1},
            {4,4,4,4,4,1,1,1,1,1},
            {4,4,4,4,4,1,1,1,1,1},
            {4,4,4,4,4,1,1,1,1,1},
            {4,4,4,4,4,1,1,1,1,1},
            {4,4,4,4,4,4,4,4,4,4},
            {1,1,1,2,2,1,1,1,1,1},
            {1,1,1,2,2,1,1,1,1,1},
            {1,1,2,2,2,1,1,1,1,1},
            {1,1,2,2,2,1,1,1,1,1},
            {1,2,2,2,2,1,1,1,1,1},
            {4,2,2,2,2,4,4,1,1,1},
            {4,4,4,4,4,4,4,4,4,4},
            {4,4,4,4,4,4,4,4,4,4},
            {4,4,4,4,4,4,4,4,4,4},
            {3,3,3,3,3,3,1,1,1,1},
            {1,3,3,3,3,3,1,1,1,1},
            {1,1,1,3,3,1,1,1,1,1},
            {2,2,2,2,2,2,2,2,1,1},
            {3,3,3,3,3,3,1,1,1,1},
            {3,3,3,3,3,3,1,1,1,1},
            {3,3,3,3,3,3,3,1,1,1},
            {3,3,3,3,3,4,3,3,4,4},
            {4,4,4,4,4,4,4,4,4,4},
            {3,3,3,3,3,3,3,3,3,3}};
        List<int> playerCards = new List<int>();
        int act = 0;
        int actRemaining = 2;
        int bet = 0;
        int parentCard = 0;
        int ph = 0;
        int cloneFlag = 0;
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
                BlackJackSimulation.End((int)(bet * 0.5), cloneFlag);
            }
            else
            {
                ChooseAct(1);
            }
        }
        public BlackJackGame(int parentCard, int act, int splitCard, int cloneFlag)
        {
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
                            act = basicStrategy[Trunp.IDtoValue(playerCards[0]) + 17, Trunp.IDtoValue(parentCard) - 2];
                        }
                        else if (Trunp.aceNum(playerCards) > 0)
                        {
                            act = basicStrategy[Trunp.IDtoValue(playerCards[1]) + 8, Trunp.IDtoValue(parentCard) - 2];
                        }
                        else
                        {
                            act = basicStrategy[SumColumn(SumPlayer()), Trunp.IDtoValue(parentCard) - 2];
                        }
                        break;
                    case 2:
                            act = basicStrategy[SumColumn(SumPlayer()), Trunp.IDtoValue(parentCard) - 2];
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
                        CheckBust(1);
                        BlackJackSimulation.Stand(SumPlayer(), bet, parentCard, cloneFlag);
                        break;
                    case 3:
                        playerCards.RemoveAt(1);
                        if(playerCards[0] == 1)
                        {
                            actRemaining = 1;
                        }
                        new BlackJackGame(parentCard,act,playerCards[0],1);
                        ChooseAct(2);
                        break;
                    default:
                        BlackJackSimulation.Stand(SumPlayer(),bet,parentCard, cloneFlag);
                        break;
                }
            } else
            {
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
                    sum -= 10;
                }
            }
            return sum;
        }
        private void CheckBust(int type)
        {
            if(SumPlayer() > 21)
            {
                BlackJackSimulation.End(bet * -1, cloneFlag);
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
        public static int nonAceSum(List<int> l)
        {
            int sum = 0;
            foreach (int a in l)
            {
                if(a > 1)
                {
                    sum = sum + IDtoValue(a);
                }
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
