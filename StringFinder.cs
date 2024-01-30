using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converter
{
    using System;
    using System.Collections.Generic;

    public class StringFinder
    {
        public static void FindOccurrencesAndCountNewLines(string input)
        {
            List<string> occurrences = new List<string>();
            int newlineCount = 0;
            bool isCapturing = false;
            string currentCapture = "";

            foreach (char c in input)
            {
                if (c == '\n')
                {
                    newlineCount++;
                }

                if ((c == 'X' || c == 'Y') && !isCapturing)
                {
                    isCapturing = true;
                    currentCapture += c;
                }
                else if (isCapturing)
                {
                    if (char.IsWhiteSpace(c))
                    {
                        occurrences.Add(currentCapture);
                        currentCapture = "";
                        isCapturing = false;
                    }
                    else
                    {
                        currentCapture += c;
                    }
                }
            }

            if (!string.IsNullOrEmpty(currentCapture))
            {
                occurrences.Add(currentCapture);
            }

            Console.WriteLine("Occurrences:");
            foreach (var occurrence in occurrences)
            {
                Console.WriteLine(occurrence);
            }
            Console.WriteLine("Newline count: " + newlineCount);
        }
    }


}
