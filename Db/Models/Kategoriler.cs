using System;
using System.Collections.Generic;

namespace Db.Models;

public partial class Kategoriler
{
    public int KategoriId { get; set; }

    public string? KategoriAd { get; set; }

    public bool? Aktiflik { get; set; }

    public virtual ICollection<Urunler> Urunlers { get; set; } = new List<Urunler>();
}
