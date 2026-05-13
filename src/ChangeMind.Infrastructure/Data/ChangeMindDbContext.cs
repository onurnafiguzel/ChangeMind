namespace ChangeMind.Infrastructure.Data;

using ChangeMind.Domain.Entities;
using ChangeMind.Domain.Enums;
using ChangeMind.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

public class ChangeMindDbContext : DbContext
{
    public ChangeMindDbContext(DbContextOptions<ChangeMindDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<CoachUser> CoachUsers { get; set; }
    public DbSet<TrainingProgram> TrainingPrograms { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<UserPhoto> UserPhotos { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<WaitingUser> WaitingUsers { get; set; }
    public DbSet<FitnessGoalItem> FitnessGoals { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<NutritionPlan> NutritionPlans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CoachConfiguration());
        modelBuilder.ApplyConfiguration(new CoachUserConfiguration());
        modelBuilder.ApplyConfiguration(new TrainingProgramConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new UserPhotoConfiguration());
        modelBuilder.ApplyConfiguration(new PackageConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new WaitingUserConfiguration());
        modelBuilder.ApplyConfiguration(new FitnessGoalItemConfiguration());
        modelBuilder.ApplyConfiguration(new WorkoutSessionConfiguration());
        modelBuilder.ApplyConfiguration(new FoodConfiguration());
        modelBuilder.ApplyConfiguration(new NutritionPlanConfiguration());

        // Seed Exercise Library
        SeedExerciseLibrary(modelBuilder);
        SeedFoodLibrary(modelBuilder);
    }

    private static void SeedFoodLibrary(ModelBuilder modelBuilder)
    {
        // Per 100g raw weight: (calories, protein, carbs, fat)
        var foods = new List<Food>
        {
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0001"), "Tavuk Göğsü",          165m, 31.0m,  0.0m,  3.6m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0002"), "Dana Bonfile",         217m, 26.1m,  0.0m, 11.8m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0003"), "Hindi Göğsü",          135m, 30.0m,  0.0m,  1.0m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0004"), "Somon",                208m, 20.0m,  0.0m, 13.0m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0005"), "Levrek",               125m, 23.0m,  0.0m,  2.9m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0006"), "Ton Balığı (konserve)",132m, 28.0m,  0.0m,  1.0m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0007"), "Yumurta (bütün)",       78m,  6.5m,  0.5m,  5.5m, "1 adet (orta)", 50m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0008"), "Yumurta Akı",           17m,  3.6m,  0.2m,  0.1m, "1 adet",         33m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0009"), "Süzme Yoğurt",          59m, 10.0m,  3.6m,  0.4m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F000A"), "Yoğurt (tam yağlı)",    61m,  3.5m,  4.7m,  3.3m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F000B"), "Lor Peyniri",           98m, 11.1m,  3.4m,  4.3m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F000C"), "Beyaz Peynir",         264m, 17.0m,  2.0m, 21.0m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F000D"), "Süt (tam yağlı)",       61m,  3.2m,  4.8m,  3.3m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F000E"), "Kefir",                 41m,  3.3m,  4.5m,  1.0m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F000F"), "Whey Protein Tozu",    380m, 80.0m,  7.5m,  3.5m),

            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0010"), "Pirinç Pilavı",        130m,  2.7m, 28.0m,  0.3m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0011"), "Esmer Pirinç",         123m,  2.6m, 25.6m,  0.9m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0012"), "Bulgur Pilavı",         83m,  3.1m, 18.6m,  0.2m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0013"), "Yulaf Ezmesi",         389m, 16.9m, 66.3m,  6.9m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0014"), "Kinoa",                120m,  4.4m, 21.3m,  1.9m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0015"), "Makarna (haşlanmış)",  131m,  5.0m, 25.0m,  1.1m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0016"), "Tam Buğday Ekmeği",     74m,  3.9m, 12.3m,  1.0m, "1 dilim",        30m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0017"), "Beyaz Ekmek",           80m,  2.7m, 14.7m,  1.0m, "1 dilim",        30m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0018"), "Tatlı Patates",         86m,  1.6m, 20.1m,  0.1m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0019"), "Patates",               77m,  2.0m, 17.0m,  0.1m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F001A"), "Yeşil Mercimek",       116m,  9.0m, 20.0m,  0.4m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F001B"), "Kırmızı Mercimek",     116m,  9.0m, 20.0m,  0.4m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F001C"), "Nohut (haşlanmış)",    164m,  8.9m, 27.4m,  2.6m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F001D"), "Kuru Fasulye",         127m,  8.7m, 22.8m,  0.5m),

            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0020"), "Brokoli",               34m,  2.8m,  6.6m,  0.4m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0021"), "Ispanak",               23m,  2.9m,  3.6m,  0.4m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0022"), "Domates",               18m,  0.9m,  3.9m,  0.2m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0023"), "Salatalık",             16m,  0.7m,  3.6m,  0.1m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0024"), "Havuç",                 41m,  0.9m,  9.6m,  0.2m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0025"), "Karnabahar",            25m,  1.9m,  5.0m,  0.3m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0026"), "Kabak",                 17m,  1.2m,  3.1m,  0.3m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0027"), "Biber (yeşil)",         20m,  0.9m,  4.6m,  0.2m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0028"), "Marul",                 15m,  1.4m,  2.9m,  0.2m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0029"), "Roka",                  25m,  2.6m,  3.7m,  0.7m),

            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0030"), "Muz",                  105m,  1.3m, 27.0m,  0.4m, "1 adet (orta)", 118m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0031"), "Elma",                  78m,  0.4m, 21.0m,  0.3m, "1 adet (orta)", 150m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0032"), "Çilek",                 32m,  0.7m,  7.7m,  0.3m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0033"), "Yaban Mersini",         57m,  0.7m, 14.5m,  0.3m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0034"), "Portakal",              65m,  1.3m, 16.3m,  0.2m, "1 adet (orta)", 140m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0035"), "Armut",                102m,  0.6m, 27.0m,  0.2m, "1 adet (orta)", 180m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0036"), "Üzüm",                  69m,  0.7m, 18.0m,  0.2m),

            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0040"), "Badem",                144m,  5.3m,  5.4m, 12.5m, "1 avuç (~25g)", 25m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0041"), "Ceviz",                 33m,  0.8m,  0.7m,  3.3m, "1 adet (çekirdek)", 5m),
            Food.SeedPiece(new Guid("00000000-0000-0000-0000-0000000F0042"), "Fındık",               157m,  3.7m,  4.2m, 15.2m, "1 avuç (~25g)", 25m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0043"), "Fıstık Ezmesi",        588m, 25.0m, 20.0m, 50.0m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0044"), "Zeytinyağı",           884m,  0.0m,  0.0m,100.0m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0045"), "Avokado",              160m,  2.0m,  8.5m, 14.7m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0046"), "Zeytin (siyah)",       115m,  0.8m,  6.3m, 10.7m),
            Food.SeedGrams(new Guid("00000000-0000-0000-0000-0000000F0047"), "Tahin",                595m, 17.0m, 21.2m, 53.8m),
        };

        modelBuilder.Entity<Food>().HasData(foods);
    }

    private static void SeedExerciseLibrary(ModelBuilder modelBuilder)
    {
        var exercises = new List<Exercise>
        {
            // Chest
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010001"), MuscleGroup.Chest, "Barbell Bench Press",
                "Düz bankta bar ile yapılan bu baskı hareketi, pektoral kasların tamamını hedef alır. Omuz genişliğinden biraz daha geniş tutuş ile bar göğse indirilip itilir; göğüs, ön omuz ve triseps birlikte çalışır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010002"), MuscleGroup.Chest, "Incline Dumbbell Press",
                "Eğimli bankta dumbbell ile yapılan bu egzersiz, üst göğüs (klavikular pektoral) kaslarını özellikle aktive eder. Her iki kol bağımsız hareket ettiğinden kas dengesizliklerini gidermeye yardımcı olur."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010003"), MuscleGroup.Chest, "Decline Bench Press",
                "Negatif eğimli bankta gerçekleştirilen bu hareket, alt göğüs kaslarını ve sternal pektoral lifleri yoğun biçimde çalıştırır. Bar, normal bench'e göre daha aşağı bir noktaya temas eder."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010004"), MuscleGroup.Chest, "Cable Flyes",
                "Kablo makinesiyle yapılan bu izolasyon hareketi, göğüs kaslarını en geniş hareket açısında çalıştırır. Sürekli gerilim sağlaması nedeniyle kas bağlantıları üzerinde etkili bir germe ve sıkıştırma hissi yaratır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010005"), MuscleGroup.Chest, "Machine Chest Press",
                "Sabit bir yörüngede hareket eden makine baskısı, özellikle başlangıç seviyesindeki kullanıcılar için güvenli bir göğüs egzersizidir. Yardımcı kasları devre dışı bırakarak pektorallere odaklanmayı kolaylaştırır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000010006"), MuscleGroup.Chest, "Smith Machine Press",
                "Smith makinesi üzerinde yapılan bu baskı hareketi, barın sabit ray üzerinde hareket etmesiyle denge gerektirmeksizin ağır yük kaldırmaya olanak tanır. Göğüs kaslarını kontrollü biçimde çalıştırır."),

            // Back
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020001"), MuscleGroup.Back, "Pull-ups",
                "Vücut ağırlığıyla yapılan bu egzersiz, latissimus dorsi, rombus ve biceps kaslarını eş zamanlı çalıştırır. Avuç içleri dışa bakacak şekilde tutunularak çene barın üzerine çıkarılır; üst sırt gelişiminin temel hareketlerinden biridir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020002"), MuscleGroup.Back, "Lat Pulldown",
                "Kablo makinesiyle geniş bir bar kullanılarak yapılan bu hareket, latissimus dorsi kaslarını etkili biçimde hedef alır. Bar omuz altına kadar çekilir ve sırt kaslarının tam sıkışması hissedilir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020003"), MuscleGroup.Back, "Bent-over Barbell Row",
                "Öne eğik pozisyonda bar ile yapılan bu çekiş hareketi, orta ve üst sırt kaslarını derinlemesine çalıştırır. Gövde 45 derece açıyla sabit tutulurken bar göbek bölgesine doğru çekilir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020004"), MuscleGroup.Back, "T-Bar Row",
                "T-bar aparatı veya landmine kullanılarak yapılan bu sırt egzersizi, kalın ve hacimli sırt kasları geliştirmek için etkilidir. Barın sabit ucunun yere çakılı olması, hareketi daha stabil hale getirir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020005"), MuscleGroup.Back, "Seal Row",
                "Yatay bankta yüz aşağı uzanarak yapılan bu sırt egzersizi, alt trapez ve rombus kaslarını izole eder. Gövde tamamen desteklendiğinden momentum kullanımı minimize edilir ve saf kas kuvveti ölçülür."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000020006"), MuscleGroup.Back, "Chest-Supported Row",
                "Göğüs destekli sırt çekişi, gövde salınımını ortadan kaldırarak orta sırt kaslarını izole şekilde çalıştırır. Eğimli banka yaslanılarak yapılan bu hareket, bel üzerindeki yükü azaltır."),

            // Shoulders
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030001"), MuscleGroup.Shoulders, "Overhead Press",
                "Omuzların üzerine bar veya dumbbell iterek yapılan bu bileşik egzersiz, ön ve orta deltoid ile trisepsi birlikte çalıştırır. Ayakta veya oturarak uygulanabilir; omuz gelişiminin temel hareketlerinden sayılır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030002"), MuscleGroup.Shoulders, "Lateral Raise",
                "Kolların yanlara doğru kaldırıldığı bu izolasyon hareketi, orta deltoid kasını doğrudan hedef alır. Hafif ağırlıkla kontrollü biçimde yapılması, omuz ekleminin korunması açısından kritiktir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030003"), MuscleGroup.Shoulders, "Machine Shoulder Press",
                "Makine üzerinde yapılan omuz baskısı, sabit yörüngesiyle özellikle başlangıç ve rehabilitasyon aşamasındaki sporcular için güvenli bir seçenektir. Deltoid kaslarını dengeli şekilde çalıştırır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030004"), MuscleGroup.Shoulders, "Reverse Pec Deck",
                "Pec deck makinesi üzerinde ters yönde gerçekleştirilen bu hareket, arka deltoid ve rhomboid kaslarını izole eder. Postür bozukluklarını düzeltmeye yardımcı olan bu egzersiz üst sırt sağlığı için önemlidir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000030005"), MuscleGroup.Shoulders, "Arnold Press",
                "Arnold Schwarzenegger tarafından popülerleştirilen bu hareket, ön ve orta deltoidi geniş bir açıda çalıştırır. Dumbbell'lar avuç içleri içe bakacak şekilde başlar ve baskı sırasında dışa döner."),

            // Biceps
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040001"), MuscleGroup.Biceps, "Barbell Curl",
                "Düz bar ile yapılan bu klasik biceps egzersizi, hem kısa hem uzun başı eş zamanlı aktive eder. Dirsekler sabit tutularak bar omuz hizasına kadar kaldırılır; biceps gelişiminin temel hareketidir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040002"), MuscleGroup.Biceps, "Dumbbell Curl",
                "Her kol bağımsız çalıştığından kas dengesizliklerini gidermeye yardımcı olan dumbbell curl, supinasyon hareketi ile biceps'in tüm liflerini aktive eder. Oturarak veya ayakta uygulanabilir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040003"), MuscleGroup.Biceps, "Cable Curl",
                "Kablo makinesiyle yapılan bu curl çeşidi, hareket boyunca sabit gerilim sağlar. Serbest ağırlıklara kıyasla pik kasılma noktasında daha fazla direnç sunar ve kasın tam kasılmasını zorlar."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000040004"), MuscleGroup.Biceps, "Preacher Curl",
                "Vaiz tezgahı üzerinde yapılan bu egzersiz, üst kolun sabitlenmesiyle biceps'i izole eder ve momentum kullanımını engeller. Uzun baş liflerini özellikle gerilim altında tutar."),

            // Triceps
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050001"), MuscleGroup.Triceps, "Tricep Dips",
                "Paralel bar veya bench üzerinde vücut ağırlığıyla yapılan bu egzersiz, trisepsin tüm üç başını çalıştırır. Gövde dik tutulduğunda triseps aktivasyonu artar; öne eğilince göğüs daha fazla devreye girer."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050002"), MuscleGroup.Triceps, "Rope Pushdown",
                "Kablo makinesiyle halat aparatı kullanılarak yapılan bu itme hareketi, trisepsin lateral başını izole eder. Hareketin sonunda halat iki yana açılarak tam kasılma sağlanır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050003"), MuscleGroup.Triceps, "Skull Crusher",
                "Düz bankta bar veya dumbbell ile yapılan bu egzersiz, trisepsin uzun başını maksimum gerilimde çalıştırır. Ağırlık alnın üzerine indirilip dirseğin açılmasıyla yukarı itilir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000050004"), MuscleGroup.Triceps, "Overhead Extension",
                "Kafanın üzerinde uzatma hareketi olarak bilinen bu egzersiz, trisepsin uzun başını en gergin konumda çalıştırır. Dumbbell veya kablo kullanılarak oturarak ya da ayakta uygulanabilir."),

            // Forearms
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000060001"), MuscleGroup.Forearms, "Barbell Curl (Reverse)",
                "Ters tutuşla yapılan bu curl çeşidi, ön kol ekstansör kaslarını ve brachioradialis'i hedef alır. Normal curl'e göre daha zorlu olan bu hareket, bilek ve ön kol kuvvetini artırır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000060002"), MuscleGroup.Forearms, "Wrist Curls",
                "Bileği büküp düzelterek yapılan bu izolasyon hareketi, ön kol fleksör kaslarını doğrudan çalıştırır. Bench veya diz üzerine desteklenerek uygulanır; kavrama gücünü artırmada etkilidir."),

            // Legs
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070001"), MuscleGroup.Legs, "Barbell Back Squat",
                "Bar arka omuzda taşınarak yapılan bu squat çeşidi, quadriceps, gluteus ve hamstring kaslarını bütünsel olarak çalıştırır. Alt vücut gelişiminin temel bileşik hareketi olup yüksek anabolik etki sağlar."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070002"), MuscleGroup.Legs, "Leg Press",
                "Bacak presi makinesi üzerinde sırt desteğiyle yapılan bu egzersiz, quadriceps ve gluteus kaslarını güvenli biçimde çalıştırır. Bel rahatsızlığı olanlar için squat'a alternatif olarak sıkça tercih edilir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070003"), MuscleGroup.Legs, "Leg Extension",
                "Makine üzerinde yapılan bu izolasyon hareketi, quadriceps kasını doğrudan hedef alır. Diz eklemine yönelik kesme kuvveti oluşturduğundan ağırlık seçimine dikkat edilmesi gerekir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070004"), MuscleGroup.Legs, "Hamstring Curl",
                "Yatay veya oturur pozisyonda makine üzerinde yapılan bu egzersiz, arka uyluk kaslarını izole eder. Diz bükülerek topuk kalçaya yaklaştırılır; hamstring gelişiminin temel hareketidir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070005"), MuscleGroup.Legs, "Smith Machine Squat",
                "Smith makinesi rayları üzerinde yapılan bu squat çeşidi, denge bileşenini ortadan kaldırarak bacak kaslarına odaklanmayı sağlar. Farklı ayak pozisyonlarıyla farklı kas grupları vurgulanabilir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070006"), MuscleGroup.Legs, "Hack Squat",
                "45 derecelik makine üzerinde yapılan bu squat varyasyonu, quadriceps kaslarını ön plana çıkarır. Sırt bölgesi makine tarafından desteklendiğinden bel üzerindeki yük minimuma iner."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000070007"), MuscleGroup.Legs, "Leg Sled",
                "Kızak üzerinde yük iterek yapılan bu fonksiyonel güç egzersizi, tüm alt vücut kaslarını explosive biçimde çalıştırır. Güç, dayanıklılık ve kardiyovasküler kapasiteyi aynı anda geliştirir."),

            // Quadriceps
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000080001"), MuscleGroup.Quadriceps, "Front Squat",
                "Bar ön omuzda taşınarak yapılan bu squat çeşidi, arka squat'a kıyasla quadriceps kaslarını daha fazla aktive eder. Gövdenin dik tutulması gerektiğinden esneklik ve mobilite açısından zorlu bir harekettir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000080002"), MuscleGroup.Quadriceps, "Sissy Squat",
                "Topuklar yerden kaldırılarak gövde geriye yatırılan bu egzersiz, quadriceps kaslarını tam açıklıkta izole eder. Diz eklemine olan yük nedeniyle ağırlıksız ya da çok hafif ağırlıkla başlanmalıdır."),

            // Hamstrings
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000090001"), MuscleGroup.Hamstrings, "Romanian Deadlift",
                "Diz hafifçe bükülü tutularak öne doğru eğilme ve düzelme şeklinde yapılan bu hareket, hamstring ve gluteus kaslarını tam aralıkta çalıştırır. Arka zincir gelişimi için en etkili hareketlerden biridir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000090002"), MuscleGroup.Hamstrings, "Good Morning",
                "Bar arka omuzda taşınarak kalçadan öne eğilip düzelme şeklinde yapılan bu egzersiz, hamstring, gluteus ve alt sırt kaslarını birlikte çalıştırır. Düşük ağırlıkla kontrollü yapılması önerilir."),

            // Glutes
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000100001"), MuscleGroup.Glutes, "Hip Thrust",
                "Sırt bench'e yaslanarak barın kalça üzerine yerleştirilmesiyle yapılan bu hareket, gluteus maximus kasını maksimum kasılmada çalıştırır. Kalça yükseltme hareketi olarak bilinen en etkili glute egzersizleri arasındadır."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000100002"), MuscleGroup.Glutes, "Bulgarian Split Squat",
                "Arka ayak bench üzerinde desteklenirken ön ayakla yapılan bu tek bacak squat varyasyonu, gluteus ve quadriceps kaslarını dengeli biçimde çalıştırır. Kas dengesizliklerini gidermeye yardımcı olur."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000100003"), MuscleGroup.Glutes, "Glute-Focused Leg Press",
                "Bacak presinde ayakların platformun üst kısmına yerleştirilmesiyle gluteus kaslarının aktivasyonu artırılır. Kalça açısı değiştirilerek hamstring ve gluteus'un farklı lifleri vurgulanır."),

            // Calves
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000110001"), MuscleGroup.Calves, "Calf Raise",
                "Ayak uçlarına yükselerek yapılan bu egzersiz, gastrocnemius ve soleus kaslarını çalıştırır. Topuk aşağıda serbest bırakılarak tam aralıkta hareket edilmesi kas gelişimi için kritik öneme sahiptir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000110002"), MuscleGroup.Calves, "Machine Calf Raise",
                "Makine üzerinde omuzlardan yüklenerek yapılan bu baldır egzersizi, gastrocnemius kasını izole eder. Kontrollü negatif fazı sayesinde eksantrik stimülasyon yüksektir."),

            // Abs
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000120001"), MuscleGroup.Abs, "Cable Crunch",
                "Kablo makinesi önünde diz üstü pozisyonda yapılan bu karın egzersizi, rectus abdominis'i yoğun biçimde çalıştırır. Ayarlanabilir ağırlık sayesinde kas gelişimine uygun direnç kolayca belirlenir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000120002"), MuscleGroup.Abs, "Machine Crunch",
                "Karın kası makinesi üzerinde yapılan bu izolasyon hareketi, rectus abdominis kasını destekli ve kontrollü biçimde çalıştırır. Başlangıç seviyeleri için güvenli ve etkili bir karın egzersizidir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000120003"), MuscleGroup.Abs, "Ab Wheel Rollout",
                "Karın tekeri ile yapılan bu ileri düzey egzersiz, rectus abdominis ve transversus abdominis kaslarını yüksek gerilimde çalıştırır. Gövde stabilitesi ve karın kası kuvveti için son derece etkilidir."),

            // Obliques
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000130001"), MuscleGroup.Obliques, "Cable Woodchop",
                "Kablo makinesiyle diyagonal bir hareket yolunda gerçekleştirilen bu egzersiz, iç ve dış oblik kasları döndürme hareketi sırasında aktive eder. Rotasyon kuvveti ve gövde stabilitesi için kritik bir harekettir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000130002"), MuscleGroup.Obliques, "Landmine Rotation",
                "Landmine aparatına takılı barın iki elle tutularak yanlara döndürülmesiyle yapılan bu hareket, oblik kasları ve rotator core kaslarını işlevsel biçimde çalıştırır. Sporsal performans gelişimi için değerlidir."),

            // Lower Back
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000140001"), MuscleGroup.LowerBack, "Deadlift",
                "Yerden bar kaldırma olarak bilinen bu bileşik egzersiz, alt sırt, gluteus, hamstring ve quadriceps kaslarını eş zamanlı çalıştırır. Vücuttaki en fazla kas kütlesini aktive eden hareketlerden biri olup anabolik etkisi yüksektir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000140002"), MuscleGroup.LowerBack, "Hyperextension",
                "Hiperextension tezgahında gövde aşağı sarkıtılıp kaldırılarak yapılan bu hareket, erector spinae ve alt sırt kaslarını güçlendirir. Bel sağlığını desteklemek ve deadlift performansını artırmak için etkilidir."),

            // Traps
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000150001"), MuscleGroup.Traps, "Barbell Shrugs",
                "Bar tutularak omuzların yukarı kaldırılıp bırakılması şeklinde yapılan bu egzersiz, üst trapez kasını izole eder. Ağır ağırlıklarla yapılabilen bu hareket, omuz ve boyun bölgesinin kalın görünümünü destekler."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000150002"), MuscleGroup.Traps, "Dumbbell Shrugs",
                "Her iki elde dumbbell tutularak yapılan bu omuz kaldırma hareketi, trapez kasını dengeli biçimde çalıştırır. Dumbbell'ların vücudun yanında serbestçe asılı kalması, hareket açısını artırır."),

            // Latissimus Dorsi
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000160001"), MuscleGroup.LatissimusDorsi, "Wide Grip Lat Pulldown",
                "Geniş tutuşla kablo üzerinden yapılan bu çekiş, latissimus dorsi kasını en geniş açıyla aktive eder. Bar göğüse indirilirken dirsekler yanlara doğru açılır ve sırt kaslarının tam kasılması hissedilir."),
            CreateExerciseForSeed(new Guid("00000000-0000-0000-0000-000000160002"), MuscleGroup.LatissimusDorsi, "Underhand Lat Pulldown",
                "Ters tutuşla yapılan bu lat pulldown çeşidi, latissimus dorsi'nin yanı sıra biceps kasını da yoğun biçimde aktive eder. Alt lat liflerini hedef alması bakımından geniş tutuşa tamamlayıcı niteliktedir.")
        };

        modelBuilder.Entity<Exercise>().HasData(exercises);
    }

    private static Exercise CreateExerciseForSeed(Guid id, MuscleGroup muscleGroup, string name, string? description = null)
        => Exercise.Seed(id, name, muscleGroup, description);
}
