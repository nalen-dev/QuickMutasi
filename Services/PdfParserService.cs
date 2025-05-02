using QuickMutasi.Models;
using UglyToad.PdfPig;
using System.Text.RegularExpressions;

namespace QuickMutasi.Services;

public class PdfParserService
{
    public List<Transaction> ParseTransactions(string pdfPath)
    {
        var transactions = new List<Transaction>();
        
        using (var document = PdfDocument.Open(pdfPath))
        {
            foreach (var page in document.GetPages())
            {
                string pageText = page.Text;
                string transactionsFull = GetChunkFromText(pageText, "KETERANGANMUTASI", 0);
                transactions.AddRange(ExtractTransactions(transactionsFull));
            }
        }
        
        return transactions;
    }

    private string GetChunkFromText(string fulltext, string cutoff, int endcut)
    {
        int _index = fulltext.IndexOf(cutoff);

        if (_index == -1)
            return "";
        
        return endcut == 0 
            ? fulltext.Substring(_index + cutoff.Length).Trim()
            : fulltext.Substring(_index + cutoff.Length, endcut);
    }

    private List<Transaction> ExtractTransactions(string fulltext)
    {
        string pattern = @"(\d{2}/\d{2}/\d{4})(.*?)(-?\d{1,3}(?:,\d{3})*\.\d{2})(DB|CR)";
        var transactions = new List<Transaction>();

        foreach (Match match in Regex.Matches(fulltext, pattern, RegexOptions.IgnoreCase))
        {
            var transaction = new Transaction
            {
                Tanggal = match.Groups[1].Value,
                Keterangan = Regex.Replace(match.Groups[2].Value, @"\s+", " ").Trim(),
                Jumlah = decimal.Parse(match.Groups[3].Value.Replace(",", "")),
                Tipe = match.Groups[4].Value
            };
            transactions.Add(transaction);
        }

        return transactions;
    }
}
