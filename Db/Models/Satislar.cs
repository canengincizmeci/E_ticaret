using System;
using System.Collections.Generic;

namespace Db.Models;

public partial class Satislar
{
    public int SatisId { get; set; }

    public int? UrunId { get; set; }

    public string? AliciAd { get; set; }

    public string? KartNumarasi { get; set; }

    public string? GecerlilikTarihi { get; set; }

    public string? Cvv { get; set; }

    public string? Address { get; set; }

    public string? Tel { get; set; }

    public string? Mail { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? Onay { get; set; }

    public virtual Urunler? Urun { get; set; }
}
