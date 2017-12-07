/*
Working from left-to-right if no digit is exceeded by the digit to its left it is called an increasing number; for example, 134468.

Similarly if no digit is exceeded by the digit to its right it is called a decreasing number; for example, 66420.

We shall call a positive integer that is neither increasing nor decreasing a "bouncy" number; for example, 155349.

As n increases, the proportion of bouncy numbers below n increases such that there are only 12951 numbers below one-million that are not bouncy and only 277032 non-bouncy numbers below 1010.

How many numbers below a googol(10100) are not bouncy?
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem113
{
    class Program
    {
        enum directionEnum { undefined, up, down };
        static void Main(string[] args)
        {
            Console.WriteLine(Solve());
        }

        public static long Solve()
        {
            int prevLen = 0, highestDigitIncremented = 0;
            Stopwatch stopwatch;
            int[] counterArray = new int[100]; for (int i = 0; i < counterArray.Length; i++) { counterArray[i] = 0; }
            long result = 0;
            directionEnum direction;
            int numLength = 100;
            int[] num = new int[numLength];     // 999999   - below 1 million
            for (int i = 0; i < num.Length; i++)
            {
                num[i] = 0;
            }
            num[num.Length - 1] = 1;        // Prime the pump with the first positive number
            //num[0] = 5;  num[1] = 5;  num[2] = 5; 
            try
            {
                bool keepGoing, addOne, foundNonZeroDigit;
                stopwatch = Stopwatch.StartNew();
                highestDigitIncremented = numLength - 1;
                while (true)
                {
                    //if (num[numLength-3] == 1 && num[numLength - 2] == 0 && num[numLength - 1] == 0) {Console.WriteLine("Bingo");}  // for debugging
                    //for (int k = 0; k < num.Length; k++) { Console.Write(num[k]); } Console.WriteLine(); 
                    direction = directionEnum.undefined;
                    int i;
                    keepGoing = true; addOne = true; foundNonZeroDigit = false;
                    for (i = highestDigitIncremented; i < numLength - 1; i++)
                    {
                        if (num[i] != 0)
                        {
                            foundNonZeroDigit = true;
                        }
                        addOne = true;
                        if (!foundNonZeroDigit) { continue; }
                        switch (direction)
                        {
                            case directionEnum.undefined:
                                if (num[i] > num[i + 1])
                                {
                                    direction = directionEnum.down;
                                } else {
                                    if (num[i] < num[i + 1])
                                    {
                                        direction = directionEnum.up;
                                    }
                                }
                                break;
                            case directionEnum.down:
                                if (num[i] < num[i + 1])
                                {
                                    keepGoing = false; addOne = true;
                                    // The number is bouncy. Give up on it. We went from down to up
                                    //PrintNum(num);
                                    for (int j = i + 1; j < numLength; j++) { num[j] = 9; }
                                    //Console.Write(":"); PrintNum(num); Console.WriteLine();
                                    direction = directionEnum.undefined;
                                }
                                break;
                            case directionEnum.up:
                                if (num[i] > num[i + 1])
                                {
                                    // The number is bouncy. Give up on it. We went from up to down.
                                    keepGoing = false; addOne = false;
                                    //PrintNum(num);
                                    for (int j = i + 1; j < numLength; j++) { num[j] = num[i]; }
                                    //Console.Write(":"); PrintNum(num); Console.WriteLine();
                                    direction = directionEnum.undefined;
                                }
                                break;
                        }
                        if (!keepGoing) { break; }      // we are outside the switch so this break takes us out of the loop
                    }

                    if (i == numLength - 1) {  // Did we make it to the end without bouncing?
                        result++;
                        //if (result % 100 == 0) {
                        //    int len = PrintNum(num);
                        //    Console.Write(len + " : " + stopwatch.ElapsedMilliseconds + ", "); stopwatch = Stopwatch.StartNew(); PrintNum(counterArray); Console.WriteLine();
                       //}
/*
                        //for (int k = 0; k < num.Length; k++) { Console.Write(num[k]); } Console.WriteLine();
                        if (result % 100 == 0) {
                            // if it ends in zero, walk backwards to the first non-zero, and replace the zero after it with a 1
                            if (num[numLength - 1] == 0) {
                                //                                Console.Write("Going from "); PrintNum(num);
                                int n = numLength - 1;
                                while (num[n] == 0 && n > 0)
                                {
                                    num[n] = 1; IncrementCounter(counterArray); result++; addOne = false;  // Fill in all the 1's and increment the counter each time. 
                                    n--;
                                }
                                num[n + 1] = 1;
                                Console.Write(" to "); PrintNum(num); Console.WriteLine();
                            }
                            else { Console.WriteLine(); }
                            /* else if (num[numLength - 1] == 9) {
                                //                                Console.Write("Going from "); PrintNum(num);
                                int n = numLength - 1;
                                while (num[n] == 9 && n > 0) {
                                    //num[n] = 0;
                                    n--;
                                }
                                int tmp = num[n] + 1;
                                for (int k = n; k <= numLength - 1; k++) { num[n] = tmp; }
                                //                                Console.Write(" to "); PrintNum(num); Console.WriteLine();
                            }
                        }
*/
                        IncrementCounter(counterArray);
                        //                        if (len != prevLen) {
                        //                            Console.Write(len + " : " + stopwatch.ElapsedMilliseconds + ", "); stopwatch = Stopwatch.StartNew(); PrintNum(counterArray); Console.WriteLine();
                        //                            prevLen = len;
                        //                        }
                    }
                    if (addOne)
                    {
                        int carry = 1;
                        // Add 1 to the number
                        int k;
                        for (k = numLength - 1; k >= 0; k--)
                        {
                            num[k] += carry;
                            if (num[k] <= 9) { break; } else { num[k] = 0; }
                        }
                        if (k < highestDigitIncremented) {
                            highestDigitIncremented = k;
                            Console.WriteLine(numLength - k);
                        }
                        //PrintNum(num); PrintNum(counterArray); Console.WriteLine();
                        //counter++; if (counter % 100000 == 0) { Console.WriteLine(counter); }

                        if (k == -1 && carry == 1)
                        {
                            Console.Write("Array counter = "); PrintNum(counterArray); Console.WriteLine();
                            throw new Exception("Done");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
        private static int PrintNum(int[] num)
        {
            int j = 0;
            while (num[j] == 0) { j++; }
            for (int i = j; i < num.Length; i++)
            {
                Console.Write(num[i]);
            }
            Console.Write(" : ");
            return num.Length - j;
        }
        private static void IncrementCounter(int[] myCounter)
        {
            int carry = 1;
            int numLength = myCounter.Length;
            // Add 1 to the number
            for (int i = numLength - 1; i >= 0; i--)
            {
                myCounter[i] += carry;
                if (myCounter[i] <= 9) { break; } else { myCounter[i] = 0; }
            }
        }
    }
}
