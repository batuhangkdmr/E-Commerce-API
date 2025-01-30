# E-Ticaret Platformu (ASP.NET Core 8 MVC)

Bu proje, **ASP.NET Core 8 MVC** kullanılarak geliştirilmiş modüler ve sürdürülebilir bir **e-ticaret platformudur**. Kullanıcı oturum yönetimi, sipariş süreçleri, yetkilendirme sistemleri ve API servisleri gibi kritik işlevler içermektedir.

## 🔧 Kullanılan Teknolojiler

- **ASP.NET Core 8 MVC** - Web uygulaması geliştirme
- **Entity Framework Core** - ORM ve veritabanı işlemleri
- **SQL Server** - Veritabanı yönetimi
- **Onion Architecture** - Modüler ve katmanlı mimari
- **JWT Token** - Kullanıcı kimlik doğrulama ve oturum yönetimi
- **MediatR** - CQRS deseni ile katmanlar arası iletişim
- **Azure DevOps** - CI/CD ve proje yönetimi
- **Swagger** - API dokümantasyonu

## 🛠 Mimari Yapı

Bu proje, **Onion Architecture** prensipleriyle geliştirilmiştir. İş mantığını merkezde tutarak bağımlılıkları dış katmanlara yönlendiren bu mimari sayesinde, **bağımsız ve modüler bir sistem** oluşturulmuştur.

### 🗂️ Klasör Yapısı

- **Core** - İş mantığını içeren katman
- **Data** - Veritabanı işlemleri ve repository yapısı
- **TestAPI** - API testleri ve entegrasyon testleri
- **ECommerceAPI** - Ana API servisleri

## 🔄 Kurulum ve Çalıştırma

Projeyi çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

1. **Depoyu Klonla:**

   ```bash
   git clone <repository-url>
   cd ECommerceApp
   ```

2. **Gerekli Bağımlılıkları Kur:**

   ```bash
   dotnet restore
   ```

3. **Veritabanı Migrasyonlarını Çalıştır:**

   ```bash
   dotnet ef database update
   ```

4. **Projeyi Derle ve Çalıştır:**

   ```bash
   dotnet run
   ```

## 👨‍💻 Katkı Sağlama

Projeye katkıda bulunmak isterseniz:

1. **Fork** yapın
2. Yeni bir **branch** oluşturun
3. Değişikliklerinizi yapın ve **commit** edin
4. **Pull Request (PR)** gönderin

## 🔖 Lisans

Bu proje **MIT Lisansı** ile lisanslanmıştır.

---

Eğer herhangi bir sorunuz veya geri bildiriminiz varsa, lütfen benimle iletişime geçin! ✨

