using Db.Models;
using Microsoft.AspNetCore.Mvc;
using SatisPaneli.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SatisPaneli.Controllers
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

        public IActionResult Index()
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
        [HttpGet]
        public ActionResult SatinAl(int urun_id)
        {
            ViewBag.urun_id = urun_id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SatinAl(int urun_id, string alici_ad, string kart_numara, string son_tarih, string CVV, string adress,
            string tel, string mail)
        {
            DateTime tarih = DateTime.Now;
            _context.Satislars.Add(new Satislar
            {
                Address = adress,
                AliciAd = alici_ad,
                Cvv = CVV,
                GecerlilikTarihi = son_tarih,
                KartNumarasi = kart_numara,
                Mail = mail,
                Onay = false,
                Tarih = tarih,
                UrunId = urun_id,
                Tel = tel
            });
            _context.SaveChanges();
            MailGonder(mail);
            return RedirectToAction("SatisTamam", "Home", new { urn_id = urun_id });
        }
        public ActionResult SatisTamam()
        {

            return View();
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
