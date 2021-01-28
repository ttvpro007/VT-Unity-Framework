using UnityEngine;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using VT.Utilities;
using System.Text;

public class Kata : MonoBehaviour
{
    #region DRIVER
    void Start()
    {
    }
    #endregion

    #region PROBLEM 1
    const string disemvowelExample = "This website is for losers LOL!";
    /// <summary>
    /// Return a string with vowel removed 
    /// </summary>
    public static string Disemvowel(string str)
    {
        string pattern = "[aiueo]";
        return Regex.Replace(str, pattern, "", RegexOptions.IgnoreCase);
    }
    #endregion

    #region PROBLEM 2
    const string duplicateEncodeExample = "((Success)";
    /// <summary>
    /// Return a string encoded with "(" for unique chars, and ")" for repeated chars
    /// </summary>
    public static string DuplicateEncode(string word)
    {
        return new string(word.ToLower().Select(outterChar => word.ToLower().Count(innerChar => outterChar == innerChar) == 1 ? '(' : ')').ToArray());
    }
    #endregion

    #region PROBLEM 3 - ROMAN ENCODE/DECODE
    const int romanConvertExample = 1;
    static readonly Dictionary<int, string> Base10ToRomNumDict = new Dictionary<int, string>()
    {
        {1000, "M"},
        {900, "CM"},
        {500,  "D"},
        {400, "CD"},
        {100,  "C"},
        {90,  "XC"},
        {50,   "L"},
        {40,  "XL"},
        {10,   "X"},
        {9,   "IX"},
        {5,    "V"},
        {4,   "IV"},
        {1,    "I"},
    };

    /// <summary>
    /// Convert base 10 to Roman numerals
    /// </summary>
    public static string BaseTenToRoman(int num)
    {
        string result = "";

        if (num == 0) return "";

        foreach (var item in Base10ToRomNumDict)
        {
            if (num >= item.Key)
            {
                for (int i = 0; i < num / item.Key; i++)
                {
                    result += item.Value;
                }

                result += BaseTenToRoman(num % item.Key);
                break;
            }
        }

        return result;
    }

    static readonly Dictionary<string, int> RomNumDictToBase10 = new Dictionary<string, int>()
    {
        {"M", 1000},
        {"CM", 900},
        {"D",  500},
        {"CD", 400},
        {"C",  100},
        {"XC",  90},
        {"L",   50},
        {"XL",  40},
        {"X",   10},
        {"IX",   9},
        {"V",    5},
        {"IV",   4},
        {"I",    1},
    };

    public static int RomanToBaseTen(string roman)
    {
        Utils.DebugMethod("public static int Solution(string roman)", "Convert Roman numeral to base 10 number", 3, $"string roman = {roman}");

        int result = 0;
        string copy = roman;
        int chunkSize = 2;

        foreach (var c in copy)
        {
            if (copy.Length == 0) break;
            result += RomanChunksValue(ref copy, Math.Min(copy.Length, chunkSize));
            chunkSize--;
        }

        return result;
    }

    public static int RomanChunksValue(ref string roman, int m)
    {
        string copy = roman;
        int result = 0;
        int removedAmount = 0;

        string chunk = "";

        Utils.DebugMethod($"public static int RomanChunksValue(ref string roman, int m)", "Splits Roman numeral into chunks and return the add up chunks value", 3, $"ref string roman = {roman}", $"int m = {m}");

        Utils.DebugEntry("append", $"Populate window chunk. Do chunk += copy[i] for { m - 0} time(s)", 2);
        for (int i = 0; i < m; i++)
        {
            // chunk += copy[i];
            Utils.DebugCode(() => { chunk += copy[i]; return chunk; }, "chunk += copy[i]", $"{chunk} + ({copy}[{i}]) {copy[i]} -> ", "");
        }

        if (RomNumDictToBase10.ContainsKey(chunk))
        {
            Utils.DebugEntry("search", $"Has (chunk) [{chunk}] in dictionary which translated to (RomNumDictToBase10[{chunk}]) = [{RomNumDictToBase10[chunk]}]", 2);

            // result += RomNumDictToBase10[chunk];
            Utils.DebugCodeEntry(() => { result += RomNumDictToBase10[chunk]; return result; }, "add", "", "result += RomNumDictToBase10[chunk]", $"{result} += {RomNumDictToBase10[chunk]} -> ", "");

            Utils.DebugEntry("remove", $"From index {0} in {roman} remove {m} letters", 2);
            Utils.DebugLabel("code", $"roman = roman.Remove(0, m)", 1);
            Utils.DebugLabel("result", $"(Old) {roman} = ", 1);
            roman = roman.Remove(0, m);
            Utils.DebugText($"(New) {roman}");

            // removedAmount += m;
            Utils.DebugCodeEntry(() => { removedAmount += m; return removedAmount; }, "cache", "", "removedAmount += m", $"{removedAmount} += {m} -> ", "");
        }

        Utils.DebugEntry("sliding window", $"Sliding the window of size {m} in string {copy} for {copy.Length - m} times", 2);
        for (int i = m; i < copy.Length; i++)
        {
            // chunk += copy[i];
            Utils.DebugCode(() => { chunk += copy[i]; return chunk; }, "chunk += copy[i]", $"{chunk} + ({copy}[{i}]) {copy[i]} -> ", "");

            // chunk = chunk.Remove(0, 1);
            Utils.DebugCode(() => { chunk = chunk.Remove(0, 1); return chunk; }, "chunk = chunk.Remove(0, 1)", $"From index {i - m - removedAmount + 1} in {roman} remove {m} letter(s) -> ", "");

            if (RomNumDictToBase10.ContainsKey(chunk))
            {
                Utils.DebugEntry("search", $"Has (chunk) [{chunk}] in dictionary which translated to (RomNumDictToBase10[{chunk}]) = [{RomNumDictToBase10[chunk]}]", 2);

                // result += RomNumDictToBase10[chunk];
                Utils.DebugCodeEntry(() => { result += RomNumDictToBase10[chunk]; return result; }, "add", "", "result += RomNumDictToBase10[chunk]", $"{result} += {RomNumDictToBase10[chunk]} -> ", "");

                Utils.DebugEntry("remove", $"From index {i - m - removedAmount + 1} in {roman} remove {m} letter(s)", 2);
                Utils.DebugLabel("code", $"roman = roman.Remove(i - m - removedAmount + 1, m)", 1);
                Utils.DebugLabel("result", $"(Old) {roman} = ", 1);
                roman = roman.Remove(i - m - removedAmount + 1, m);
                Utils.DebugText($"(New) {roman}", 0);

                Utils.DebugCodeEntry(() => { removedAmount += m; return removedAmount + m; }, "cache", $"Caching the amount of letter removed from parameter roman", $"removedAmount += m", $"{removedAmount} += {m} -> ", "");
            }

            if (i < copy.Length - 1)
            {
                Utils.DebugEntry($"sliding window (continue {i})]", "", 2);
            }
        }

        Utils.DebugEntry("return", $"return result = {result}", 2);
        return result;
    }

    public static int RomanToBaseTen2(string roman)
    {
        int result = 0;

        for (int i = 0; i < roman.Length; i++)
        {
            if (i < roman.Length - 1 && RomNumDictToBase10["" + roman[i]] < RomNumDictToBase10["" + roman[i + 1]])
            {
                result += RomNumDictToBase10["" + roman[i + 1]] - RomNumDictToBase10["" + roman[i]];
                i++;
            }
            else
            {
                result += RomNumDictToBase10["" + roman[i]];
            }
        }

        return result;
    }
    #endregion

    #region PROBLEM 4
    public static bool Narcissistic(int value)
    {
        string valueString = value.ToString();

        // solution 1
        // return valueString.Aggregate(0, (r, c) => r + (int)Math.Pow(Char.GetNumericValue(c), valueString.Length)) == value;

        // solution 2
        return valueString.Sum(c => (int)Math.Pow(Char.GetNumericValue(c), valueString.Length)) == value;
    }
    #endregion

    #region PROBLEM 5
    public static string Rot13(string message)
    {
        // solution 1
        //int shift = 13;
        //string result = string.Empty;

        //foreach (var c in message)
        //{
        //    result += Char.IsUpper(c) ? (char)(LoopNumInRange(65, 90, c + shift)) : (char)(LoopNumInRange(97, 122, c + shift));
        //}

        //return result;

        // solution 2
        //int shift = 13;
        //return message.Aggregate("",
        //    (result, currentChar) =>
        //    {
        //        if (Char.IsLetter(currentChar))
        //        {
        //            result += Char.IsUpper(currentChar) ? GetShiftedUpperCaseLetter(currentChar, shift) : GetShiftedLowerCaseLetter(currentChar, shift);
        //        }
        //        else
        //        {
        //            result += currentChar;
        //        }

        //        return result;
        //    });

        // solution 3
        return string.Join("", message.Select(x => char.IsLetter(x) ? (x >= 65 && x <= 77) || (x >= 97 && x <= 109) ? (char)(x + 13) : (char)(x - 13) : x));
    }

    public static char GetShiftedUpperCaseLetter(char currentChar, int shift)
    {
        return (char)LoopNumInRange('A', 'Z', currentChar + shift);
    }

    public static char GetShiftedLowerCaseLetter(char currentChar, int shift)
    {
        return (char)LoopNumInRange('a', 'z', currentChar + shift);
    }

    public static int LoopNumInRange(int min, int max, int current)
    {
        return (current - min) % (max - min + 1) + min;
    }
    #endregion

    #region PROBLEM 6
    const string s1 = "aretheyhere";
    const string s2 = "yestheyarehere";
    /// <summary>
    /// Return a joined sorted unique chars string 
    /// </summary>
    public static string Longest(string s1, string s2)
    {
        return new string((s1 + s2).Distinct().OrderBy(x => x).ToArray());
    }
    #endregion

    #region PROBLEM 7
    const string accumExample = "test";
    /// <summary>
    /// Return a string with joined words by "-" separator each with capitalize first letter only 
    /// and repeated total x times base on their position in original string 
    /// </summary>
    public static string Accum(string s)
    {
        return string.Join("-", s.Select((x, i) => char.ToUpper(x) + new string(char.ToLower(x), i)));
    }
    #endregion

    #region PROBLEM 8
    readonly int[] maxSequenceExample = { 7, 4, 11, -11, 39, 36, 10, -6, 37, -10, -32, 44, -26, -34, 43, 43 };
    /// <summary>
    /// Return max sum of contiguous elements in array
    /// </summary>
    public static int MaxSequence(int[] arr)
    {
        int max = int.MinValue;
        for (int i = 1; i <= arr.Length; i++)
        {
            max = Math.Max(MaxSequenceRange(arr, i), max);
        }

        return max;
    }

    public static int MaxSequenceRange(int[] arr, int m)
    {
        int max = 0;
        for (int i = 0; i < m; i++)
        {
            max += arr[i];
        }

        int window = max;
        for (int i = m; i < arr.Length; i++)
        {
            window += arr[i] - arr[i - m];
            max = Math.Max(window, max);
        }

        return max;
    }

    public static int MaxSequenceShortened(int[] arr)
    {
        int min = 0, max = 0, sum = 0;

        foreach (var item in arr)
        {
            sum += item;
            min = sum > min ? min : sum;
            max = max > sum - min ? max : sum - min;
        }

        return max;
    }
    #endregion

    #region PROBLEM 9
    const string uniqueInOrderExample = "AAAABBBCCDAABBB";
    /// <summary>
    /// Return a collection that has every adjacent element unique but not necessary unique in whole
    /// </summary>
    public static IEnumerable<T> UniqueInOrder<T>(IEnumerable<T> iterable)
    {
        // solution 1
        //T prev = default;
        //foreach (var curr in iterable)
        //{
        //    if (!curr.Equals(prev))
        //    {
        //        prev = curr;
        //        yield return curr;
        //    }
        //}

        // solution 2
        //List<T> result = iterable.ToList();
        //for (int i = 0; i < result.Count - 1; i++)
        //{
        //    if (result[i].Equals(result[i + 1]))
        //    {
        //        result.RemoveAt(i + 1);
        //        i--;
        //    }
        //}
        //return result;

        // solution 3
        return iterable.Where((x, i) => i == 0 || !x.Equals(iterable.ElementAt(i - 1)));
    }
    #endregion

    #region PROBLEM 10
    const string findOddNumberOutExample = "2 4 7 8 10 a 108 c, a , ";
    /// <summary>
    /// Return the index of the odd one out in a string of numbers
    /// </summary>
    public static int FindOddNumberOut(string numbers)
    {
        // solution 1
        //var trimmedNumbers = numbers.ToIntList();
        //bool isOddOutEven = trimmedNumbers.GroupBy(number => number %= 2).Where(y => y.Count() < 2).Select(z => z.Key).FirstOrDefault() == 0;
        //return trimmedNumbers.Select((element, index) => new { element, index }).First(x => x.element % 2 == (isOddOutEven ? 0 : 1)).index;

        // solution 2
        var ints = numbers.Split(' ').Select(int.Parse).ToList();
        var unique = ints.GroupBy(n => n % 2).OrderBy(c => c.Count()).First().First();
        return ints.FindIndex(c => c == unique) + 1;
    }
    #endregion

    #region PROBLEM 11
    const int ticketPrice = 25;

    public static string Tickets1(int[] peopleInLine)
    {
        Console.Write(string.Join(", ", peopleInLine));

        string result = "";

        var cashBox = new SortedDictionary<int, int>()
        {
            { 25, 0},
            { 50, 0},
            { 100, 0}
        };

        int change = 0, availableChange = 0;

        for (int i = 0; i < peopleInLine.Length; i++)
        {
            change = peopleInLine[i] - ticketPrice;

            if (change == 0)
            {
                cashBox[peopleInLine[i]] += 1;
            }
            else
            {
                var currentLargestAvailableBill = MaxKeyWithCondition(cashBox, x => x.Value > 0 && x.Key <= change);
                var currentSmallestAvailableBill = MinKeyWithCondition(cashBox, x => x.Value > 0 && x.Key <= change);

                while (availableChange < change)
                {
                    if (currentLargestAvailableBill == currentSmallestAvailableBill)
                    {
                        for (int j = 1; j <= cashBox[currentLargestAvailableBill]; j++)
                        {
                            if (change == availableChange + currentLargestAvailableBill * j)
                            {
                                cashBox[currentLargestAvailableBill] -= j;
                                availableChange += currentLargestAvailableBill * j;

                                result = "YES";
                                break;
                            }
                        }

                        if (availableChange != change)
                        {
                            result = "NO";
                            break;
                        }
                    }

                    if (cashBox[currentLargestAvailableBill] > 0)
                    {
                        availableChange += currentLargestAvailableBill;
                        cashBox[currentLargestAvailableBill]--;
                    }
                    else
                    {
                        currentLargestAvailableBill = MaxKeyWithCondition(cashBox, x => x.Key < currentLargestAvailableBill && x.Value > 0);
                    }

                    if (availableChange > change)
                    {
                        cashBox[currentLargestAvailableBill]++;
                        availableChange -= currentLargestAvailableBill;

                        currentLargestAvailableBill = MaxKeyWithCondition(cashBox, x => x.Key < currentLargestAvailableBill && x.Value > 0);
                    }
                }

                if (availableChange == change)
                {
                    availableChange = 0;
                    cashBox[peopleInLine[i]] += 1;
                }
                else
                {
                    result = "NO";
                    break;
                }
            }
        }

        return result;
    }

    private static int MinKeyWithCondition(IDictionary<int, int> source, Func<KeyValuePair<int, int>, bool> selector)
    {
        int min = source.Last().Key;

        foreach (var item in source)
        {
            min = item.Key < min && selector(item) ? item.Key : min;
        }

        return min;
    }

    private static int MaxKeyWithCondition(IDictionary<int, int> source, Func<KeyValuePair<int, int>, bool> selector)
    {
        int max = source.First().Key;

        foreach (var item in source)
        {
            max = item.Key > max && selector(item) ? item.Key : max;
        }

        return max;
    }

    public static string Tickets2(int[] p)
    {
        // solution 1
        //int m25 = 0, m50 = 0;
        //for (int i = 0; i < p.Length & m25 >= 0; i++)
        //{
        //    m25 += (p[i] == 25 ? 25 : p[i] == 50 ? -25 : m50 < 50 ? -75 : -25);
        //    m50 += (p[i] == 25 ? 0 : p[i] == 50 ? 50 : m50 < 50 ? 0 : -50);
        //}
        //return m25 < 0 ? "NO" : "YES";

        // solution 2
        int[] bills = { 0, 0, 0 };

        foreach (var v in p)
        {
            switch (v)
            {
                case 25:
                    bills[0] += 1;
                    break;
                case 50:
                    bills[0] -= 1;
                    bills[1] += 1;
                    break;
                case 100 when bills[1] > 0:
                    bills[0] -= 1;
                    bills[1] -= 1;
                    break;
                case 100 when bills[1] == 0:
                    bills[0] -= 3;
                    break;
            }

            if (bills[0] < 0 || bills[1] < 0 || bills[2] < 0) return "NO";
        }

        return "YES";
    }

    public static int GetUnique(IEnumerable<int> numbers)
    {
        return numbers.GroupBy(x => x).Where(y => y.Count() == 1).Select(z => z.Key).FirstOrDefault();
    }
    #endregion

    #region PROBLEM 12
    public static long[] SumDigPow(long a, long b)
    {
        long[] result = new long[b - a];
        int index = 0;

        for (int i = 0; i <= result.Length; i++)
        {
            if (CheckValidSumDigPow((int)a + i))
            {
                result[index] = a + i;
                index++;
            }
        }

        Array.Resize(ref result, index + 1);

        return result;
    }

    public static bool CheckValidSumDigPow(int value)
    {
        string valueString = value.ToString();
        int i = 0;
        return valueString.Sum(c => Math.Pow(Char.GetNumericValue(c), ++i)) == value;
    }
    #endregion

    #region PROBLEM 13 TORTOISE HARE LOOP
    public static int GetLoopSize_Floyd(LoopDetector.Node tortoise, LoopDetector.Node hare)
    {
        int size = 1;

        while ((tortoise = tortoise.next) != null && (hare = hare.next) != null && tortoise != hare)
        {
            size++;
        }

        return size;
    }

    public static int GetStartLoopNodeIndex_Floyd(LoopDetector.Node startNode, LoopDetector.Node hare)
    {
        LoopDetector.Node tortoise = startNode;
        int index = 0;

        while ((tortoise = tortoise.next) != null && tortoise != hare)
        {
            index++;
        }

        return index;
    }

    public static bool HasLoop_Floyd(LoopDetector.Node startNode)
    {
        LoopDetector.Node tortoise = startNode, hare = startNode;

        // find meeting point x_i = x_2i
        while ((tortoise = tortoise.next) != null && (hare = hare.next.next) != null && tortoise != hare)
        {
            return true;
        }

        return false;
    }

    public static int GetLoopSize_Brent(LoopDetector.Node startNode)
    {
        LoopDetector.Node tortoise = startNode, hare = startNode.next;
        int size = 1, pow = 1;

        while (tortoise != hare)
        {
            if (pow == size)
            {
                tortoise = hare;
                pow *= 2;
                size = 0;
            }

            hare = hare.next;
            size += 1;
        }

        return size;
    }
    #endregion

    #region PROBLEM 14
    public static int SumIntervals((int, int)[] intervals)
    {
        if (intervals.Length <= 0) return 0;

        var sortedList = intervals.OrderBy(x => x.Item1).ToList();
        var mergedIntervals = new List<(int, int)>();

        mergedIntervals.Add(sortedList[0]);
        for (int i = 1; i < sortedList.Count; i++)
        {
            var interval = mergedIntervals[mergedIntervals.Count - 1];

            if (IsIntervalOverlapping(interval, sortedList[i]))
            {
                mergedIntervals.RemoveAt(mergedIntervals.Count - 1);
                mergedIntervals.Add(MergeIntervals(interval, sortedList[i]));
            }
            else
            {
                mergedIntervals.Add(sortedList[i]);
            }
        }

        int total = 0;
        foreach (var item in mergedIntervals)
        {
            total += item.Item2 - item.Item1;
        }

        return total;
    }

    public static bool IsIntervalOverlapping((int, int) first, (int, int) second)
    {
        return first.Item2 >= second.Item1;
    }

    public static (int, int) MergeIntervals((int, int) first, (int, int) second)
    {
        return (first.Item1, first.Item2 > second.Item2 ? first.Item2 : second.Item2);
    }

    public static int SumIntervals2((int, int)[] intervals)
    {
        return intervals
         .SelectMany(i => Enumerable.Range(i.Item1, i.Item2 - i.Item1))
         .Distinct()
         .Count();
    }
    #endregion

    #region PROBLEM 15
    public static Dictionary<string, int> ParseMolecule(string formula)
    {
        // solution 1
        //var factor = new Stack<int>(); factor.Push(1); factor.Push(1);
        //var result = new Dictionary<string, int>();
        //var lexems = Regex.Matches(formula, @"(?<e>[A-Z][a-z]*)|(?<n>\d+)|(?<l>[\[({])|(?<r>[\])}])");

        //foreach (var lexem in lexems.Cast<Match>().Reverse())
        //{
        //    if (lexem.Groups["n"].Success)
        //    {
        //        factor.Push(factor.Pop() * int.Parse(lexem.Value));
        //    }

        //    if (lexem.Groups["l"].Success)
        //    {
        //        factor.Pop();
        //        factor.Pop();
        //        factor.Push(factor.Peek());
        //    }

        //    if (lexem.Groups["r"].Success)
        //    {
        //        factor.Push(factor.Peek());
        //    }

        //    if (lexem.Groups["e"].Success)
        //    {
        //        result[lexem.Value] = factor.Pop() + result.SafeGetValue(lexem.Value);
        //        factor.Push(factor.Peek());
        //    }
        //}

        //return result;

        // solution 2
        //var molecules = new Dictionary<string, int>();

        //formula = new string(formula.Reverse().ToArray());

        //var multipliers = new Dictionary<int, int>
        //{
        //    [0] = 1
        //};

        //int depth = 0;
        //string count = "1";
        //string symbol = "";
        //string suffix = "";

        //for (int i = 0; i < formula.Length; i++)
        //{
        //    char c = formula[i];

        //    switch (c)
        //    {
        //        case ']':
        //        case ')':
        //        case '}':
        //            depth++;
        //            multipliers[depth] = int.Parse(count) * multipliers[depth - 1];
        //            count = "1";
        //            break;
        //        case '[':
        //        case '(':
        //        case '{':
        //            depth--;
        //            break;
        //        default:

        //            if (char.IsLower(c))
        //            {
        //                suffix = c.ToString() + suffix;
        //            }
        //            else if (char.IsUpper(c))
        //            {
        //                symbol = c + suffix;
        //                suffix = "";

        //                if (!molecules.ContainsKey(symbol))
        //                    molecules[symbol] = 0;

        //                molecules[symbol] += int.Parse(count) * multipliers[depth];
        //                count = "1";
        //            }
        //            else if (char.IsNumber(c))
        //            {
        //                if (i > 0 && char.IsNumber(formula[i - 1]))
        //                    count = c.ToString() + count.ToString();
        //                else
        //                    count = c.ToString();
        //            }
        //            break;
        //    }
        //}
        //return molecules;

        // solution 3
        formula = Regex.Replace(formula, @"[{[]", "(");
        formula = Regex.Replace(formula, @"\}|\]", ")");
        formula = Regex.Replace(formula, @"([A-Z][a-z]*|\))(?!\d|[a-z])", m => m.Value + "1");

        while (Regex.IsMatch(formula, @"\("))
            formula = Regex.Replace(formula, @"\(([A-Za-z0-9]+)\)(\d+)", m =>
                 Regex.Replace(m.Groups[1].ToString(), @"(?<=[a-zA-Z])(\d+)", (x) =>
                     (int.Parse(x.Value) * int.Parse(m.Groups[2].ToString())).ToString())
            );

        return Regex.Split(formula, @"(?<!^)(?=[A-Z])").
                  Aggregate(new Dictionary<string, int>(), (s, n) => {
                      string[] tokens = Regex.Split(n, @"(?<=[A-Za-z])(?=\d)");
                      if (s.ContainsKey(tokens[0]))
                          s[tokens[0]] += int.Parse(tokens[1]);
                      else s[tokens[0]] = int.Parse(tokens[1]);
                      return s;
                  }
                  );
    }
    #endregion

    #region PROBLEM 16
    #endregion
    #region PROBLEM 17
    #endregion
    #region PROBLEM 18
    #endregion
    #region PROBLEM 19
    #endregion
    #region PROBLEM 20
    #endregion
    #region PROBLEM 21
    #endregion
    #region PROBLEM 22
    #endregion
    #region PROBLEM 23
    #endregion
    #region PROBLEM 24
    #endregion
    #region PROBLEM 25
    #endregion
    #region MISC
    void merge(int[] arr, int l, int m, int r)
    {
        int n1 = m - l + 1;
        int n2 = r - m;

        // Create temp arrays
        int[] L = new int[n1], R = new int[n2];

        // Copy data to temp arrays L[] and R[]
        for (int x = 0; x < n1; x++)
            L[x] = arr[l + x];
        for (int y = 0; y < n2; y++)
            R[y] = arr[m + 1 + y];

        // Merge the temp arrays back into arr[l..r]

        // Initial index of first subarray
        int i = 0;

        // Initial index of second subarray
        int j = 0;

        // Initial index of merged subarray
        int k = l;

        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
            {
                arr[k] = L[i];
                i++;
            }
            else
            {
                arr[k] = R[j];
                j++;
            }
            k++;
        }

        // Copy the remaining elements of
        // L[], if there are any
        while (i < n1)
        {
            arr[k] = L[i];
            i++;
            k++;
        }

        // Copy the remaining elements of
        // R[], if there are any
        while (j < n2)
        {
            arr[k] = R[j];
            j++;
            k++;
        }
    }

    // l is for left index and r is
    // right index of the sub-array
    // of arr to be sorted */
    void MergeSort(int[] arr, int l, int r)
    {
        if (l >= r)
        {
            return;//returns recursively
        }
        int m = (l + r - 1) / 2;
        MergeSort(arr, l, m);
        MergeSort(arr, m + 1, r);
        merge(arr, l, m, r);
    }
    #endregion
}

public static class Extension
{
    // Problem 15
    public static V SafeGetValue<K, V>(this Dictionary<K, V> dict, K key, V value = default(V))
        => dict.ContainsKey(key) ? dict[key] : value;

    public static List<int> ToIntList(this string numberString)
    {
        List<int> trimmedNumbers = new List<int>();

        numberString.Split(' ')
            .ToList()
            .ForEach(ss =>
            {
                if (int.TryParse(ss, out int num))
                {
                    trimmedNumbers.Add(num);
                }
            });

        return trimmedNumbers;
    }
}