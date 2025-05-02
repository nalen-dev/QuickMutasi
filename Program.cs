using QuickMutasi.Models;
using QuickMutasi.Services;

namespace QuickMutasi;

class Program
{
    static void Main()
    {
        string pdfPath = "mutasi-rekening/MutasiBCA_010425-070425.pdf";
        
        var parser = new PdfParserService();
        var transactions = parser.ParseTransactions(pdfPath);
        
        foreach (var t in transactions)
        {
            Console.WriteLine($"Tanggal: {t.Tanggal}");
            Console.WriteLine($"Keterangan: {t.Keterangan}");
            Console.WriteLine($"Mutasi: {t.Jumlah} {t.Tipe}\n");
        }
    }
}
