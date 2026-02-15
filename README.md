# 🍰 Baker - Pastane Yönetim Sistemi

Modern bir pastane işletmesi için geliştirilmiş kapsamlı web tabanlı yönetim sistemi. ASP.NET Core 8.0 ve Razor Pages teknolojileri kullanılarak oluşturulmuştur.

## 📋 Proje Hakkında

Baker, pastane ve fırın işletmelerinin ürün yönetimi, kategori organizasyonu, müşteri mesajları ve hakkımızda bilgilerini tek bir platformdan yönetmesini sağlayan modern bir web uygulamasıdır.

## 🚀 Özellikler

### Yönetim Paneli
- **Dashboard**: Genel istatistikler ve özet bilgiler
  - Toplam ürün, kategori ve mesaj sayıları
  - Okunmamış mesaj bildirimleri
  - Kategori bazlı ürün dağılımı
  - Son eklenen ürünler ve mesajlar

### Ürün Yönetimi
- Ürün ekleme, düzenleme ve silme
- Ürün bilgileri:
  - Ürün adı
  - Fiyat
  - Görsel (URL)
  - Açıklama
  - Kategori
  - Aktiflik durumu
- Kategori bazlı ürün filtreleme

### Kategori Yönetimi
- Kategori ekleme ve düzenleme
- Kategori bazlı ürün organizasyonu

### Hakkımızda Yönetimi
- Şirket bilgilerini düzenleme
- Çoklu görsel desteği (2 görsel)
- Başlık ve açıklama
- Dinamik madde ekleme (maksimum 4 madde)
- Gerçek zamanlı madde sayısı takibi

### Mesaj Yönetimi
- Gelen mesajları görüntüleme
- Mesaj okuma/okunmadı durumu
- Son mesajlar listesi

## 🛠️ Teknolojiler

### Backend
- **.NET 8.0**
- **ASP.NET Core Razor Pages**
- **C# 12.0**
- **HttpClient** - API iletişimi
- **Newtonsoft.Json** - JSON serileştirme

### Frontend
- **Bootstrap** - Responsive tasarım
- **JavaScript** - Dinamik form yönetimi
- **Razor View Components** - Yeniden kullanılabilir bileşenler

### API
- RESTful API yapısı
- HTTPS protokolü (localhost:7136)

## 📁 Proje Yapısı

## 🔧 Kurulum

### Gereksinimler
- .NET 8.0 SDK
- Visual Studio 2022 veya üzeri
- API servisi (localhost:7136)

### Adımlar

1. **Projeyi Klonlayın**
2. **Bağımlılıkları Yükleyin**
3. **API Ayarları**
- API servisinin `https://localhost:7136` adresinde çalıştığından emin olun
- Gerekirse Controllers içindeki API endpoint'lerini güncelleyin

4. **Uygulamayı Çalıştırın**
5. **Tarayıcıda Açın**

## 💡 Kullanım

### Ürün Ekleme
1. Yönetim panelinden "Ürünler" bölümüne gidin
2. "Yeni Ürün Ekle" butonuna tıklayın
3. Ürün bilgilerini doldurun
4. Kategori seçin
5. "Kaydet" butonuna tıklayın

### Hakkımızda Düzenleme
1. "Hakkımızda" bölümüne gidin
2. Başlık, görseller ve açıklama bilgilerini girin
3. "+ Madde Ekle" ile madde ekleyin (max 4)
4. "Kaydet" ile değişiklikleri kaydedin

### Dashboard İstatistikleri
- Toplam ürün, kategori ve mesaj sayıları otomatik güncellenir
- Kategori bazlı ürün dağılımı grafiksel olarak gösterilir
- Son eklenen ürünler ve mesajlar listelenir

## 🔐 Güvenlik

- **Anti-forgery Token**: Form güvenliği için CSRF koruması
- **HTTPS**: Güvenli veri iletişimi
- **Model Validation**: Veri doğrulama
- **Error Handling**: Kapsamlı hata yönetimi

## 📱 Responsive Tasarım

- Masaüstü, tablet ve mobil cihazlarda uyumlu
- Bootstrap grid sistemi
- Responsive form elemanları

## 🤝 Katkıda Bulunma

1. Bu depoyu fork edin
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje açık kaynaklıdır ve herhangi bir lisans altında dağıtılmamaktadır.

## 👤 Geliştirici

**Begüm**
- GitHub: [@begum02](https://github.com/begum02)

## 📞 İletişim

Proje ile ilgili sorularınız için GitHub Issues kullanabilirsiniz.

---

⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!
