using System;
using System.Collections.Generic;

namespace Db.Models;

public partial class Urunler
{
    public int UrunId { get; set; }

    public string? UrunAd { get; set; }

    public int? KategoriId { get; set; }

    public int? Miktar { get; set; }

    public bool? Akitflik { get; set; }

    public virtual Kategoriler? Kategori { get; set; }

    public virtual ICollection<Satislar> Satislars { get; set; } = new List<Satislar>();
}
