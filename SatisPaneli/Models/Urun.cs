namespace SatisPaneli.Models
{
    public class Urun
    {
        public int UrunId { get; set; }

        public string? UrunAd { get; set; }

        public int? KategoriId { get; set; }

        public int? Miktar { get; set; }

        public bool? Akitflik { get; set; }
        public string? KategoriAd { get; set; }
    }
}
