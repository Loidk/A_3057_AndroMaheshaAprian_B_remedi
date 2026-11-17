using System;
using System.Collections.Generic;

namespace MafiaGalon
{
    class Agen
    {
        public string IdAgen { get; set; }
        public string NamaAgen { get; set; }
        public int StokAgen { get; set; }

        public Agen(string idAgen, string namaAgen, int stokAgen)
        {
            IdAgen = idAgen;
            NamaAgen = namaAgen;
            StokAgen = stokAgen;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"ID: {IdAgen} | Nama: {NamaAgen} | Stok: {StokAgen} galon");
        }
    }

    abstract class ProdukGalon
    {
        private string idProduk;
        private string namaProduk;
        private int stokGudang;

        protected ProdukGalon(string idProduk, string namaProduk, int stokGudang)
        {
            this.idProduk = idProduk;
            this.namaProduk = namaProduk;
            StokGudang = stokGudang;
        }

        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }

        public int StokGudang
        {
            get { return stokGudang; }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Stok gudang tidak boleh negatif!");
                    return;
                }
                if (value < stokGudang)
                {
                    Console.WriteLine("Stok hanya boleh dikurangi lewat method KurangiStok()!");
                    return;
                }
                stokGudang = value;
            }
        }

        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah pengurangan harus lebih dari 0!");
                return false;
            }
            if (jumlah > stokGudang)
            {
                Console.WriteLine("Pengurangan melebihi stok gudang, stok tidak boleh negatif!");
                return false;
            }
            stokGudang -= jumlah;
            return true;
        }

        public void TambahStok(int jumlah)
        {
            if (jumlah > 0) stokGudang += jumlah;
        }

        public abstract int HitungBiaya(int jumlah);

        public virtual void TampilInfo()
        {
            Console.WriteLine($"ID: {IdProduk} | Nama: {NamaProduk} | Stok gudang: {StokGudang}");
        }
    }

    class GalonAkua : ProdukGalon
    {
        private const int HargaPerGalon = 20000;

        public GalonAkua(string idProduk, string namaProduk, int stokGudang)
            : base(idProduk, namaProduk, stokGudang) { }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerGalon;
        }

        public override void TampilInfo()
        {
            base.TampilInfo();
            Console.WriteLine($"            Jenis: Galon Akua (merek resmi) | Harga: Rp{HargaPerGalon:N0}/galon");
        }
    }

    class GalonBerkeran : ProdukGalon
    {
        private const int HargaPerGalon = 12000;

        public string JenisKeran { get; set; }

        public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
            : base(idProduk, namaProduk, stokGudang)
        {
            JenisKeran = jenisKeran;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerGalon;
        }

        public override void TampilInfo()
        {
            base.TampilInfo();
            Console.WriteLine($"            Jenis: Galon Berkeran | Keran: {JenisKeran} | Harga: Rp{HargaPerGalon:N0}/galon");
        }
    }

    interface DapatDidistribusikan
    {
        bool KirimPaket();
    }

    class DistribusiGalon : DapatDidistribusikan
    {
        public Agen AgenTujuan { get; private set; }
        public ProdukGalon Produk { get; private set; }
        public int Jumlah { get; private set; }

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
        {
            AgenTujuan = agenTujuan;
            Produk = produk;
            Jumlah = jumlah;
        }

        public bool KirimPaket()
        {
            if (Jumlah <= 0)
            {
                Console.WriteLine($"Pengiriman ke {AgenTujuan.NamaAgen} ditolak: jumlah galon harus lebih dari 0.");
                return false;
            }

            if (Jumlah > Produk.StokGudang)
            {
                Console.WriteLine($"Pengiriman ke {AgenTujuan.NamaAgen} ditolak: stok gudang {Produk.NamaProduk} " +
                                  $"hanya {Produk.StokGudang}, diminta {Jumlah}.");
                return false;
            }

            Produk.KurangiStok(Jumlah);
            AgenTujuan.StokAgen += Jumlah;

            Console.WriteLine($"Paket berhasil dikirimkan ke agen {AgenTujuan.NamaAgen}: " +
                              $"{Jumlah} {Produk.NamaProduk} (biaya Rp{Produk.HitungBiaya(Jumlah):N0}). " +
                              "Sindikat tetap bungkam...");
            return true;
        }
    }

    class SindikatGalon
    {
        private List<Agen> daftarAgen = new List<Agen>();
        private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

        public void TambahAgen(Agen agen)
        {
            daftarAgen.Add(agen);
            Console.WriteLine($"Agen {agen.NamaAgen} resmi bergabung ke sindikat.");
        }

        public void TambahProduk(ProdukGalon produk)
        {
            daftarProduk.Add(produk);
            Console.WriteLine($"{produk.NamaProduk} masuk gudang pusat ({produk.StokGudang} galon).");
        }

        public void TampilkanSindikat()
        {
            Console.WriteLine("========== ASET SINDIKAT MAFIA GALON ==========");
            Console.WriteLine("-- Daftar Agen --");
            foreach (Agen a in daftarAgen) a.TampilInfo();
            Console.WriteLine("-- Daftar Produk Galon --");
            foreach (ProdukGalon p in daftarProduk) p.TampilInfo();
            Console.WriteLine("===============================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            // agen

            Agen agen1 = new Agen("AG-001", "Budi", 10);
            Agen agen2 = new Agen("AG-002", "Siti", 5);
            agen1.TampilInfo();
            agen2.TampilInfo();

            Console.WriteLine();

            //galon

            ProdukGalon akua = new GalonAkua("PG-001", "Galon Akua", 100);
            ProdukGalon berkeran = new GalonBerkeran("PG-002", "Galon Berkeran", 50, "Keran Uranium");

            Console.WriteLine();



            akua.StokGudang = -5;
            akua.StokGudang = 10;
            Console.WriteLine($"Stok Akua: {akua.StokGudang}");
            akua.KurangiStok(10);
            Console.WriteLine($"Stok Akua setelah KurangiStok(10): {akua.StokGudang}");
            akua.KurangiStok(500);
            akua.TambahStok(10);

            Console.WriteLine();

            //polimorfisme

            ProdukGalon[] semuaProduk = { akua, berkeran };
            foreach (ProdukGalon p in semuaProduk)
            {
                Console.WriteLine($"{p.NamaProduk}: 5 galon = Rp{p.HitungBiaya(5):N0}");
            }

            Console.WriteLine();
                
            //interface

            DapatDidistribusikan kirim1 = new DistribusiGalon(agen1, akua, 20);
            DapatDidistribusikan kirim2 = new DistribusiGalon(agen2, berkeran, 0);
            DapatDidistribusikan kirim3 = new DistribusiGalon(agen2, berkeran, 999);
            DapatDidistribusikan kirim4 = new DistribusiGalon(agen2, berkeran, 15);
            kirim1.KirimPaket();
            kirim2.KirimPaket();
            kirim3.KirimPaket();
            kirim4.KirimPaket();

            Console.WriteLine();
            
            //opsional

            SindikatGalon sindikat = new SindikatGalon();
            sindikat.TambahAgen(agen1);
            sindikat.TambahAgen(agen2);
            sindikat.TambahProduk(akua);
            sindikat.TambahProduk(berkeran);
            Console.WriteLine();
            sindikat.TampilkanSindikat();

        }
    }
}