# Traffic Fine Management

Traffic Fine Management; araçları, şoför atamalarını ve trafik cezalarının onay süreçlerini yönetmek için geliştirilmiş bir **.NET 10 modüler monolith** uygulamasıdır.

Proje ilk kez Development ortamında çalıştırıldığında veritabanı migration'ları ve demo verileri otomatik oluşturulur. Böylece uygulamayı açan biri farklı rollerle giriş yaparak bütün iş akışlarını doğrudan deneyebilir.

## Canlı demo

Uygulamanın sunucuya kurulmuş sürümüne aşağıdaki adresten erişilebilir:

- Giriş ekranı: [https://traffic.wordlope.com/login](https://traffic.wordlope.com/login)

Kontrol amacıyla seed edilen hesapların ortak parolası:

```text
qvvSRLXcXoAxKYhcfrYd49AdqnEcB7LE
```

Örneğin bütün özellikleri incelemek için `admin` kullanıcı adıyla giriş yapılabilir. Diğer kullanıcı adları ve rol bilgileri [Seed kullanıcıları](#seed-kullanıcıları) bölümünde listelenmiştir.

> Canlı ortam yalnızca demo ve değerlendirme amaçlıdır. Giriş bilgileri herkese açık olduğundan gerçek veya hassas veri kullanılmamalıdır.

## Temel özellikler

- Cookie tabanlı kullanıcı girişi ve rol bazlı yetkilendirme
- Araç oluşturma ve araç tipi yönetimi
- Şoförlerin araçlara tarih bazlı atanması
- Bir şoförün aynı anda yalnızca bir aktif araç kullanabilmesi
- Araç kullanım geçmişinin görüntülenmesi
- Şoförün kendi cezasını oluşturabilmesi
- Ceza görevlisinin araç ve tarih üzerinden tüm şoförler için ceza oluşturabilmesi
- Yönetici ve finans onay aşamaları
- Aşamaya bağlı ceza reddetme işlemleri
- Ceza işlem geçmişinin görüntülenmesi
- Transactional outbox ve modüller arası event tabanlı veri senkronizasyonu
- Otomatik ve tekrar çalıştırılabilir demo seed verileri

## Kullanılan teknolojiler

- .NET 10 / ASP.NET Core MVC ve Web API
- PostgreSQL
- Entity Framework Core
- Dapper
- MediatR
- Autofac
- FluentValidation
- Quartz.NET
- xUnit ve Testcontainers
- DDD, CQRS, modüler monolith ve transactional outbox yaklaşımları

## Proje modülleri

| Modül | Sorumluluk |
| --- | --- |
| `Users` | Kullanıcılar, parola hashleme, kimlik doğrulama ve roller |
| `Vehicles` | Araçlar, araç tipleri ve şoför kullanım geçmişleri |
| `TrafficFine` | Ceza oluşturma, onaylama, reddetme ve tamamlama süreçleri |

Ortak domain ve infrastructure bileşenleri `src/BuildingBlocks` altında bulunur. Modüller kendi domain, application, infrastructure ve integration event katmanlarına sahiptir.

## Gereksinimler

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- PostgreSQL
- Git
- Entegrasyon testlerini çalıştırmak için Docker

Kurulu .NET sürümünü kontrol etmek için:

```bash
dotnet --version
```

Çıktının `10.x` olması gerekir.

## Kurulum ve çalıştırma

### 1. Projeyi klonlayın

```bash
git clone <repository-url>
cd TrafficFineManagement
```

### 2. PostgreSQL veritabanını oluşturun

PostgreSQL çalışırken aşağıdaki komutu kullanın:

```bash
psql -h localhost -U postgres -c 'CREATE DATABASE traffic_fine_management;'
```

Veritabanı daha önce oluşturulduysa bu adımı tekrar uygulamanız gerekmez.

### 3. Bağlantı bilgilerini tanımlayın

Üç modül aynı PostgreSQL veritabanını kullanır. `POSTGRES_PASSWORD` bölümünü kendi PostgreSQL parolanızla değiştirin.

macOS/Linux:

```bash
export ConnectionStrings__VehiclesConnectionString='Host=localhost;Port=5432;Database=traffic_fine_management;Username=postgres;Password=POSTGRES_PASSWORD'
export ConnectionStrings__TrafficFineConnectionString='Host=localhost;Port=5432;Database=traffic_fine_management;Username=postgres;Password=POSTGRES_PASSWORD'
export ConnectionStrings__UsersConnectionString='Host=localhost;Port=5432;Database=traffic_fine_management;Username=postgres;Password=POSTGRES_PASSWORD'
```

Windows PowerShell:

```powershell
$env:ConnectionStrings__VehiclesConnectionString='Host=localhost;Port=5432;Database=traffic_fine_management;Username=postgres;Password=POSTGRES_PASSWORD'
$env:ConnectionStrings__TrafficFineConnectionString='Host=localhost;Port=5432;Database=traffic_fine_management;Username=postgres;Password=POSTGRES_PASSWORD'
$env:ConnectionStrings__UsersConnectionString='Host=localhost;Port=5432;Database=traffic_fine_management;Username=postgres;Password=POSTGRES_PASSWORD'
```

### 4. Bağımlılıkları yükleyin

```bash
dotnet restore TrafficFineManagement.slnx
```

### 5. Uygulamayı çalıştırın

```bash
dotnet run --project src/API/TrafficFineManagement.API/TrafficFineManagement.API.csproj --launch-profile http
```

Uygulama şu adreslerde kullanılabilir:

- Giriş ekranı: [http://localhost:5090/login](http://localhost:5090/login)
- Cezalar: [http://localhost:5090/traffic-fines](http://localhost:5090/traffic-fines)
- Araçlar: [http://localhost:5090/vehicles](http://localhost:5090/vehicles)

`http` launch profili ortamı otomatik olarak `Development` yapar. Bu ortamda:

1. `src/Database/Scripts` altındaki SQL migration'ları numara sırasıyla uygulanır.
2. Uygulanan migration'lar `app."SchemaMigrations"` tablosuna kaydedilir.
3. Kullanıcı, araç, kullanım geçmişi ve ceza demo verileri oluşturulur.
4. Seed işlemleri tekrar çalıştırılabilir; uygulama her açıldığında aynı kayıtlar yeniden üretilmez.

## Roller ve yetkileri

Tüm giriş yapmış kullanıcılar araçları, araç geçmişlerini, cezaları ve ceza geçmişlerini görüntüleyebilir.

| Rol | Yapabildikleri |
| --- | --- |
| **Şoför (`Driver`)** | Kendi yediği cezayı oluşturur. Araç seçmez; sistem ceza tarihindeki aktif/geçmiş araç kullanımından aracı otomatik belirler. |
| **Yönetici (`Manager`)** | Yeni araç oluşturur, boş araca şoför atar, aktif araç kullanımını tamamlar. `Created` aşamasındaki cezayı onaylar veya reddeder. |
| **Finansçı (`Finance`)** | Yalnızca yönetici tarafından onaylanmış (`ManagerApproved`) cezayı onaylar veya reddeder. |
| **Ceza görevlisi (`FineOfficer`)** | Araç ve tarih seçerek tüm şoförler adına ceza oluşturur. Sistem o tarihte aracı kullanan şoförü otomatik bulur. Finans onayından geçen cezayı tamamlar. |
| **Admin (`Admin`)** | Bütün yetkilere sahiptir. Kullanıcı ve şoför oluşturabilir, araç yönetebilir, ceza oluşturabilir ve cezanın tüm onay adımlarını gerçekleştirebilir. |

Admin, araç atama penceresinden yeni bir şoför oluşturduğunda şoför doğrudan seçili araca atanır. Bu alandan yalnızca `Driver` rolünde kullanıcı oluşturulur. API üzerinden diğer rollerde kullanıcı oluşturma yetkisi de yalnızca Admin'e aittir.

## Ceza iş akışı

```text
Created
   │
   ├── Yönetici reddeder ──────────────> Rejected / Pasif
   │
   └── Yönetici onaylar
              │
              v
       ManagerApproved
              │
              ├── Finansçı reddeder ───> Rejected / Pasif
              │
              └── Finansçı onaylar
                         │
                         v
                 FinanceApproved
                         │
                         └── Ceza görevlisi tamamlar
                                      │
                                      v
                              Completed / Pasif
```

- Ret işlemi yalnızca ilgili onay aşamasında yapılabilir.
- Yönetici sadece `Created` aşamasında işlem yapabilir.
- Finansçı sadece `ManagerApproved` aşamasında işlem yapabilir.
- Ceza görevlisi sadece `FinanceApproved` aşamasındaki cezayı tamamlayabilir.
- Admin bütün aşamalarda ilgili işlemi gerçekleştirebilir.

## Seed kullanıcıları

Development ortamındaki bütün seed kullanıcılarının parolası:

```text
Test123!
```

| Ad soyad | Kullanıcı adı | Rol |
| --- | --- | --- |
| Test Şoför | `driver` | Şoför |
| Ayşe Yılmaz | `driver.ayse` | Şoför |
| Mehmet Kaya | `driver.mehmet` | Şoför |
| Elif Demir | `driver.elif` | Şoför |
| Can Aydın | `driver.can` | Şoför |
| Test Yönetici | `manager` | Yönetici |
| Test Finansçı | `finance` | Finansçı |
| Test Ceza Görevlisi | `fineofficer` | Ceza görevlisi |
| Test Admin | `admin` | Admin |

> Seed hesapları yalnızca geliştirme ve demo amaçlıdır. Production ortamında kullanılmamalıdır.

## Seed araçları ve kullanım senaryoları

| Plaka | Araç | Tip | Kullanım durumu |
| --- | --- | --- | --- |
| `34 DEMO 001` | Toyota Corolla | Binek | `driver` aktif kullanıcıdır; `driver.can` için geçmiş kullanım bulunur. |
| `34 DEMO 002` | Volvo FH16 | Çekici | `driver.ayse` için tamamlanmış kullanım geçmişi bulunur. |
| `34 DEMO 003` | Schmitz S.CF | Dorse | `driver.mehmet` için tamamlanmış kullanım geçmişi bulunur. |
| `34 DEMO 004` | Renault Clio | Kiralık araç | `driver.elif` için tamamlanmış kullanım geçmişi bulunur. |

Bir şoför aynı anda yalnızca bir aktif araca atanabilir. Kullanımı tamamlanan şoför daha sonra aynı veya farklı bir araca yeniden atanabilir.

## Seed ceza senaryoları

| İhlal kodu | Güncel işlem | Şoför | Araç |
| --- | --- | --- | --- |
| `DEMO-CREATED` | Yönetici onayı bekliyor | `driver` | `34 DEMO 001` |
| `DEMO-MANAGER` | Finans onayı bekliyor | `driver.ayse` | `34 DEMO 002` |
| `DEMO-FINANCE` | Tamamlanmayı bekliyor | `driver.mehmet` | `34 DEMO 003` |
| `DEMO-REJECTED` | Reddedildi | `driver.elif` | `34 DEMO 004` |
| `DEMO-COMPLETED` | Tamamlandı | `driver.can` | `34 DEMO 001` |

Her cezanın tarihi, ilgili şoförün aracı kullandığı tarih aralığına denk gelecek şekilde oluşturulur.

## Yapılandırma seçenekleri

| Ayar | Açıklama |
| --- | --- |
| `DatabaseMigrations:Enabled` | SQL migration'larının uygulama başlangıcında çalışmasını belirler. Development ortamında `true` değerindedir. |
| `UserSeed:Enabled` | Seed kullanıcılarının oluşturulmasını belirler. |
| `UserSeed:Password` | Seed kullanıcılarının ortak parolasıdır. |
| `DemoDataSeed:Enabled` | Demo araç, kullanım ve ceza senaryolarını etkinleştirir. |
| `Quartz:Enabled` | Transactional outbox işlerinin Quartz ile çalışmasını belirler. |

Production ortamında migration ve seed seçenekleri varsayılan olarak kapalıdır.

## Testler

Docker çalışırken tüm testleri aşağıdaki komutla çalıştırabilirsiniz:

```bash
dotnet test TrafficFineManagement.slnx
```

Test paketi şunları doğrular:

- Users ve Vehicles domain kuralları
- Kimlik doğrulama ve rol yetkileri
- Araç atama ve tek aktif araç kuralı
- Ceza oluşturma ve tüm onay akışı
- Transactional outbox projeksiyonları
- Migration'ların temiz veritabanına uygulanması
- Demo seed verilerinin eksiksiz ve ilişkisel olarak doğru oluşturulması

Entegrasyon testleri Testcontainers kullanarak geçici bir PostgreSQL container'ı başlatır ve test tamamlandığında kaldırır.

## Production notları

- Seed kullanıcılarını ve demo verilerini kapalı tutun.
- Bağlantı bilgilerini kaynak kodda saklamayın; environment variable veya güvenli bir secret store kullanın.
- HTTPS kullanın ve cookie güvenlik ayarlarını deployment ortamına göre yapılandırın.
- Migration çalıştırma politikasını deployment sürecinize göre belirleyin.
