namespace QuickMutasi.Models;

public class Transaction
{
    public string Tanggal { get; set; }
    public string Keterangan { get; set; }
    public decimal Jumlah { get; set; }
    public string Tipe { get; set; } // "DB" atau "CR"
}
