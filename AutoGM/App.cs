using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGM
{
    class App
    {
        static string[] lastRoll;

        static void Main(string[] args)
        {
            string input = "";
            while (input != "stop")
            {
                input = Console.ReadLine();
                input = ParseInput(input);
            }
        }

        static string ParseInput(string input)
        {
            string[] inputArr = input.Split(' ');
            switch (inputArr[0])
            {
                case "":
                    Console.WriteLine();
                    break;
                case "stop":
                    return "stop";
                case "r":
                case "roll":
                    ParseRoll(inputArr);
                    break;
                case "rs":
                case "rollstats":
                    try
                    {
                        Console.WriteLine(RollStats(lastRoll[1]));
                    }
                    catch
                    {
                        Console.WriteLine("Command rollstats shows the stats of the previous roll, which has not been set.");
                    }
                    break;
                default:
                    Console.WriteLine("Unknown command: " + input[0] + ". Write \"help\" for a list of commands.");
                    break;
            }
            return input;
        }

        static void ParseRoll(string[] input)
        {
            switch (input.Length)
            {
                case 1:
                    if (lastRoll == null)
                    {
                        Console.WriteLine("Previous roll not yet set, please include an argument after \"roll\".");
                        return;
                    }
                    ParseRoll(lastRoll);
                    return;
                case 2:
                    try
                    {
                        Console.WriteLine(Roll(input[1]));
                        lastRoll = input;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid dice roll. Please use format ndn+dn+n (order doesn't matter), where each n is any integer. +-ndn+ is ok, +-dn+ is ok, +-n+ is ok, +d-n+ is not.");
                        return;
                    }
                    break;
                default:

                    break;
            }
        }

        static int Roll(string dice)
        {
            Random rnd = new Random();
            int total = 0;
            while (dice.Length > 0)
            {
                bool neg = dice[0] == '-';
                if (dice[0] == '-' || dice[0] == '+')
                {
                    dice = dice.Substring(1);
                }
                int nextDie = Math.Min(dice.IndexOf('+'), dice.IndexOf('-'));
                if (nextDie == -1)
                {
                    nextDie = Math.Max(dice.IndexOf('+'), dice.IndexOf('-'));
                }
                if (nextDie == -1)
                {
                    nextDie = dice.Length;
                }
                int index = dice.IndexOf('d');
                int die;
                if (index < nextDie)
                {
                    die = Int32.Parse(dice.Substring(index + 1, nextDie - index - 1));
                }
                else if (index > nextDie)
                {
                    die = Int32.Parse(dice.Substring(0, nextDie));
                }
                else
                {
                    die = Int32.Parse(dice);
                }
                if (index == -1 || index > nextDie)
                {
                    if (neg)
                    {
                        total -= die;
                    }
                    else
                    {
                        total += die;
                    }
                }
                else if (index == 0)
                {
                    if (neg)
                    {
                        total -= rnd.Next(1, die + 1);
                    }
                    else
                    {
                        total += rnd.Next(1, die + 1);
                    }
                }
                else
                {
                    int count = Int32.Parse(dice.Substring(0, index));
                    for (; count > 0; count--)
                    {
                        if (neg)
                        {
                            total -= rnd.Next(1, die + 1);
                        }
                        else
                        {
                            total += rnd.Next(1, die + 1);
                        }
                    }
                }
                dice = dice.Substring(nextDie);
            }
            return total;
        }

        static string RollStats(string dice)
        {
            int min = 0;
            int max = 0;
            double avg = 0;
            string output = "Stats for roll " + dice;
            while (dice.Length > 0)
            {
                bool neg = dice[0] == '-';
                if (dice[0] == '-' || dice[0] == '+')
                {
                    dice = dice.Substring(1);
                }
                int nextDie = Math.Min(dice.IndexOf('+'), dice.IndexOf('-'));
                if (nextDie == -1)
                {
                    nextDie = Math.Max(dice.IndexOf('+'), dice.IndexOf('-'));
                }
                if (nextDie == -1)
                {
                    nextDie = dice.Length;
                }
                int index = dice.IndexOf('d');
                int die;
                if (index < nextDie)
                {
                    die = Int32.Parse(dice.Substring(index + 1, nextDie - index - 1));
                }
                else if (index > nextDie)
                {
                    die = Int32.Parse(dice.Substring(0, nextDie));
                }
                else
                {
                    die = Int32.Parse(dice);
                }
                if (index == -1 || index > nextDie)
                {
                    if (neg)
                    {
                        avg -= die;
                        min -= die;
                        max -= die;
                    }
                    else
                    {
                        avg += die;
                        min += die;
                        max += die;
                    }
                }
                else if (index == 0)
                {
                    if (neg)
                    {
                        avg -= (die + 1) / 2.0;
                        min -= die;
                        max -= 1;
                    }
                    else
                    {
                        avg += (die + 1) / 2.0;
                        min += 1;
                        max += die;
                    }
                }
                else
                {
                    int count = Int32.Parse(dice.Substring(0, index));
                    for (; count > 0; count--)
                    {
                        if (neg)
                        {
                            avg -= (die + 1) / 2.0;
                            min -= die;
                            max -= 1;
                        }
                        else
                        {
                            avg += (die + 1) / 2.0;
                            min += 1;
                            max += die;
                        }
                    }
                }
                dice = dice.Substring(nextDie);
            }

            return output + ": min=" + min + " max=" + max + " avg=" + avg;
        }
    }
}
