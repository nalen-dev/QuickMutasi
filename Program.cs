using System.Text.RegularExpressions;
using UglyToad.PdfPig;

public class Program
{
    public class Transaction
    {
        public string Tanggal { get; set; }
        public string Keterangan { get; set; }
        public decimal Jumlah { get; set; }
        public string Tipe { get; set; } // "DB" atau "CR"
    }

    static void Main()
    {
        string pdfPath = "mutasi-rekening/MutasiBCA_010425-070425.pdf";
        ExtractText(pdfPath);
    }


    public static void ExtractText(string path)
    {
        using (PdfDocument document = PdfDocument.Open(path))
        {
            foreach (var page in document.GetPages())
            {
                string pageText = page.Text;
                string period = GetChunkFromText(pageText, "PERIODE:", 24);
                Console.WriteLine(period);
                string transactionsFull = GetChunkFromText(pageText, "KETERANGANMUTASI", 0);
                ExtractTrasactions(transactionsFull);
            }
        }
    }

    public static string GetChunkFromText(string fulltext, string cutoff, int endcut)
    {
        int _index = fulltext.IndexOf(cutoff);

        if (_index == -1)
            return "";
        else if (endcut == 0)
        {
            return fulltext.Substring(_index + cutoff.Length).Trim();
        }
        else
        {
            return fulltext.Substring(_index + cutoff.Length, endcut);
        }
    }

    public static void ExtractTrasactions(string fulltext)
    {
        string pattern = @"(\d{2}/\d{2}/\d{4})(.*?)(-?\d{1,3}(?:,\d{3})*\.\d{2})(DB|CR)";

        var transactions = new List<Transaction>();

        foreach (Match match in Regex.Matches(fulltext, pattern, RegexOptions.IgnoreCase))
        {
            var transaction = new Transaction
            {
                Tanggal = match.Groups[1].Value,
                Keterangan = match.Groups[2].Value.Trim(),
                Jumlah = decimal.Parse(match.Groups[3].Value.Replace(",", "")),
                Tipe = match.Groups[4].Value
            };

            transactions.Add(transaction);
        }

        // Cetak hasil
        foreach (var t in transactions)
        {
            Console.WriteLine($"Tanggal: {t.Tanggal}");
            Console.WriteLine($"Keterangan: {t.Keterangan}");
            Console.WriteLine($"Mutasi: {t.Jumlah} {t.Tipe}\n");
        }

    }


}

