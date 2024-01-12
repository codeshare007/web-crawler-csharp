using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace web_crawler_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://en.wikipedia.org/wiki/Microsoft";
            string text = GetTextFromWebpage(url);
            List<string> commonWords = MostCommonWords(text, 10);
            Console.WriteLine("The 10 most common words on the webpage are:");
            foreach (string word in commonWords)
            {
                Console.WriteLine(word);
            }
            Console.ReadKey();
        }

        static string GetTextFromWebpage(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                return content;
            }
        }

        static List<string> MostCommonWords(string text, int numWords)
        {
            Dictionary<string, int> wordCounts = new Dictionary<string, int>();

            foreach (Match match in new Regex("\\w+").Matches(text))
            {
                string word = match.Value;
                if (wordCounts.ContainsKey(word))
                {
                    wordCounts[word]++;
                }
                else
                {
                    wordCounts.Add(word, 1);
                }
            }

            return wordCounts.OrderByDescending(kvp => kvp.Value).Take(numWords).Select(kvp => kvp.Key).ToList();
        }
    }
}

