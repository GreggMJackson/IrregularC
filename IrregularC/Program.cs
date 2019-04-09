using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace IrregularC {
    public class Program {
        public static void Main1(string[] args) {
            string pattern = "(Mr\\.? |Mrs\\.? |Miss |Ms\\.? )";
            string[] names = { "Mr. Henry Hunt", "Ms. Sara Samuels",
                         "Abraham Adams", "Ms. Nicole Norris" };
            foreach (string name in names)
                Console.WriteLine(Regex.Replace(name, pattern, String.Empty));
        }
        public static void Main2(string[] args) {
            string pattern = @"\b(\w+?)\s\1\b";
            string input = "This this is a nice day. What about this? This tastes good. I saw a a dog.";
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                Console.WriteLine("{0} (duplicates '{1}') at position 2", match.Value, match.Groups[1].Value, match.Index);
            Console.ReadLine();
        }
        public static void Main3() {
            //Define text to be parsed.
            string input = "Office expenses on 2/13/2008:\n" +
                           "Paper (500 sheets)                 £3.95\n" +
                           "Pencils (box of 10)                £1.00\n" +
                           "Pen (box of 10)                    £4.49\n" +
                           "Erasers                            £2.19\n" +
                           "Ink jet printer                   £69.95\n" +
                           "Total expenses                   £ 81.58\n";

            // Get current culture's NumberFormatInfo object.
            NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
            //Assign needed property values to variables.
            string currencySymbol = nfi.CurrencySymbol;
            bool symbolPrecedesIfPositive = nfi.CurrencyPositivePattern % 2 == 0;
            string groupSeparator = nfi.CurrencyGroupSeparator;
            string decimalSeparator = nfi.CurrencyDecimalSeparator;

            // Form Regular expresssion pattern.
            string pattern = Regex.Escape(symbolPrecedesIfPositive ? currencySymbol : "") +
                             @"\s*[-+]?" + "([0-9]{0,3}(" + groupSeparator + "[0-9]{3})*(" +
                             Regex.Escape(decimalSeparator) + "[0-9]+)?)" +
                             (!symbolPrecedesIfPositive ? currencySymbol : "");
            Console.WriteLine("The regular expression is:");
            Console.WriteLine("    " + pattern);

            // Get text that matches the regular expression pattern.
            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnorePatternWhitespace);
            Console.WriteLine($"Found {matches.Count} matches.");

            // Get numeric string, convert it to a value, and add it to list object.
            List<decimal> expenses = new List<decimal>();

            foreach (Match match in matches)
                expenses.Add(Decimal.Parse(match.Groups[1].Value));

            // Determine whether total is present and if present, whether it is correct.
            decimal total = 0;
            foreach (decimal value in expenses)
                total += value;

            if (total / 2 == expenses[expenses.Count - 1])
                Console.WriteLine("The expenses total {0:C2}.", expenses[expenses.Count - 1]);
            else
                Console.WriteLine("The expenses total {0:C2}.", total);
            Console.ReadLine();
        }
        public static void Main4() {
            string delimited = @"\G(.+)[\t\u007c](.+)\r?\n";
            string input = "Mumbai, India|13,922,125\t\n" +
                           "Shanghai, China\t13,831,900\n" +
                           "Karachi, Pakistan|12,991,000\n" +
                           "Delhi, India\t12,259,230\n" +
                           "Istanbul, Turkey|11,372,613\n";
            Console.WriteLine("Population of the world's largest cities, 2009");
            Console.WriteLine();
            Console.WriteLine("{0,-20} {1,10}", "City", "Population");
            Console.WriteLine();
            foreach (Match match in Regex.Matches(input, delimited))
                Console.WriteLine("{0,-20} {1,10}", match.Groups[1].Value, match.Groups[2].Value);
            Console.ReadLine();
        }
        public static void Main5() {
            string pattern = @"gr[ae]y\s\S+?[\s\p{P}]";
            string input = "The gray wolf jumped over the grey wall.";
            MatchCollection matches = Regex.Matches(input, pattern);
            foreach (Match match in matches)
                Console.WriteLine($"'{match.Value}'");
            Console.ReadLine();
        }
        public static void Main6() {
            string pattern = @"\b[A-Z]\w*\b";
            string input = "A city Albany Zulu maritime Marseilles";
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine(match.Value);
            Console.ReadLine();
        }
        public static void Main7() {
            string pattern = @"\bth[^o]\w+\b";
            string input = "thought thing though them through thus thorough this";
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine(match.Value);
            Console.ReadLine();
        }
        public static void Main8() {
            string pattern = "^.+";
            string input = "This is one line and" + Environment.NewLine + "this is the second.";
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine(Regex.Escape(match.Value));

            Console.WriteLine();
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.Singleline))
                Console.WriteLine(Regex.Escape(match.Value));
            Console.ReadLine();
        }
        public static void Main9() {
            string pattern = @"\b.*[.?!;:](\s|\z)";
            string input = "this. what: is? go, thing.";
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine(match.Value);
            Console.ReadLine();
        }
        public static void Main10() {
            string pattern = @"\b(\p{IsGreek}+(\s)?)+\p{Pd}\s(\p{IsBasicLatin}+(\s)?)+";
            string input = "Κατα Μαθθαίον - The Gospel of Matthew";
            Console.WriteLine(Regex.IsMatch(input, pattern));
            Console.ReadLine();
        }
        public static void Main11() {
            string pattern = @"(\P{Sc})+";
            string[] values = { "$164,091.78", "£1,073,142.68", "73¢", "€120" };
            foreach (string value in values)
                Console.WriteLine(Regex.Match(value, pattern).Value);
            Console.ReadLine();
        }
        public static void Main12() {
            string pattern = @"(\w)\1";
            string[] words = { "trellis", "seer", "latter", "summer", "hoarse", "lesser", "aardvark", "stunned" };
            foreach (string word in words) {
                Match match = Regex.Match(word, pattern);
                if (match.Success)
                    Console.WriteLine("'{0}' found in '{1}' at position {2}.", match.Value, word, match.Index);
                else
                    Console.WriteLine("No double characters in '{0}'", word);
            }
            Console.ReadLine();
        }
        public static void Main13() {
            string pattern = @"\b(\w+)(\W){1,2}";
            string input = "The old, grey mare slowly walked across the narrow, green pasture.";
            foreach (Match match in Regex.Matches(input, pattern)) {
                Console.WriteLine(match.Value);
                Console.Write("    Non-word character(s):");
                CaptureCollection captures = match.Groups[2].Captures;
                for (int ctr = 0; ctr < captures.Count; ctr++)
                    Console.Write(@"'{0}' (\u{1}){2}",
                        captures[ctr].Value,
                        Convert.ToUInt16(captures[ctr].Value[0]).ToString("X4"),
                        ctr < captures.Count - 1 ? ", " : "");
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        public static void Main14() {
            string pattern = @"\b\w+(e)?s(\s|$)";
            string input = "matches stores stops leave leaves";
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine(match.Value);
            Console.ReadLine();
        }
        public static void Main15() {
            string pattern = @"\b(\S+)\s?";
            string input = "This is the first sentence of the first paragraph. " +
                "This is the second sentence.\n" +
                "This is the only sentence of the second paragraph.";
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine(match.Groups[1]);
            Console.ReadLine();
        }
        public static void Main16() {
            string pattern = @"^(\(?\d{3}\)?[\s-])?\d{3}-\d{4}$";
            string[] inputs = { "111 111-1111", "222-2222", "222 333-444", "(212) 111-1111", "111-AB1-1111", "212-111-1111", "01 999-9999" };
            foreach (string input in inputs) {
                if (Regex.IsMatch(input, pattern))
                    Console.WriteLine(string.Format("{0,-15}", input) + ": matched");
                else
                    Console.WriteLine(string.Format("{0,-15}", input) + ": match failed");
            }
            Console.ReadLine();
        }
        public static void Main17() {
            string pattern = @"^\D\d{1,5}\D*$";
            string[] inputs = { "A1039C", "AA0001", "C18A", "Y938518" };
            foreach (string input in inputs) {
                if (Regex.IsMatch(input, pattern))
                    Console.WriteLine(string.Format("{0,-15} : matched", input));
                else
                    Console.WriteLine(string.Format("{0,-15} : match failed", input));
            }
            Console.ReadLine();
        }
        public static void Main18() {
            char[] chars = { 'a', 'X', '8', ',', ' ', '\u0009', '!' };
            foreach (char ch in chars)
                Console.WriteLine("'{0}': {1}", Regex.Escape(ch.ToString()), Char.GetUnicodeCategory(ch));
            Console.ReadLine();
        }
        public static void Main19() {
            string[] inputs = { "123", "1357953", "3557798", "335599901" };
            string pattern = @"^[0-9-[2468]]+$";
            foreach (string input in inputs) {
                Match match = Regex.Match(input, pattern);
                if (match.Success)
                    Console.WriteLine(match.Value);
            }
            Console.ReadLine();
        }
        public static void Main20() {
            int startPos = 0, endPos = 70;
            string cr = Environment.NewLine;
            string input = "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957" + cr +
                           "Chicago Cubs, National League, 1903-present" + cr +
                           "Detroit Tigers, American League, 1901-present" + cr +
                           "New York Giants, National League, 1885-1957" + cr +
                           "Washington Senators, American League, 1901-1960" + cr;
            string pattern = @"^((\w+(\s?)){2,}),\s(\w+\s\w+),(\s\d{4}(-(\d{4}|present))?,?)+";
            Match match;

            if (input.Substring(startPos, endPos).Contains(",")) {
                match = Regex.Match(input, pattern);
                while (match.Success) {
                    Console.Write("The {0} played in the {1} in", match.Groups[1].Value, match.Groups[4].Value);
                    foreach (Capture capture in match.Groups[5].Captures)
                        Console.Write(capture.Value);
                    Console.WriteLine(".");
                    startPos = match.Index + match.Length;
                    endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
                    if (!input.Substring(startPos, endPos).Contains(",")) break;
                    match = match.NextMatch();
                }
                Console.WriteLine();
            }

            if (input.Substring(startPos, endPos).Contains(",")) {
                match = Regex.Match(input, pattern, RegexOptions.Multiline);
                while (match.Success) {
                    Console.Write("The {0} played in the {1} in", match.Groups[1].Value, match.Groups[4].Value);
                    foreach (Capture capture in match.Groups[5].Captures)
                        Console.Write(capture.Value);
                    Console.WriteLine(".");
                    startPos = match.Index + match.Length;
                    endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
                    if (!input.Substring(startPos, endPos).Contains(",")) break;
                    match = match.NextMatch();
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        public static void Main21() {
            int startPos = 0, endPos = 70;
            string cr = Environment.NewLine;
            string input = "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957" + cr +
                           "Chicago Cubs, National League, 1903-present" + cr +
                           "Detroit Tigers, American League, 1901-present" + cr +
                           "New York Giants, National League, 1885-1957" + cr +
                           "Washington Senators, American League, 1901-1960" + cr;
            Match match;

            string basePattern = @"^((\w+(\s?)){2,}),\s(\w+\s\w+),(\s\d{4}(-(\d{4}|present))?,?)+";
            string pattern = basePattern + "$";
            Console.WriteLine("Attempting to match the entire input string:");
            if (input.Substring(startPos, endPos).Contains(",")) {
                match = Regex.Match(input, pattern);
                while (match.Success) {
                    Console.Write("The {0} played in the {1} in", match.Groups[1].Value, match.Groups[4].Value);
                    foreach (Capture capture in match.Groups[5].Captures)
                        Console.Write(capture.Value);
                    Console.WriteLine(".");
                    startPos = match.Index + match.Length;
                    endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
                    if (!input.Substring(startPos, endPos).Contains(",")) break;
                    match = match.NextMatch();
                }
                Console.WriteLine();
            }

            string[] teams = input.Split(new string[] { cr }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine("Attempting to match each element in a string array:");
            foreach (String team in teams) {
                if (team.Length > 70) continue;
                match = Regex.Match(team, pattern);
                if (match.Success) {
                    Console.Write("The {0} played in the {1} in", match.Groups[1].Value, match.Groups[4].Value);
                    foreach (Capture capture in match.Groups[5].Captures)
                        Console.Write(capture.Value);
                    Console.WriteLine(".");
                }
            }
            Console.WriteLine();

            startPos = 0;
            endPos = 70;
            Console.WriteLine("Attempting to match each line of an input string with '$':");
            if (input.Substring(startPos, endPos).Contains(",")) {
                match = Regex.Match(input, pattern, RegexOptions.Multiline);
                while (match.Success) {
                    Console.Write("The {0} played in the {1} in", match.Groups[1].Value, match.Groups[4].Value);
                    foreach (Capture capture in match.Groups[5].Captures)
                        Console.Write(capture.Value);
                    Console.WriteLine(".");
                    startPos = match.Index + match.Length;
                    endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
                    if (!input.Substring(startPos, endPos).Contains(",")) break;
                    match = match.NextMatch();
                }
                Console.WriteLine();
            }

            startPos = 0;
            endPos = 70;
            pattern = basePattern + "\r?$";
            Console.WriteLine(@"Attempting to match each line of an input string with '\r?$':");
            if (input.Substring(startPos, endPos).Contains(",")) {
                match = Regex.Match(input, pattern, RegexOptions.Multiline);
                while (match.Success) {
                    Console.Write("The {0} played in the {1} in", match.Groups[1].Value, match.Groups[4].Value);
                    foreach (Capture capture in match.Groups[5].Captures)
                        Console.Write(capture.Value);
                    Console.WriteLine(".");
                    startPos = match.Index + match.Length;
                    endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
                    if (!input.Substring(startPos, endPos).Contains(",")) break;
                    match = match.NextMatch();
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        public static void Main22() {
            int startPos = 0, endPos = 70;
            string cr = Environment.NewLine;
            string input = "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957" + cr +
                           "Chicago Cubs, National League, 1903-present" + cr +
                           "Detroit Tigers, American League, 1901-present" + cr +
                           "New York Giants, National League, 1885-1957" + cr +
                           "Washington Senators, American League, 1901-1960" + cr;
            string pattern = @"\A((\w+(\s?)){2,}),\s(\w+\s\w+),(\s\d{4}(-(\d{4}|present))?,?)+";
            Match match;

            if (input.Substring(startPos, endPos).Contains(",")) {
                match = Regex.Match(input, pattern, RegexOptions.Multiline);
                while (match.Success) {
                    Console.Write("The {0} played in the {1} in", match.Groups[1].Value, match.Groups[4].Value);
                    foreach (Capture capture in match.Groups[5].Captures)
                        Console.Write(capture.Value);
                    Console.WriteLine(".");
                    startPos = match.Index + match.Length;
                    endPos = startPos + 70 <= input.Length ? 70 : input.Length - startPos;
                    if (!input.Substring(startPos, endPos).Contains(",")) break;
                    match = match.NextMatch();
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        public static void Main23() {
            string[] inputs = { "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957",
                                "Chicago Cubs, National League, 1903-present" + Environment.NewLine,
                                "Detroit Tigers, American League, 1901-present" + Regex.Unescape(@"\n") ,
                                "New York Giants, National League, 1885-1957" ,
                                "Washington Senators, American League, 1901-1960" + Environment.NewLine};
            string pattern = @"^((\w+(\s?)){2,}),\s(\w+\s\w+),(\s\d{4}(-(\d{4}|present))?,?)+\r?\Z";
            foreach (string input in inputs) {
                if (input.Length > 70 || !input.Contains(",")) continue;
                Console.WriteLine(Regex.Escape(input));
                Match match = Regex.Match(input, pattern);
                if (match.Success)
                    Console.WriteLine("    Match succeeded");
                else
                    Console.WriteLine("    Match failed.");
            }
            Console.ReadLine();
        }
        public static void Main24() {
            string[] inputs = { "Brooklyn Dodgers, National League, 1911, 1912, 1932-1957",
                                "Chicago Cubs, National League, 1903-present" + Environment.NewLine,
                                "Detroit Tigers, American League, 1901-present" + Regex.Unescape(@"\n") ,
                                "New York Giants, National League, 1885-1957" ,
                                "Washington Senators, American League, 1901-1960" + Environment.NewLine};
            string pattern = @"^((\w+(\s?)){2,}),\s(\w+\s\w+),(\s\d{4}(-(\d{4}|present))?,?)+\r?\z";
            foreach (String input in inputs) {
                if (input.Length > 70 || !input.Contains(",")) continue;
                Console.WriteLine(Regex.Escape(input));
                Match match = Regex.Match(input, pattern);
                if (match.Success)
                    Console.WriteLine("    Match succeeded.");
                else
                    Console.WriteLine("    Match failed.");
            }
            Console.ReadLine();
        }
        public static void Main25() {
            string input = "capybara,squirrel,chipmunk,porcupine,gopher," +
                           "beaver,groundhog,hamster,guinea pig,gerbil," +
                           "chinchilla,prairie dog,mouse,rat";
            string pattern = @"\G(\w+\s?\w*),?";
            Match match = Regex.Match(input, pattern);
            while (match.Success) {
                Console.WriteLine(match.Groups[1].Value);
                match = match.NextMatch();
            }
            Console.ReadLine();
        }
        public static void Main26() {
            string input = "area bare arena mare";
            string pattern = @"\bare\w*\b";
            Console.WriteLine("Words that begin with 'are':");
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine("'{0}' found at position {1}", match.Value, match.Index);
            Console.ReadLine();
        }
        public static void Main27() {
            string input = "equity queen equip acquaint quiet";
            string pattern = @"\Bqu\w+";
            foreach (Match match in Regex.Matches(input, pattern))
                Console.WriteLine("'{0}' found at position {1}", match.Value, match.Index);
            Console.ReadLine();
        }
        public static void Main28() {
            string pattern = @"(\w+)\s(\1)";
            string input = "He said that that was the the correct answer.";
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                Console.WriteLine("Duplicate '{0}' found at positions {1} and {2},",
                                  match.Groups[1].Value,
                                  match.Groups[1].Index,
                                  match.Groups[2].Index);
            Console.ReadLine();
        }
        public static void Main29() {
            string pattern = @"(?<duplicateWord>\w+)\s\k<duplicateWord>\W(?<nextWord>\w+)";
            string input = "He said that that was the the correct answer.";
            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                Console.WriteLine("A duplicate '{0}' as position {1} followed by '{2}'.",
                                  match.Groups["duplicateWord"].Value,
                                  match.Groups["duplicateWord"].Index,
                                  match.Groups["nextWord"].Value);
            Console.ReadLine();
        }
        public static void Main30() {
            string pattern = @"\D+(?<digit>\d+)\D+(?<digit>\d+)?";
            string[] inputs = { "abc123def456", "abc123def" };
            foreach (var input in inputs) {
                Match m = Regex.Match(input, pattern);
                if (m.Success) {
                    Console.WriteLine($"Match: {m.Value}");
                    for (int i = 1; i < m.Groups.Count; i++) {
                        Group g = m.Groups[i];
                        Console.WriteLine($"Group {i}: {g.Value}");
                        for (int c = 0; c < g.Captures.Count; c++)
                            Console.WriteLine($"    Capture {c}: {g.Captures[c].Value}");
                    }
                } else {
                    Console.WriteLine("The match failed.");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
        public static void Main31() {
            string pattern = "^[^<>]*" +
                             "(" +
                             "((?'Open'<)[^<>]*)+" +
                             "((?'Close-Open'>)[^<>]*)+" +
                             ")*" +
                             "(?(Open)(?!))$";
            string input = "<abc><mno<xyz>>";

            // Group 0 - Input string
            // Group 1 - 1st level things (plus brackets)
            // Group 2 - Openings
            // Group 3 - Closing (bracket(s))
            // Group 4 
            // Group 5 - Contents between every matching bracket pair.

            Match m = Regex.Match(input, pattern);
            if (m.Success) {
                Console.WriteLine("Input: \"{0}\" \nMatch: \"{1}\"", input, m);
                int g = 0;
                foreach (Group grp in m.Groups) {
                    Console.WriteLine("    Group {0}: {1}", g, grp.Value);
                    g++;
                    int c = 0;
                    foreach (Capture cap in grp.Captures) {
                        Console.WriteLine("     Capture {0}: {1}", c, cap.Value);
                        c++;
                    }
                }
            } else {
                Console.WriteLine("Match failed.");
            }
            Console.ReadLine();
        }
        public static void Main32() {
            string pattern = @"(?:\b(?:\w+)\W*)+\.";
            string input = "This is a short sentence.";
            Match match = Regex.Match(input, pattern);
            Console.WriteLine("Match: {0}", match.Value);
            for (int i = 1; i < match.Groups.Count; i++)
                Console.WriteLine("    Group {0}: {1}", i, match.Groups[i].Value);
            Console.ReadLine();
        }

        public static void Main33() {
            string pattern = @"\b(?ix: d \w+)\s";
            string input = "Dogs are decidedly good pets.";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}'// found at index {1}.", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main34() {
            string pattern = @"\b\w+(?=\sis\b)";
            string[] inputs = { "The dog is a Malamute.",
                                "The island has beautiful birds.",
                                "The pitch missed home plate.",
                                "Sunday is a weekend day." };
            foreach (string inputb in inputs) {
                Match m = Regex.Match(inputb, pattern);
                if (m.Success)
                    Console.WriteLine("'{0}' precedes 'is'.", m.Value);
                else
                    Console.WriteLine("'{0}' does not match the pattern.", inputb);
            }
        }
        public static void Main35() {
            string pattern = @"\b(?!un)\w+\b";
            string input = "unite one unethical ethics use untie ultimate";
            //foreach(Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
            //    Console.WriteLine(match.Value);
            Regex.Matches(input, pattern, RegexOptions.IgnoreCase).ToList().ForEach(m => Console.WriteLine(m.Value));
        }
        public static void Main36() {
            string pattern = @"\b\w+\b(?!\p{P})";
            string input = "Disconnected, disjointed thoughts in a sentence fragment.";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine(m.Value));
        }
        public static void Main37() {
            string input = "2010 1999 1861 2140 2009";
            string pattern = @"(?<=\b20)\d{2}\b";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine(m.Value));
        }
        public static void Main38() {
            string[] dates = { "Monday February 1, 2010",
                               "Wednesday February 3, 2010",
                               "Saturday February 6, 2010",
                               "Sunday February 7, 2010",
                               "Monday, February 8, 2010"};
            string pattern = @"(?<!(Saturday|Sunday) )\b\w+ \d{1,2}, \d{4}\b";
            dates.ToList().ForEach(dv => {
                var m = Regex.Match(dv, pattern);
                if (m.Success) Console.WriteLine(m.Value);
            });
            Console.ReadLine();
        }
        public static void Main39() {
            string[] inputs = { "cccd.", "aaad", "aaaa" };
            string back = @"(\w)\1+.\b";
            string noback = @"(?>(\w)\1+).\b";
            foreach (string input in inputs) {
                Match m1 = Regex.Match(input, back);
                Match m2 = Regex.Match(input, noback);
                Console.WriteLine("{0}: ", input);

                Console.WriteLine("   Backtracking : ");
                if (m1.Success)
                    Console.WriteLine(m1.Value);
                else
                    Console.WriteLine("No match.");

                Console.WriteLine("    Nonbacktracking : ");
                if (m2.Success)
                    Console.WriteLine(m2.Value);
                else
                    Console.WriteLine("No match.");
            }
            Console.ReadLine();
        }
        public static void Main40() {
            string pattern = @"(\b(\w+)\W+)+";
            string input = "This is a short sentence.";
            Match match = Regex.Match(input, pattern);
            Console.WriteLine("Match: '{0}'", match.Value);
            int i = 0;
            match.Groups.Skip(1).ToList().ForEach(grp => {
                Console.WriteLine("   Group {0}: '{1}'", ++i, grp.Value);
                int c = 0;
                grp.Captures.ToList().ForEach(cap => {
                    Console.WriteLine("     Capture {0}: '{1}'", c++, cap.Value);
                });
            });
            Console.ReadLine();
        }
        public static void Main41() {
            string pattern = @"\b91*9*\b";
            string input = "99 95 919 929 9119 9219 999 9919 91119";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main42() {
            string pattern = @"\ban+\w*?\b";
            string input = "Autumn is a great time for an annual announcement to all antique collectors.";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main43() {
            string pattern = @"\ban?\b";
            string input = "An amiable animal with a large snout and an animated nose.";
            Regex.Matches(input, pattern, RegexOptions.IgnoreCase).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main44() {
            string pattern = @"\b\d+\,\d{3}\b";
            string input = "Sales totalled 103,524 million in January," +
                           "106,971 million in February, but only" +
                           "943 million in March.";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}.", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main45() {
            string pattern = @"\b\d{2,}\b\D+";
            string input = "7 days, 10 weeks, 300 years";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main46() {
            string pattern = @"(00\s){2,4}";
            string input = "0x00 FF 00 00 18 17 FF 00 00 00 21 00 00 00 00 00";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main47() {
            string pattern = @"\b\w*?oo\w*?\b";
            string input = "woof root root rob oof woo woe";
            Regex.Matches(input, pattern, RegexOptions.IgnoreCase).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main48() {
            string pattern = @"\b\w+?\b";
            string input = "Aa Bb Cc Dd Ee Ff";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main49() {
            string pattern = @"^\s*(System.)?? Console.write(Line)??\(??";
            string input = "System.Console.WriteLine(\"Hello!\")\n" +
                                  "Console.Write(\"Hello!\")\n" +
                                  "Console.WriteLine(\"Hello!\")\n" +
                                  "Console.ReadLine()\n" +
                                  "   Console.WriteLine";
            Regex.Matches(input, pattern, RegexOptions.IgnorePatternWhitespace |
                                          RegexOptions.IgnoreCase |
                                          RegexOptions.Multiline
                                          ).ToList().ForEach(m =>
                Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main50() {
            string pattern = @"\b(\w{3,}?\.){2}?\w{3,}?\b";
            string input = "www.microsoft.com msdn.microsoft.com mywebsite mycompany.com";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at position {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main51() {
            string pattern = @"\b[A-Z](\w*?\s*?){1,10}[.!?]";
            string input = "Hi. I am writing a short note. Its purpose is " +
                            "to test a regular expression that attempts to find " +
                            "sentences with ten of fewer words. Most sentences " +
                            "in this note are short.";
            Regex.Matches(input, pattern).ToList().ForEach(m => Console.WriteLine("'{0}' found at positionm {1}", m.Value, m.Index));
            Console.ReadLine();
        }
        public static void Main52() {
            string greedyPattern = @"\b.*([0-9]{4})\b";
            string input = "1112223333 3992991999";
            Regex.Matches(input, greedyPattern).ToList().ForEach(m => Console.WriteLine("Account ending in ******{0}.", m.Groups[1].Value));
            Console.ReadLine();
        }
        public static void Main53() {
            string lazyPattern = @"\b.*?([0-9]{4})\b";
            string input = "1112223333 3992991999";
            Regex.Matches(input, lazyPattern).ToList().ForEach(m => Console.WriteLine("Account ending in ******{0}.", m.Groups[1].Value));
            Console.ReadLine();
        }
        public static void Main54() {
            string pattern = @"(a?)*";
            string input = "aaabbb";
            Match match = Regex.Match(input, pattern);
            Console.WriteLine("Match: '{0}' at index {1}", match.Value, match.Index);
            int i = 0;
            if (match.Groups.Count > 1) {
                match.Groups.Skip(1).ToList().ForEach(g => {
                    Console.WriteLine("   Group {0}: '{1}' at index {2}", ++i, g.Value, g.Index);
                    int j = 0;
                    g.Captures.ToList().ForEach(c => {
                        Console.WriteLine("     Capture {0}: '{1}' at index {2}", ++j, c.Value, c.Index);
                    });
                });
            }
            Console.ReadLine();
        }
        public static void Main55() {
            string pattern = @"(a\1|(?(1)\1)){0,2}";
            string input = "aaabbb";
            Console.WriteLine($"Regex pattern: {pattern}");
            Match match = Regex.Match(input, pattern);
            Console.WriteLine($"Match: '{match.Value}' at position {match.Index}");
            int i = 1;
            if (match.Groups.Count > 1) {
                match.Groups.Skip(1).ToList().ForEach(g => {
                    Console.WriteLine($"   Group: {i} '{g.Value}' at position {g.Index}.");
                    int j = 0;
                    g.Captures.ToList().ForEach(c => {
                        Console.WriteLine($"     Capture {j}: '{c.Value}' at position {c.Index}");
                    });
                });
            }
            Console.WriteLine();

            pattern = @"(a\1|(?(1)\1)){2}";
            Console.WriteLine($"Regex pattern: {pattern}");
            match = Regex.Match(input, pattern);
            Console.WriteLine($"Matched '{match.Value}' at position {match.Index}");
            i = 0;
            if (match.Groups.Count > 1) {
                match.Groups.Skip(1).ToList().ForEach(g => {
                    Console.WriteLine($"   Group: {i} '{g.Value}' at position {g.Index}.");
                    int j = 0;
                    g.Captures.ToList().ForEach(c => {
                        Console.WriteLine($"     Capture: {j}: '{c.Value}' at position {c.Index}.");
                    });
                });
            }
            Console.ReadLine();
        }
        public static void Main56() {
            string pattern = @"(\w)\1";
            string input = "trellis llama webbing dresser swagger";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"Found '{m.Value}' at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main57() {
            string pattern = @"(?<char>\w)\k<char>";
            string input = "trellis llama webbing dresser swagger";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"Found '{m.Value}' at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main58() {
            string pattern = @"(?<2>\w)\k<2>";
            string input = "trellis llama webbing dresser swagger";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"Found '{m.Value}' at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main59() {
            Console.WriteLine(Regex.IsMatch("aa", @"(?<char>\w)\k<1>"));
            Console.ReadLine();
        }
        public static void Main60() {
            Console.WriteLine(Regex.IsMatch("aa", @"(?<2>\w)\k<1>"));
            Console.ReadLine();
        }
        public static void Main61() {
            string pattern = @"(?<1>a)(?<1>\1b)*";
            string input = "aababb";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"Match: {m.Value}");
                m.Groups.ToList().ForEach(g => {
                    Console.WriteLine($"    Group: {g.Value}");
                });
            });
            Console.ReadLine();
        }
        public static void Main62() {
            string pattern = @"\b(\p{Lu}{2})(\d{2})?(\p{Lu}{2})\b";
            string[] inputs = { "AA22ZZ", "AABB" };
            inputs.ToList().ForEach(input => {
                Regex.Matches(input, pattern).ToList().ForEach(m => {
                    if (m.Success) {
                        Console.WriteLine($"Match in {input}: {m.Value}");
                        if (m.Groups.Count > 1) {
                            int i = 0;
                            m.Groups.Skip(1).ToList().ForEach(g => {
                                Console.WriteLine($"Group {++i}: {(g.Success ? g.Value : "<no match>")}");
                            });
                        }
                    }
                });
            });
            Console.ReadLine();
        }
        public static void Main63() {
            //Regular expression using character class
            string pattern1 = @"\bgr[ae]y\b";
            //Regular expression using either/or
            string pattern2 = @"\bgr(e|a)y\b";
            string input = "The gray wolf blended in among the grey rocks.";
            Regex.Matches(input, pattern1).ToList().ForEach(m => {
                Console.WriteLine($"'{m.Value}' found at position {m.Index}");
            });
            Console.WriteLine();
            Regex.Matches(input, pattern2).ToList().ForEach(m => {
                Console.WriteLine($"'{m.Value}' found at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main64() {
            string pattern = @"\b(\d{2}-\d{7}|\d{3}-\d{2}-\d{4})\b";
            string input = "01-9999999 020-333333 777-88-9999";
            Console.WriteLine($"Matches for {pattern}:");
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"    {m.Value} at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main65() {
            string pattern = @"\b(?(\d{2}-)\d{2}-\d{7}|\d{3}-\d{2}-\d{4})\b";
            string input = "01-9999999 020-333333 777-88-9999";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"   {m.Value} at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main66() {
            string pattern = @"\b(?<n2>\d{2}-)?(?(n2)\d{7}|\d{3}-\d{2}-\d{4})\b";
            string input = "01-9999999 020-333333 777-88-9999";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"    {m.Value} at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main67() {
            string pattern = @"\b(\d{2}-)?(?(1)\d{7}|\d{3}-\d{2}-\d{4})\b";
            string input = "01-9999999 020-333333 777-88-9999";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"    {m.Value} at position {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main68() {
            string pattern = @"\p{Sc}*(\s?\d+[.,]?\d*)\p{Sc}*";
            string replacement = "$1";
            string input = "$16.32  12.19 £16.29 €18.29  €18,29";
            string result = Regex.Replace(input, pattern, replacement);
            Console.WriteLine(result);
            Console.ReadLine();
        }
        public static void Main69() {
            string pattern = @"\p{Sc}*(?<amount>\s?\d+[.,]?\d*)\p{Sc}*";
            string replacement = "${amount}";
            string input = "$16.32  12.19 £16.29 €18.29  €18,29";
            string result = Regex.Replace(input, pattern, replacement);
            Console.WriteLine(result);
            Console.ReadLine();
        }
        public static void Main70() {
            // Define array of decimal values.
            string[] values = { "16.35", "19.72", "1234", "0.99" };
            // Determine whether currency precedes (True) or follows (False) number.
            bool precedes = NumberFormatInfo.CurrentInfo.CurrencyPositivePattern % 2 == 0;
            // Get decimal separator.
            string cSeparator = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            // Get currency symbol.
            string symbol = NumberFormatInfo.CurrentInfo.CurrencySymbol;
            // If symbol is a "$", add an extra "$".
            if (symbol == "$") symbol = "$$";

            // Define regular expression pattern and replacement string.
            string pattern = @"\b(\d+)(" + cSeparator + @"(\d+))?";
            string replacement = "$1$2";
            replacement = precedes ? symbol + " " + replacement : replacement + " " + symbol;
            values.ToList().ForEach(value => {
                Console.WriteLine($"{value} --> {Regex.Replace(value, pattern, replacement)}");
            });
            Console.ReadLine();
        }
        public static void Main71() {
            string pattern = @"^(\w+\s?)+$";
            string[] titles = { "A Tale of Two Cities",
                                "The Hound of the Baskervilles",
                                "The Protestant Ethic and the Spirit of Capitalism",
                                "The Origin of Species" };
            string replacement = "\"$&\"";
            titles.ToList().ForEach(t => { Console.WriteLine(Regex.Replace(t, pattern, replacement)); });
            Console.ReadLine();
        }
        public static void Main72() {
            string pattern = @"\d+";
            string input = "aa1bb2cc3dd4ee5";
            string substitution = "$`";
            Console.WriteLine("Matches:");
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"   {m.Value} at position {m.Index}");
            });
            Console.WriteLine($"Input string: {input}");
            Console.WriteLine($"Output string: {Regex.Replace(input, pattern, substitution)}");
            Console.ReadLine();
        }
        public static void Main73() {
            string pattern = @"\d+";
            string input = "aa1bb2cc3dd4ee5";
            string substitution = "$'";
            Console.WriteLine("Matches:");
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"   {m.Value} at position {m.Index}");
            });
            Console.WriteLine($"Input string: {input}");
            Console.WriteLine($"Output string: {Regex.Replace(input, pattern, substitution)}");
            Console.ReadLine();
        }
        public static void Main74() {
            string pattern = @"\b(\w+)\s\1\b";
            string substitution = "$+";
            string input = "The the dog jumped over the fence fence.";
            Console.WriteLine(Regex.Replace(input, pattern, substitution, RegexOptions.IgnoreCase));
            Console.ReadLine();
        }
        public static void Main75() {
            string pattern = @"\d+";
            string input = "ABC123DEF456";
            string substitution = "$_";
            Console.WriteLine($"Original String:          {input}");
            Console.WriteLine($"String with substitution: {Regex.Replace(input, pattern, substitution)}");
            Console.ReadLine();
        }
        //Regular Expression Options
        public static void Main76() {
            string pattern = @"d \w+ \s";
            string input = "Dogs are decidedly good pets.";
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
            Regex.Matches(input, pattern, options).ToList().ForEach(m => {
                Console.WriteLine($"'{m.Value}// found at index {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main77() {
            string pattern = @"(?ix)d \w+ \s";
            string input = "Dogs are decidedly good pets.";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"'{m.Value}// found at index {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main78() {
            string pattern = @"(?ix: d \w+)\s";
            string input = "Dogs are decidedly good pets.";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"'{m.Value}// found at index {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main79() {
            string pattern = @"\bthe\w*\b";
            string input = "The man then told them about that event.";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"Found {m.Value} at index {m.Index}");
            });
            Console.WriteLine();
            Regex.Matches(input, pattern, RegexOptions.IgnoreCase).ToList().ForEach(m => {
                Console.WriteLine($"Found {m.Value} at index {m.Index}");
            });
            Console.ReadLine();
        }
        public static void Main80() {
            string pattern = @"\b(?i:t)he\w*\b";
            string input = "The man then told them about that event.";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                Console.WriteLine($"Found {m.Value} at index {m.Index}");
            });
            Console.WriteLine();
            Regex.Matches(input, pattern, RegexOptions.IgnoreCase).ToList().ForEach(m => {
                Console.WriteLine($"Found {m.Value} at index {m.Index}");
            });
            Console.ReadLine();
        }
        public class DescendingComparer<T> : IComparer<T> {
            public int Compare(T x, T y) => Comparer<T>.Default.Compare(x, y) * -1;
        }
        public static void Main81() {
            SortedList<int, string> scores = new SortedList<int, string>(new DescendingComparer<int>());

            string input = "Joe 164\n" +
                           "Sam 208\n" +
                           "Allison 211\n" +
                           "Gwen 171\n";
            string pattern = @"^(\w+)\s(\d+)$";
            bool matched = false;

            Console.WriteLine("Without Multiline option:");
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                scores.Add(Int32.Parse(m.Groups[2].Value), (string)m.Groups[1].Value);
                matched = true;
            });
            if (!matched)
                Console.WriteLine("    No matches.");
            Console.WriteLine();

            //Redfine the pattern to handle multiple lines.
            pattern = @"^(\w+)\s(\d+)\r*$";
            Console.WriteLine("With Multiline option:");
            Regex.Matches(input, pattern, RegexOptions.Multiline).ToList().ForEach(m => {
                scores.Add(Int32.Parse(m.Groups[2].Value), (string)m.Groups[1].Value);
            });

            //scores.ToList().ForEach(kvp=> {Console.WriteLine($"{kvp.Value}: {kvp.Key}");});
            scores.ToList().ForEach(kvp => { cw($"{kvp.Value}: {kvp.Key}"); });
            cr();
        }
        public static void Main82() {
            string pattern = @"^.+";
            string input = "This is one line and" + Environment.NewLine + "this is the second.";
            Regex.Matches(input, pattern).ToList().ForEach(m => cw(Regex.Escape(m.Value)));
            cw("");
            Regex.Matches(input, pattern, RegexOptions.Singleline).ToList().ForEach(m => cw(Regex.Escape(m.Value)));
            cr();
        }
        public static void Main83() {
            string pattern = @"(?s)^.+";
            string input = "This is one line and" + Environment.NewLine + "this is the second.";
            Regex.Matches(input, pattern).ToList().ForEach(m => cw(Regex.Escape(m.Value)));
            cr();
        }
        public static void Main84() {
            string input = "This is the first sentence. Is it the beginning " +
                           "of a literary masterpiece? I think not. Instead, " +
                           "it is a nonsensical paragraph.";
            string pattern = @"\b\(?((?>\w+),?\s?)+[\.!?]\)?";
            cw("With implicit captures:");
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                cw($"The Match: {m.Value}");
                int i = -1;
                m.Groups.ToList().ForEach(g => {
                    cw($"    Group {++i}: {g.Value}");
                    int j = -1;
                    g.Captures.ToList().ForEach(c => {
                        cw($"      Capture {++j}: {c.Value}");
                    });
                });
            });
            cw("");
            cw("With explicit captures only:");
            Regex.Matches(input, pattern, RegexOptions.ExplicitCapture).ToList().ForEach(m => {
                cw($"The Match: {m.Value}");
                int i = 0;
                m.Groups.ToList().ForEach(g => {
                    cw($"    Group {i++}: {g.Value}");
                    int j = 0;
                    g.Captures.ToList().ForEach(c => {
                        cw($"      Capture {j++}: {c.Value}");
                    });
                });
            });
            cw("");
            cr();
        }
        public static void Main85() {
            string input = "This is the first sentence. Is it the beginning " +
                           "of a literary masterpiece? I think not. Instead, " +
                           "it is a nonsensical paragraph.";
            string pattern = @"(?n)\b\(?((?>\w+),?\s?)+[\.!?]\)?";

            Regex.Matches(input, pattern).ToList().ForEach(m => {
                cw($"The Match: {m.Value}");
                int i = -1;
                m.Groups.ToList().ForEach(g => {
                    cw($"    Group {++i}: {g.Value}");
                    int j = -1;
                    g.Captures.ToList().ForEach(c => {
                        cw($"      Capture {++j}: {c.Value}");
                    });
                });
            });
            cr();
        }
        public static void Main86() {
            string input = "This is the first sentence. Is it the beginning " +
                           "of a literary masterpiece? I think not. Instead, " +
                           "it is a nonsensical paragraph.";
            string pattern = @"\b\(?(?n:(?>\w+),?\s?)+[\.!?]\)?";

            Regex.Matches(input, pattern).ToList().ForEach(m => {
                cw($"The Match: {m.Value}");
                int i = -1;
                m.Groups.ToList().ForEach(g => {
                    cw($"    Group {++i}: {g.Value}");
                    int j = -1;
                    g.Captures.ToList().ForEach(c => {
                        cw($"      Capture {++j}: {c.Value}");
                    });
                });
            });
            cr();
        }
        public static void Main87() {
            string input = "This is the first sentence. Is it the beginning " +
                           "of a literary masterpiece? I think not. Instead, " +
                           "it is a nonsensical paragraph.";
            string pattern = @"\b\(?(?n:(?>\w+),?\s?)+[\.!?]\)?";
            Regex.Matches(input, pattern, RegexOptions.IgnorePatternWhitespace).ToList().ForEach(m => { cw($"The Match: {m.Value}"); });
            cr();
        }
        public static void Main88() {
            string input = "This is the first sentence. Is it the beginning " +
                           "of a literary masterpiece? I think not. Instead, " +
                           "it is a nonsensical paragraph.";
            string pattern = @"(?x)\b \(? ( (?>\w+) ,?\s? )+ [\.!?] \)? # Matches an entire sentence.";
            Regex.Matches(input, pattern).ToList().ForEach(m => { cw(m.Value); });
            cr();
        }
        public static void Main89() {
            string pattern = @"\bb\w+\s";
            string input = "builder rob rabble";
            Regex.Matches(input, pattern, RegexOptions.RightToLeft).ToList().ForEach(m => {
                cw($"'{m.Value}' found at position {m.Index}");
            });
            cr();
        }
        public static void Main90() {
            string[] inputs = { "1 May 1917", "June 16, 2003" };
            string pattern = @"(?<=\d{1,2}\s)\w+,?\s\d{4}";
            inputs.ToList().ForEach(input => {
                var match = Regex.Match(input, pattern, RegexOptions.RightToLeft);
                if (match.Success)
                    cw($"The date occurs in {match.Value}");
                else
                    cw($"{input} does not match.");
            });
            cr();
        }
        public static void Main91() {
            string[] values = { "целый мир", "the whole world" };
            string pattern = @"\b(\w+\s*)+";
            values.ToList().ForEach(v => {
                cw("Canonical matching:");
                if (Regex.IsMatch(v, pattern))
                    cw($"'{v}' matches the pattern");
                else
                    cw($"'{v}' does not match the pattern");
                cw("ECMA matching:");
                if (Regex.IsMatch(v, pattern, RegexOptions.ECMAScript))
                    cw($"'{v}' matches the pattern");
                else
                    cw($"'{v}' does not match the pattern");
            });
            cr();
        }
        static string pattern;
        public static void Main92() {
            string input = "aa aaaa aaaaaa ";
            pattern = @"((a+)(\1) ?)+";
            cw("Doing 1");
            //Match using Canonical matching.
            AnalyseMatch(Regex.Match(input, pattern));
            cw("Doing 2");
            // Match using ECMAScript.
            AnalyseMatch(Regex.Match(input, pattern, RegexOptions.ECMAScript));
            cr();
        }
        private static void AnalyseMatch(Match m) {
            if (m.Success) {
                cw($"'{pattern}' matches {m.Value} at position {m.Index}");
                int i = 0;
                m.Groups.ToList().ForEach(g => {
                    cw($"    {i}: '{g.Value}'");
                    int j = 0; i++;
                    g.Captures.ToList().ForEach(c => {
                        cw($"      {j}: '{c.Value}'");
                        j++;
                    });
                });
            } else {
                cw("No match found.");
            }
            cw("");
        }
        public static void Main93() {
            CultureInfo defaultCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

            string input = "file://C:/Documents.MyReport.Doc";
            string pattern = @"FILE://";

            cw($"Culture-sensitive matching ({Thread.CurrentThread.CurrentCulture.Name} culture)...");
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                cw("URLs that access files are not allowed");
            else
                cw($"Access to {input} is allowed.");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            cr();
        }
        public static void Main94() {
            CultureInfo defaultCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

            string input = "file://C:/Documents.MyReport.Doc";
            string pattern = @"FILE://";

            cw($"Culture-sensitive matching ({Thread.CurrentThread.CurrentCulture.Name} culture)...");
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                cw("URLs athat access files are not allowed");
            else
                cw($"Access to {input} is allowed.");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            cr();
        }
        public static void Main95() {
            string pattern;
            string input = "double dare double Double a Drooling dog The Dreaded Deep";

            pattern = @"\b(D\w+)\s(d\w+)\b";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                cw(m.Value);
                if (m.Groups.Count > 1) {
                    int i = 0;
                    m.Groups.Skip(1).ToList().ForEach(g => {
                        cw($"    Group {++i}: {g.Value}");
                    });
                }
            });
            cw("");

            // Change regular expression pattern to include options.
            pattern = @"\b(D\w+)(?ixn) \s (d\w+) \b";
            Regex.Matches(input, pattern).ToList().ForEach(m => {
                cw(m.Value);
                if (m.Groups.Count > 1) {
                    int i = 0;
                    m.Groups.Skip(1).ToList().ForEach(g => {
                        cw($"    Group {++i}: {g.Value}");
                    });
                }
            });
            cr();
        }
        public static void Main96() {
            string pattern = @"\b((?# case-sensitive comparison)D\w+)\s(?ixn)((?# case-insensitive comparison)d\w+)\b";
            Regex rgx = new Regex(pattern);
            string input = "double dare double Double a Drooling dog The Dreaded Deep";

            cw("Pattern: " + pattern.ToString());
            // Match pattern using default options.
            rgx.Matches(input).ToList().ForEach(m => {
                cw(m.Value);
                m.Groups.Skip(1).ToList().ForEach(g => {
                    int i = 1;
                    g.Captures.ToList().ForEach(c => {
                        cw($"    Group {++i}: {c.Value}");
                    });
                });
            });
            cr();
        }
        public static void Main97() {
            string pattern = @"\{\d+(,-*\d+)*(\:\w{1,4}?)*\}(?x) # Looks for a composite format item.";
            string input = "{0,-3:F}";
            cw($"'{input}':");
            if (Regex.IsMatch(input, pattern))
                cw("    contains a composite format item.");
            else
                cw("    does not contain a composite format item.");
            cr();
        }
        public static void Main98() {
            Stopwatch sw;
            string[] addresses = { "AAAAAAAAAAA@contoso.com",
                             "AAAAAAAAAAaaaaaaaaaa!@contoso.com"};
            // The following regular expression should not actually be used to
            // validate an email address.
            string pattern = @"^[0-9A-Z]([-.\w]*[0-9A-Z])*$";
            string input;

            addresses.ToList().ForEach(a => {
                string mailbox = a.Substring(0, a.IndexOf("@"));
                int i = 0;
                for (int ctr = mailbox.Length - 1; ctr >= 0; ctr--) {
                    i++;

                    input = mailbox.Substring(ctr, i);
                    sw = Stopwatch.StartNew();
                    Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                    sw.Stop();
                    if (m.Success)
                        cw($"{String.Format("{0,2}. Matched '{1,25}' in {2}", i, m.Value, sw.Elapsed)}");
                    else
                        cw($"{String.Format("{0,2}. Failed '{1,25}' in {2}", i, input, sw.Elapsed)}");
                }
            });
            cr();
        }
        public static bool IsValidCurrencyInefficient(string ccyValue) {
            string pattern = @"\p{Sc}+\s*\d+";
            Regex currencyRegex = new Regex(pattern);
            return currencyRegex.IsMatch(ccyValue);
        }
        public static bool IsValidCurrency(string ccyValue) {
            string pattern = @"\p{Sc}+\s*\d+";
            return Regex.IsMatch(ccyValue, pattern);
        }
        public static void Main99() {
            string pattern = @"\b(\w+((\r?\n)|,?\s))*\w+[.?:;!]";
            Stopwatch sw;
            Match m;
            int i;

            StreamReader inFile = new StreamReader(@"C:\Users\Dragon\Documents\Books\Not art of war.txt");
            string input = inFile.ReadToEnd();
            inFile.Close();

            // Read the first ten pages with interpreted Regex.
            cw("10 sentences with interpreted Regex:");
            sw = Stopwatch.StartNew();
            Regex int10 = new Regex(pattern, RegexOptions.Singleline);
            m = int10.Match(input);
            for (i = 0; i <= 9; i++) {
                if (m.Success)
                    // Do nothing with the match except get the next match.
                    m = m.NextMatch();
                else
                    break;
            }
            sw.Stop();
            cw($"   {i} matches in {sw.Elapsed}");

            // Read the first ten pages with compiled Regex.
            cw("10 sentences with compiled Regex:");
            sw = Stopwatch.StartNew();
            Regex comp10 = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled);
            m = comp10.Match(input);
            for (i = 0; i <= 9; i++) {
                if (m.Success)
                    // Do nothing with the match except get the next match.
                    m = m.NextMatch();
                else
                    break;
            }
            sw.Stop();
            cw($"   {i} matches in {sw.Elapsed}");

            // Read all pages with interpreted Regex.
            cw("All sentences with interpreted Regex:");
            sw = Stopwatch.StartNew();
            Regex intAll = new Regex(pattern, RegexOptions.Singleline);
            m = intAll.Match(input);
            int matches = 0;
            while (m.Success) {
                matches++;
                m = m.NextMatch();
            }
            sw.Stop();
            cw(String.Format("   {0:N0} matches in {1}", matches, sw.Elapsed));

            // Read all pages with compiled Regex.
            cw("All sentences with compiled Regex:");
            sw = Stopwatch.StartNew();
            Regex compAll = new Regex(pattern, RegexOptions.Singleline |RegexOptions.Compiled);
            m = compAll.Match(input);
            matches = 0;
            while (m.Success) {
                matches++;
                m = m.NextMatch();
            }
            sw.Stop();
            cw(String.Format("   {0:N0} matches in {1}", matches, sw.Elapsed));

            cr();
        }
        public static void Main100() {
            RegexCompilationInfo SentencePattern 
                = new RegexCompilationInfo(@"\b(\w+((\r?\n)|,?\s))*\w+[.?:;!]",
                                           RegexOptions.Multiline,
                                           "SentencePattern",
                                           "Utilities.RegularExpressions",
                                           true);
            RegexCompilationInfo[] regexes = { SentencePattern };
            AssemblyName assemName = new AssemblyName("RegexLib, Version=1.0.0.1001, Culture=neutral, PublicKeyToken=null");
            Regex.CompileToAssembly(regexes, assemName);
            cr();
        }
        //public static void Main() {
        //    string pattern = @"";
        //    string input = "";
        //    Regex.Matches(input, pattern)
        //    cr();
        //}
        public static void cw(string intStr) => Console.WriteLine(intStr);
        public static void cr() => Console.ReadLine();
    } //Program
} //Namespace

