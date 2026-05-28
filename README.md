

Tugas Besar Strategi Algoritma - Kelompok stimAAA

---

## 📌 Algoritma Greedy yang Dipakai

Bot yang kami buat menggunakan **algoritma greedy**, yaitu strategi di mana bot selalu mengambil keputusan terbaik berdasarkan kondisi saat itu tanpa memikirkan masa depan.

Kami membuat **4 bot** dengan strategi greedy yang berbeda:

| Bot | Strategi | Heuristic |
|-----|----------|-----------|
| **Sally** | Hybrid 5 mode | Adaptif sesuai kondisi (darurat, banyak musuh, sniper, ramming, normal) |
| **Rusher** | Ramming | Kejar musuh terdekat, tabrak & tembak power besar |
| **Sour** | Sniper | Jaga jarak, tembak akurat dari jauh |
| **Kicau** | Survival | Prioritaskan bertahan hidup, kabur saat bahaya |

Bot utama kami adalah **Sally** karena paling seimbang. Dia punya 5 mode strategi yang otomatis berubah tergantung situasi:
- **Mode Darurat** (energi < 35) → kabur
- **Mode Banyak Musuh** (lawan ≥ 2) → menjauh dari keramaian
- **Mode Sniper** (jarak > 220) → jaga jarak, tembak akurat
- **Mode Ramming** (jarak < 65 & energi lebih) → tabrak & tembak besar
- **Mode Normal** (sisa kondisi) → zigzag standar

---

## Yang Dibutuhkan

- .NET SDK (10.0 atau 6.0)
- Java (buat jalanin servernya)
- Robocode Tank Royale Engine (dari starter pack atau download sendiri)

---

## Cara Jalanin

1. Buka Server Dulu
```bash
cd tank-royale-0.30.0
java -jar robocode-tankroyale-gui-0.30.0.jar

2. Build Bot
bash
cd src/main-bot/Sally
dotnet build

3. Run Bot
bash
dotnet run

4. Di GUI
Config → Bot Root Directories → tambahin folder Sally

Klik Add, tunggu sampe muncul di Joined Bots

Pilih lawan trus Start Battle

Isi Folder
tubes1_stimAAA/
├── src/
│   ├── Sally/
│   │   ├── Sally.cs
│   │   ├── Sally.csproj
│   │   └── Sally.json
│   ├── Rusher/
│   │   ├── Rusher.cs
│   │   ├── Rusher.csproj
│   │   └── Rusher.json
│   ├── Sour/
│   │   ├── Sour.cs
│   │   ├── Sour.csproj
│   │   └── Sour.json
│   └── Kicau/
│       ├── Kicau.cs
│       ├── Kicau.csproj
│       └── Kicau.json
├── doc/
│   └── laporan.pdf
└── README.md

Author
Suci Marwah Anissa	124140161
Reza Lesmana	124140115
M Faza Prasetyo	124140204
Kelas: [RA]

Mata Kuliah: Strategi Algoritma

Tahun: 2026

⚠️ Catatan 
Server harus jalan sebelum bot di-run. Kalo ga, bot ga bakal konek.

Kalo error "Invalid secret", coba set environment variable dulu:

bash
$env:SERVER_SECRET = ""
dotnet run
Jangan lupa build ulang kalo ada perubahan kode.

Udah itu aja. Selamat bertempur! 🎯
