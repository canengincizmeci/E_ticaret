namespace AdminPaneli.Models
{
    public class Satis
    {
        public int SatisId { get; set; }
        public int? UrunId { get; set; }
        public string? UrunAd { get; set; }
        public string? AliciAd { get; set; }
        public string? KartNumarasi { get; set; }

        public string? GecerlilikTarihi { get; set; }

        public string? Cvv { get; set; }

        public string? Address { get; set; }

        public string? Tel { get; set; }

        public string? Mail { get; set; }

        public DateTime? Tarih { get; set; }

        public bool? Onay { get; set; }
    }
}
