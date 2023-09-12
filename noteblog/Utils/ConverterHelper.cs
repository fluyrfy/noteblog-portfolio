using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

public class ConverterHelper
{
    public static string ConvertToUCS(string str, bool s)
    {
        str = str.ToLower();
        string charSet = "UTF-8";
        string[] arr = new string[str.Length];
        string output = "";
        int c = str.Length;
        bool t = false;

        for (int i = 0; i < c; i++)
        {
            arr[i] = str.Substring(i, 1);
        }

        foreach (string v in arr)
        {
            if (Regex.IsMatch(v, @"\w", RegexOptions.IgnoreCase))
            {
                output += v;
                t = true;
            }
            else
            {
                if (t)
                    output += " ";
                if (s)
                    output += "+";

                byte[] utf8Bytes = Encoding.UTF8.GetBytes(v);
                byte[] ucs2Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);
                string hex = BitConverter.ToString(ucs2Bytes).Replace("-", "");
                output += hex + " ";
                t = false;
            }
        }
        return output;
    }

    public static string GenerateNgrams(string input, int n)
    {
        input = input.ToLower();
        string[] words = input.Split(' ');
        List<string> ngrams = new List<string>();

        for (int i = 0; i < words.Length - n + 1; i++)
        {
            StringBuilder ngram = new StringBuilder();
            for (int j = i; j < i + n; j++)
            {
                ngram.Append(words[j]);
                if (j < i + n - 1)
                {
                    ngram.Append(" ");
                }
            }
            ngrams.Add(ngram.ToString());
        }

        return string.Join(" ", ngrams);
    }

    public static string ExtractTextFromHtml(string html)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        var textNodes = doc.DocumentNode.DescendantsAndSelf()
            .Where(n => n.NodeType == HtmlNodeType.Text);

        StringBuilder sb = new StringBuilder();

        foreach (var node in textNodes)
        {
            sb.Append(node.InnerText);
        }

        return sb.ToString();
    }

    public static byte[] ConvertFileToBytes(Stream fileStream)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            fileStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public string RemoveSpaces(string input)
    {
        return Regex.Replace(input, @"\s+", "");
    }
}
