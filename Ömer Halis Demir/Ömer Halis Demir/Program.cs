using System;
using System.Threading;
using WMPLib;

class Stuff
{
    static Random rand = new Random();
    static void Main()
    {
        WMPLib.WindowsMediaPlayer wplayer2 = new WMPLib.WindowsMediaPlayer();
        wplayer2.URL = "b.mp3";
        wplayer2.controls.stop();

        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        wplayer.URL = "a.mp3";
        wplayer.controls.play();

        string Progresbar = "ÖMER HALİS DEMİR";
        var title = "";
        while (true)
        {
            for (int i = 0; i < Progresbar.Length; i++)
            {
                title += Progresbar[i];
                Console.Title = title;
                Thread.Sleep(90);
            }
        Console.ForegroundColor = ConsoleColor.Green;
        yavaşYazı("                                                                      15 Temmuz Gecesi\n");
        yavaşYazı("\n");
        yavaşYazı("15 Temmuz gecesi düşman, milletimizin ve cumhuriyetimizin onuruna ve namusuna el uzatıldı. Zorlu, yorucu ve uzun bir geceydi ama geride kaldı.\n");
        yavaşYazı("Bir yandan hasar almış cumhuriyetimizin yaralarını sarmaya çalışıyor.\n");
        yavaşYazı("Öbür yandan gece ansızın kentlerimizi basan darbeci eşkiyaları temizliyorduk.\n");
        yavaşYazı("Bunun bu kadar kolay olmadığını sen bilirsin çünkü pek çok yıldır kendilerini belli etmeden aramızda yattılar.\n");
        yavaşYazı("Aynı yemekleri yediler, aynı giysileri giydiler, görüntüde aynı yeminleri ettiler ve aynı bayrağa baktılar.\n");
        yavaşYazı("Ancak bir gün kendi kara bayraklarını çıkartıp şeytanın adıyla bize karşı harekete geçeceklerine, kendi insanlarına, meclisine, bayrağına ateş açabileceğine kimse inanmıyor\n");
        Console.ForegroundColor = ConsoleColor.Blue;
        yavaşYazı(".\n");
        yavaşYazı(".\n");
        yavaşYazı(".\n");
        yavaşYazı("\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        yavaşYazı("İnanmak istemiyordu.\n");
        yavaşYazı("O temmuz gecesinde nihayet karanlık tarikat harekete geçmişti.\n");
        yavaşYazı("Uzaktan kumanda edilen robot insanlardan bir farkları yoktu.\n");
        yavaşYazı("Beyinleri yıkanmış, çevrelerini saran, tankların üzerine çıkıp altına yatan, öldükçe gürleşen insanlarımızın haykırışları bile kör gözlerini açmaya yetmedi..\n");
        yavaşYazı("Yerden, gökten ateş edip duruyorlardı. Yetmedi. Silahlarını bırakıp küfür ve lanet söylemleriyle teslim oldular.\n");
        yavaşYazı("Ömer abi sana bu mektubu, gözün arkanda kalmasın diye yazıyorum. Eğer hala yaşıyor olsaydın, günün ilk ışıkları altında dalgalanan şanlı bayrağımızı sen de görürdün.\n");
        yavaşYazı("\n");
        Console.ForegroundColor = ConsoleColor.Cyan;
        yavaşYazı("Kadınlar, çocuklar, gençler, yaşlılar, engelliler, kızlar, oğlanlar bütün milletçe gece karanlığının içinden koca bir tünel açıp, görünmez atlarla meydana inmiştik.\n");
        yavaşYazı("Bir kez daha tarihe karşı çıkmış olan Türk milleti yalnızca mahallelerinde, kışlalarında, meydanlarında, işyerlerinde ya da okullarında gizlenmiş olan düşmana değil,\n");
        yavaşYazı("Onları bir gece baskınıyla hain bir şekilde üstümüze salanlara da muhteşem bir cevap vermişlerdi.\n");
        yavaşYazı("Bilmeni isterim ki cesur halkımız ölüm aralarında kol gezdiği anda bile bir adım bile geri çekilmediler, yılmadılar, vazgeçmediler, düşenlerin yerlerini hep başkaları doldurdu.\n");
        yavaşYazı("\n");
        Console.ForegroundColor = ConsoleColor.Magenta;
        yavaşYazı("Çanakkale'deki neferler gibi gösterişsiz ama onlar kadar inançlıydılar.\n");
        yavaşYazı("Senin adını anıyoruz durmadan Ömer abi. Diyoruz ki ilk kurşunu Ömer abi sıktı düşmana, ona ölmesi emredildiğinde bir an bile düşünmedi, bir an bile tereddüt etmedi.\n");
        yavaşYazı("Senden bahsederken gözleri yaşarıyor milletimin, gururu ve duyguları birbirine karışıyor.\n");
        yavaşYazı("20-25 yılı sınır boylarında, kışlalarda, dağ yollarında beraber geçirmişsiniz komutanınla; o gece de sana son bir emir vermiş.\n");
        yavaşYazı("Emri ölçüp tartmamış, cevabını uzatmamışsın sadece 'Baş üstüne komutanım, hakkım helal olsun, siz de helal edin' demişsin.\n");
        yavaşYazı("Çekip silahını, alnının ortasından vurmuşsun düşmanı. Bazen bir kurşun bir vatan kazandırır; bilesin ki attığın bir mermi yalnızca bir haini ortadan kaldırmadı, bütün Türk milletini de ayağa kaldırdı.\n");
        yavaşYazı("O gece meydanlarda dalgalanan her bayrak sana selam gönderiyordu.\n");
        yavaşYazı("Otuz kurşun yarası varmış vücudunda; ihanet çok korkmuş senden. Tıpkı meydanlarca, köprülerde ve yollarda toplanan silahsız halktan korktuğu gibi.\n");
        yavaşYazı("Bizim öbür dünyada da bir halkımız var biliyorusn; Çanakkale'den, Sarıkamış'dan, Plevne'den ve diğer bütün şehitlerimizden, çıktığın yolculukta yalnız değilsin. u\n");
        yavaşYazı("Türkiye Cumhuriyetini o gece korkusuz bir şekilde bütün onur, vatan sevgin ve cesaretinle savunduğun için.\n");
        yavaşYazı("\n");
        yavaşYazı("\n");
        Console.ForegroundColor = ConsoleColor.Red;
        yavaşYazı("Milletçe\n"); Console.ForegroundColor = ConsoleColor.White; yavaşYazı("TEŞEKKÜR EDERİZ.\n");
        yavaşYazı("Ne Mutlu"); Console.ForegroundColor = ConsoleColor.White; yavaşYazı(" TÜRKÜM Diyene.\n");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        yavaşYazı("                                                                                                                 Berke Okan UĞUR\n");
        wplayer.controls.stop();
        wplayer2.controls.play();
        Console.ReadLine();
        }
    }

    static void yanlışYazı(string s)
    {
       
        if (s == "\b \b")
        {
            Thread.Sleep(rand.Next(15, 30));
        }
        Console.Write(s);

        Console.Out.Flush();
        Thread.Sleep(rand.Next(30, 90));
         }

    static void yavaşYazı(string s)
    {

        foreach (char c in s)
        {
            if (rand.Next(35) == 0 && !Char.IsControl(c))
            {
                yanlışYazı(((char)rand.Next((int)'a', (int)'z' + 1)).ToString());

                yanlışYazı("\b \b");
            }

            yanlışYazı(c.ToString());

        }}

}

