using System;
using System.IO;
using System.Collections.Generic;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

class Program
{
    static void Main(string[] args)
    {
        string pdfPath = null;
        bool noComments = false;
        double columnThreshold = 15.0;
        double yTolerance = 3.0;
        string separator = "###";

        // Parse arguments
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--no-comments":
                    noComments = true;
                    break;
                case "--column-threshold":
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double ct))
                    {
                        columnThreshold = ct;
                        i++;
                    }
                    break;
                case "--y-tolerance":
                    if (i + 1 < args.Length && double.TryParse(args[i + 1], out double yt))
                    {
                        yTolerance = yt;
                        i++;
                    }
                    break;
                case "--separator":
                    if (i + 1 < args.Length)
                    {
                        separator = args[i + 1];
                        i++;
                    }
                    break;
                default:
                    if (pdfPath == null)
                    {
                        pdfPath = args[i];
                    }
                    break;
            }
        }

        if (pdfPath == null)
        {
            Console.WriteLine("Error: No PDF file provided.");
            Console.WriteLine("Usage: PdfTextExtractor <PDF file path> [--no-comments] [--column-threshold <value>] [--y-tolerance <value>] [--separator <string>]");
            return;
        }

        if (!File.Exists(pdfPath))
        {
            Console.WriteLine($"Error: File '{pdfPath}' does not exist.");
            return;
        }

        if (Path.GetExtension(pdfPath).ToLower() != ".pdf")
        {
            Console.WriteLine("Error: The file must be a PDF (.pdf).");
            return;
        }

        if (!noComments) Console.WriteLine($"***Processing PDF: {pdfPath}");

        try
        {
            using (PdfDocument document = PdfDocument.Open(pdfPath))
            {
                foreach (var page in document.GetPages())
                {
                    ExtractLinesFromPage(page, columnThreshold, yTolerance, separator);
                    if (!noComments) Console.WriteLine("---End of page---");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading PDF: {ex.Message}");
        }
    }

    static void ExtractLinesFromPage(Page page, double columnThreshold, double yTolerance, string separator)
    {
        List<(string text, double y, double x, double width)> words = new List<(string, double, double, double)>();

        foreach (var word in page.GetWords())
        {
            double wordWidth = word.BoundingBox.Right - word.BoundingBox.Left;
            words.Add((word.Text, word.BoundingBox.Bottom, word.BoundingBox.Left, wordWidth));
        }

        words.Sort((a, b) => 
        {
            int yComparison = b.y.CompareTo(a.y);
            return yComparison != 0 ? yComparison : a.x.CompareTo(b.x);
        });

        double? lastY = null;
        double lastWordEndX = 0;
        List<string> currentLine = new List<string>();

        foreach (var (text, y, x, width) in words)
        {
            if (lastY != null && Math.Abs(lastY.Value - y) > yTolerance)
            {
                Console.WriteLine(string.Join("", currentLine));
                currentLine.Clear();
                lastWordEndX = 0;
            }

            double spaceBetween = x - lastWordEndX;

            if (currentLine.Count > 0 && spaceBetween > columnThreshold)
            {
                currentLine.Add(separator);
            }
            else
            {
                currentLine.Add(" ");
            }

            currentLine.Add(text);
            lastWordEndX = x + width;
            lastY = y;
        }

        if (currentLine.Count > 0)
        {
            Console.WriteLine(string.Join("", currentLine));
        }
    }
}
