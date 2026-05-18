using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeMind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Description sütunu zaten 20260414133100_exercise-library-crud migration'ında eklendi.
            // Burada yalnızca seed açıklamalarını günceller.

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010001"),
                column: "Description",
                value: "Düz bankta bar ile yapılan bu baskı hareketi, pektoral kasların tamamını hedef alır. Omuz genişliğinden biraz daha geniş tutuş ile bar göğse indirilip itilir; göğüs, ön omuz ve triseps birlikte çalışır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010002"),
                column: "Description",
                value: "Eğimli bankta dumbbell ile yapılan bu egzersiz, üst göğüs (klavikular pektoral) kaslarını özellikle aktive eder. Her iki kol bağımsız hareket ettiğinden kas dengesizliklerini gidermeye yardımcı olur.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010003"),
                column: "Description",
                value: "Negatif eğimli bankta gerçekleştirilen bu hareket, alt göğüs kaslarını ve sternal pektoral lifleri yoğun biçimde çalıştırır. Bar, normal bench'e göre daha aşağı bir noktaya temas eder.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010004"),
                column: "Description",
                value: "Kablo makinesiyle yapılan bu izolasyon hareketi, göğüs kaslarını en geniş hareket açısında çalıştırır. Sürekli gerilim sağlaması nedeniyle kas bağlantıları üzerinde etkili bir germe ve sıkıştırma hissi yaratır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010005"),
                column: "Description",
                value: "Sabit bir yörüngede hareket eden makine baskısı, özellikle başlangıç seviyesindeki kullanıcılar için güvenli bir göğüs egzersizidir. Yardımcı kasları devre dışı bırakarak pektorallere odaklanmayı kolaylaştırır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010006"),
                column: "Description",
                value: "Smith makinesi üzerinde yapılan bu baskı hareketi, barın sabit ray üzerinde hareket etmesiyle denge gerektirmeksizin ağır yük kaldırmaya olanak tanır. Göğüs kaslarını kontrollü biçimde çalıştırır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020001"),
                column: "Description",
                value: "Vücut ağırlığıyla yapılan bu egzersiz, latissimus dorsi, rombus ve biceps kaslarını eş zamanlı çalıştırır. Avuç içleri dışa bakacak şekilde tutunularak çene barın üzerine çıkarılır; üst sırt gelişiminin temel hareketlerinden biridir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020002"),
                column: "Description",
                value: "Kablo makinesiyle geniş bir bar kullanılarak yapılan bu hareket, latissimus dorsi kaslarını etkili biçimde hedef alır. Bar omuz altına kadar çekilir ve sırt kaslarının tam sıkışması hissedilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020003"),
                column: "Description",
                value: "Öne eğik pozisyonda bar ile yapılan bu çekiş hareketi, orta ve üst sırt kaslarını derinlemesine çalıştırır. Gövde 45 derece açıyla sabit tutulurken bar göbek bölgesine doğru çekilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020004"),
                column: "Description",
                value: "T-bar aparatı veya landmine kullanılarak yapılan bu sırt egzersizi, kalın ve hacimli sırt kasları geliştirmek için etkilidir. Barın sabit ucunun yere çakılı olması, hareketi daha stabil hale getirir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020005"),
                column: "Description",
                value: "Yatay bankta yüz aşağı uzanarak yapılan bu sırt egzersizi, alt trapez ve rombus kaslarını izole eder. Gövde tamamen desteklendiğinden momentum kullanımı minimize edilir ve saf kas kuvveti ölçülür.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020006"),
                column: "Description",
                value: "Göğüs destekli sırt çekişi, gövde salınımını ortadan kaldırarak orta sırt kaslarını izole şekilde çalıştırır. Eğimli banka yaslanılarak yapılan bu hareket, bel üzerindeki yükü azaltır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030001"),
                column: "Description",
                value: "Omuzların üzerine bar veya dumbbell iterek yapılan bu bileşik egzersiz, ön ve orta deltoid ile trisepsi birlikte çalıştırır. Ayakta veya oturarak uygulanabilir; omuz gelişiminin temel hareketlerinden sayılır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030002"),
                column: "Description",
                value: "Kolların yanlara doğru kaldırıldığı bu izolasyon hareketi, orta deltoid kasını doğrudan hedef alır. Hafif ağırlıkla kontrollü biçimde yapılması, omuz ekleminin korunması açısından kritiktir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030003"),
                column: "Description",
                value: "Makine üzerinde yapılan omuz baskısı, sabit yörüngesiyle özellikle başlangıç ve rehabilitasyon aşamasındaki sporcular için güvenli bir seçenektir. Deltoid kaslarını dengeli şekilde çalıştırır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030004"),
                column: "Description",
                value: "Pec deck makinesi üzerinde ters yönde gerçekleştirilen bu hareket, arka deltoid ve rhomboid kaslarını izole eder. Postür bozukluklarını düzeltmeye yardımcı olan bu egzersiz üst sırt sağlığı için önemlidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030005"),
                column: "Description",
                value: "Arnold Schwarzenegger tarafından popülerleştirilen bu hareket, ön ve orta deltoidi geniş bir açıda çalıştırır. Dumbbell'lar avuç içleri içe bakacak şekilde başlar ve baskı sırasında dışa döner.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040001"),
                column: "Description",
                value: "Düz bar ile yapılan bu klasik biceps egzersizi, hem kısa hem uzun başı eş zamanlı aktive eder. Dirsekler sabit tutularak bar omuz hizasına kadar kaldırılır; biceps gelişiminin temel hareketidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040002"),
                column: "Description",
                value: "Her kol bağımsız çalıştığından kas dengesizliklerini gidermeye yardımcı olan dumbbell curl, supinasyon hareketi ile biceps'in tüm liflerini aktive eder. Oturarak veya ayakta uygulanabilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040003"),
                column: "Description",
                value: "Kablo makinesiyle yapılan bu curl çeşidi, hareket boyunca sabit gerilim sağlar. Serbest ağırlıklara kıyasla pik kasılma noktasında daha fazla direnç sunar ve kasın tam kasılmasını zorlar.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040004"),
                column: "Description",
                value: "Vaiz tezgahı üzerinde yapılan bu egzersiz, üst kolun sabitlenmesiyle biceps'i izole eder ve momentum kullanımını engeller. Uzun baş liflerini özellikle gerilim altında tutar.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050001"),
                column: "Description",
                value: "Paralel bar veya bench üzerinde vücut ağırlığıyla yapılan bu egzersiz, trisepsin tüm üç başını çalıştırır. Gövde dik tutulduğunda triseps aktivasyonu artar; öne eğilince göğüs daha fazla devreye girer.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050002"),
                column: "Description",
                value: "Kablo makinesiyle halat aparatı kullanılarak yapılan bu itme hareketi, trisepsin lateral başını izole eder. Hareketin sonunda halat iki yana açılarak tam kasılma sağlanır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050003"),
                column: "Description",
                value: "Düz bankta bar veya dumbbell ile yapılan bu egzersiz, trisepsin uzun başını maksimum gerilimde çalıştırır. Ağırlık alnın üzerine indirilip dirseğin açılmasıyla yukarı itilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050004"),
                column: "Description",
                value: "Kafanın üzerinde uzatma hareketi olarak bilinen bu egzersiz, trisepsin uzun başını en gergin konumda çalıştırır. Dumbbell veya kablo kullanılarak oturarak ya da ayakta uygulanabilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000060001"),
                column: "Description",
                value: "Ters tutuşla yapılan bu curl çeşidi, ön kol ekstansör kaslarını ve brachioradialis'i hedef alır. Normal curl'e göre daha zorlu olan bu hareket, bilek ve ön kol kuvvetini artırır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000060002"),
                column: "Description",
                value: "Bileği büküp düzelterek yapılan bu izolasyon hareketi, ön kol fleksör kaslarını doğrudan çalıştırır. Bench veya diz üzerine desteklenerek uygulanır; kavrama gücünü artırmada etkilidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070001"),
                column: "Description",
                value: "Bar arka omuzda taşınarak yapılan bu squat çeşidi, quadriceps, gluteus ve hamstring kaslarını bütünsel olarak çalıştırır. Alt vücut gelişiminin temel bileşik hareketi olup yüksek anabolik etki sağlar.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070002"),
                column: "Description",
                value: "Bacak presi makinesi üzerinde sırt desteğiyle yapılan bu egzersiz, quadriceps ve gluteus kaslarını güvenli biçimde çalıştırır. Bel rahatsızlığı olanlar için squat'a alternatif olarak sıkça tercih edilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070003"),
                column: "Description",
                value: "Makine üzerinde yapılan bu izolasyon hareketi, quadriceps kasını doğrudan hedef alır. Diz eklemine yönelik kesme kuvveti oluşturduğundan ağırlık seçimine dikkat edilmesi gerekir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070004"),
                column: "Description",
                value: "Yatay veya oturur pozisyonda makine üzerinde yapılan bu egzersiz, arka uyluk kaslarını izole eder. Diz bükülerek topuk kalçaya yaklaştırılır; hamstring gelişiminin temel hareketidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070005"),
                column: "Description",
                value: "Smith makinesi rayları üzerinde yapılan bu squat çeşidi, denge bileşenini ortadan kaldırarak bacak kaslarına odaklanmayı sağlar. Farklı ayak pozisyonlarıyla farklı kas grupları vurgulanabilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070006"),
                column: "Description",
                value: "45 derecelik makine üzerinde yapılan bu squat varyasyonu, quadriceps kaslarını ön plana çıkarır. Sırt bölgesi makine tarafından desteklendiğinden bel üzerindeki yük minimuma iner.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070007"),
                column: "Description",
                value: "Kızak üzerinde yük iterek yapılan bu fonksiyonel güç egzersizi, tüm alt vücut kaslarını explosive biçimde çalıştırır. Güç, dayanıklılık ve kardiyovasküler kapasiteyi aynı anda geliştirir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000080001"),
                column: "Description",
                value: "Bar ön omuzda taşınarak yapılan bu squat çeşidi, arka squat'a kıyasla quadriceps kaslarını daha fazla aktive eder. Gövdenin dik tutulması gerektiğinden esneklik ve mobilite açısından zorlu bir harekettir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000080002"),
                column: "Description",
                value: "Topuklar yerden kaldırılarak gövde geriye yatırılan bu egzersiz, quadriceps kaslarını tam açıklıkta izole eder. Diz eklemine olan yük nedeniyle ağırlıksız ya da çok hafif ağırlıkla başlanmalıdır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000090001"),
                column: "Description",
                value: "Diz hafifçe bükülü tutularak öne doğru eğilme ve düzelme şeklinde yapılan bu hareket, hamstring ve gluteus kaslarını tam aralıkta çalıştırır. Arka zincir gelişimi için en etkili hareketlerden biridir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000090002"),
                column: "Description",
                value: "Bar arka omuzda taşınarak kalçadan öne eğilip düzelme şeklinde yapılan bu egzersiz, hamstring, gluteus ve alt sırt kaslarını birlikte çalıştırır. Düşük ağırlıkla kontrollü yapılması önerilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100001"),
                column: "Description",
                value: "Sırt bench'e yaslanarak barın kalça üzerine yerleştirilmesiyle yapılan bu hareket, gluteus maximus kasını maksimum kasılmada çalıştırır. Kalça yükseltme hareketi olarak bilinen en etkili glute egzersizleri arasındadır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100002"),
                column: "Description",
                value: "Arka ayak bench üzerinde desteklenirken ön ayakla yapılan bu tek bacak squat varyasyonu, gluteus ve quadriceps kaslarını dengeli biçimde çalıştırır. Kas dengesizliklerini gidermeye yardımcı olur.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100003"),
                column: "Description",
                value: "Bacak presinde ayakların platformun üst kısmına yerleştirilmesiyle gluteus kaslarının aktivasyonu artırılır. Kalça açısı değiştirilerek hamstring ve gluteus'un farklı lifleri vurgulanır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000110001"),
                column: "Description",
                value: "Ayak uçlarına yükselerek yapılan bu egzersiz, gastrocnemius ve soleus kaslarını çalıştırır. Topuk aşağıda serbest bırakılarak tam aralıkta hareket edilmesi kas gelişimi için kritik öneme sahiptir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000110002"),
                column: "Description",
                value: "Makine üzerinde omuzlardan yüklenerek yapılan bu baldır egzersizi, gastrocnemius kasını izole eder. Kontrollü negatif fazı sayesinde eksantrik stimülasyon yüksektir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120001"),
                column: "Description",
                value: "Kablo makinesi önünde diz üstü pozisyonda yapılan bu karın egzersizi, rectus abdominis'i yoğun biçimde çalıştırır. Ayarlanabilir ağırlık sayesinde kas gelişimine uygun direnç kolayca belirlenir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120002"),
                column: "Description",
                value: "Karın kası makinesi üzerinde yapılan bu izolasyon hareketi, rectus abdominis kasını destekli ve kontrollü biçimde çalıştırır. Başlangıç seviyeleri için güvenli ve etkili bir karın egzersizidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120003"),
                column: "Description",
                value: "Karın tekeri ile yapılan bu ileri düzey egzersiz, rectus abdominis ve transversus abdominis kaslarını yüksek gerilimde çalıştırır. Gövde stabilitesi ve karın kası kuvveti için son derece etkilidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000130001"),
                column: "Description",
                value: "Kablo makinesiyle diyagonal bir hareket yolunda gerçekleştirilen bu egzersiz, iç ve dış oblik kasları döndürme hareketi sırasında aktive eder. Rotasyon kuvveti ve gövde stabilitesi için kritik bir harekettir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000130002"),
                column: "Description",
                value: "Landmine aparatına takılı barın iki elle tutularak yanlara döndürülmesiyle yapılan bu hareket, oblik kasları ve rotator core kaslarını işlevsel biçimde çalıştırır. Sporsal performans gelişimi için değerlidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000140001"),
                column: "Description",
                value: "Yerden bar kaldırma olarak bilinen bu bileşik egzersiz, alt sırt, gluteus, hamstring ve quadriceps kaslarını eş zamanlı çalıştırır. Vücuttaki en fazla kas kütlesini aktive eden hareketlerden biri olup anabolik etkisi yüksektir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000140002"),
                column: "Description",
                value: "Hiperextension tezgahında gövde aşağı sarkıtılıp kaldırılarak yapılan bu hareket, erector spinae ve alt sırt kaslarını güçlendirir. Bel sağlığını desteklemek ve deadlift performansını artırmak için etkilidir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000150001"),
                column: "Description",
                value: "Bar tutularak omuzların yukarı kaldırılıp bırakılması şeklinde yapılan bu egzersiz, üst trapez kasını izole eder. Ağır ağırlıklarla yapılabilen bu hareket, omuz ve boyun bölgesinin kalın görünümünü destekler.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000150002"),
                column: "Description",
                value: "Her iki elde dumbbell tutularak yapılan bu omuz kaldırma hareketi, trapez kasını dengeli biçimde çalıştırır. Dumbbell'ların vücudun yanında serbestçe asılı kalması, hareket açısını artırır.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000160001"),
                column: "Description",
                value: "Geniş tutuşla kablo üzerinden yapılan bu çekiş, latissimus dorsi kasını en geniş açıyla aktive eder. Bar göğüse indirilirken dirsekler yanlara doğru açılır ve sırt kaslarının tam kasılması hissedilir.");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000160002"),
                column: "Description",
                value: "Ters tutuşla yapılan bu lat pulldown çeşidi, latissimus dorsi'nin yanı sıra biceps kasını da yoğun biçimde aktive eder. Alt lat liflerini hedef alması bakımından geniş tutuşa tamamlayıcı niteliktedir.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010004"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010005"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000010006"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020004"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020005"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000020006"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030004"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000030005"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000040004"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000050004"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000060001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000060002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070004"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070005"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070006"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000070007"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000080001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000080002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000090001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000090002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000100003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000110001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000110002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000120003"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000130001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000130002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000140001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000140002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000150001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000150002"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000160001"),
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000160002"),
                column: "Description",
                value: null);
        }
    }
}
