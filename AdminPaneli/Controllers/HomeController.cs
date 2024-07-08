using AdminPaneli.Models;
using Db.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AdminPaneli.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Task2EticaretContext _context;

        public HomeController(ILogger<HomeController> logger, Task2EticaretContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string _mail, string _sifre)
        {
            var admin = _context.Admins.Find(1);
            string sifre = admin.Sifre;
            string mail = admin.Mail;
            int id = admin.AdminId;
            if (sifre == _sifre && mail == _mail)
            {
                HttpContext.Session.SetInt32("admin_id", id);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }
        public ActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            string mail = _context.Admins.Find(1).Mail;
            int kod = SifreUnutmaKOdu(mail);
            ViewBag.Kod = kod;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifremiUnuttum(int _kod, int Kod)
        {
            if (_kod == Kod)
            {

                return RedirectToAction("SifreDegistir", "Home");
            }
            else
            {
                return RedirectToAction("SifreDegistir", "Home");
            }
        }
        [HttpGet]
        public ActionResult SifreDegistir()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifreDegistir(string sifre)
        {
            _context.Admins.Find(1).Sifre = sifre;
            _context.SaveChanges();
            return RedirectToAction("Login", "Home");
        }
        public IActionResult Index()
        {
            int? admin_id = HttpContext.Session.GetInt32("admin_id");
            if (admin_id.HasValue)
            {
                var liste = _context.Urunlers.Where(p => p.Akitflik == true).OrderByDescending(p => p.UrunId).Select(p => new Urun
                {
                    UrunId = p.UrunId,
                    Akitflik = p.Akitflik,
                    KategoriAd = p.Kategori.KategoriAd,
                    KategoriId = p.KategoriId,
                    Miktar = p.Miktar,
                    UrunAd = p.UrunAd
                }).ToList();
                return View(liste);
            }
            else
            {

                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult Sil(int urun_id)
        {
            int? admin_id = HttpContext.Session.GetInt32("admin_id");
            if (admin_id.HasValue)
            {
                _context.Urunlers.Find(urun_id).Akitflik = false;
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpGet]
        public ActionResult Ekle()
        {
            int? admin_id = HttpContext.Session.GetInt32("admin_id");
            if (admin_id.HasValue)
            {
                List<SelectListItem> ktg = (from i in _context.Kategorilers.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.KategoriAd,
                                                Value = i.KategoriId.ToString()
                                            }).ToList();
                ViewBag.Ktg = ktg;
                return View();

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle(int KategoriId, int miktar, string urun_ad)
        {
            _context.Urunlers.Add(new Urunler
            {
                Akitflik = true,
                KategoriId = KategoriId,
                Miktar = miktar,
                UrunAd = urun_ad
            });
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Satislar()
        {
            int? admin_id = HttpContext.Session.GetInt32("admin_id");
            if (admin_id.HasValue)
            {
                SatislarViewModel model = new SatislarViewModel();
                model.onayli_satislar = _context.Satislars.Where(p => p.Onay == true).OrderByDescending(p => p.SatisId).Select(p => new Satis
                {
                    SatisId = p.SatisId,
                    Onay = p.Onay,
                    Address = p.Address,
                    AliciAd = p.AliciAd,
                    Cvv = p.Cvv,
                    GecerlilikTarihi = p.GecerlilikTarihi,
                    KartNumarasi = p.KartNumarasi,
                    Mail = p.Mail,
                    Tarih = p.Tarih,
                    Tel = p.Tel,
                    UrunAd = p.Urun.UrunAd,
                    UrunId = p.UrunId
                }).ToList();
                model.onaysiz_satislar = _context.Satislars.Where(p => p.Onay == false).OrderByDescending(p => p.SatisId).Select(p => new Satis
                {
                    SatisId = p.SatisId,
                    Onay = p.Onay,
                    Address = p.Address,
                    AliciAd = p.AliciAd,
                    Cvv = p.Cvv,
                    GecerlilikTarihi = p.GecerlilikTarihi,
                    KartNumarasi = p.KartNumarasi,
                    Mail = p.Mail,
                    Tarih = p.Tarih,
                    Tel = p.Tel,
                    UrunAd = p.Urun.UrunAd,
                    UrunId = p.UrunId
                }).ToList();
                return View(model);

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult SatisOnayla(int satis_id)
        {
            var satis = _context.Satislars.Find(satis_id);
            satis.Onay = true;
            string kullanici_mail = satis.Mail;
            _context.SaveChanges();
            MailGonder(kullanici_mail);
            return RedirectToAction("Satislar", "Home");
        }
        public ActionResult Kategoriler()
        {
            var liste = _context.Kategorilers.Where(p => p.Aktiflik == true).OrderByDescending(p => p.KategoriId).Select(p => new Kategori
            {
                KategoriId = p.KategoriId,
                KategoriAd = p.KategoriAd
            }).ToList();
            return View(liste);
        }
        public ActionResult KategoriSil(int kategori_id)
        {
            _context.Kategorilers.Find(kategori_id).Aktiflik = false;
            _context.SaveChanges();
            return RedirectToAction("Kategoriler", "Home");
        }
        [HttpGet]
        public ActionResult KategoriEkle()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KategoriEkle(string kategori_ad)
        {
            _context.Kategorilers.Add(new Kategoriler
            {
                Aktiflik = true,
                KategoriAd = kategori_ad
            });
            _context.SaveChanges();
            return RedirectToAction("Kategoriler", "Home");
        }
        public void MailGonder(string kullaniciMail)
        {

            DateTime tarih = DateTime.Now;
            string sifre = _context.Admins.Find(1).MailSifre;
            string mail = _context.Admins.Find(1).Mail;
            var cred = new NetworkCredential(mail, sifre);
            var client = new SmtpClient("smtp.gmail.com", 587);
            var msg = new System.Net.Mail.MailMessage();
            msg.To.Add(kullaniciMail);
            msg.Subject = "Satýþ talebi";
            msg.Body = $"{tarih} tarihinde satýþ talebiniz alýnmýþtýr bu adresten geri dönüþ saðlanacaktýr";
            msg.IsBodyHtml = false;
            msg.From = new MailAddress(mail, "Satýþ Sitesi", Encoding.UTF8);
            client.Credentials = cred;
            client.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            client.Send(msg);


        }

        public int SifreUnutmaKOdu(string kullaniciMail)
        {
            Random rnd = new Random();
            int random = rnd.Next(100000, 999999 + 1);

            var admin = _context.Admins.Find(1);
            string sifre = admin.MailSifre;
            string mail = admin.Mail;
            DateTime tarih = DateTime.Now;
            var cred = new NetworkCredential(mail, sifre);
            var client = new SmtpClient("smtp.gmail.com", 587);
            var msg = new System.Net.Mail.MailMessage();
            msg.To.Add(kullaniciMail);
            msg.Subject = "Kayýt Onay Kodu";
            msg.Body = $"Þifre yenileme için mailinizi þu kodu girerek doðrulayýnýz {random}";
            msg.IsBodyHtml = false;
            msg.From = new MailAddress(mail, "Doðrulama Kodu", Encoding.UTF8);
            client.Credentials = cred;
            client.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            client.Send(msg);
            return random;


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
