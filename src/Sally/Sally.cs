using System;
using System.Collections.Generic;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Sally : Bot
{
    double tx, ty, te;
    bool adaTarget;
    int gerak, arahRadar = 1;
    List<string> musuh = new List<string>();
    Random acak = new Random();
    int kenaTembak;

    static void Main(string[] args) => new Sally().Start();
    public Sally() : base(BotInfo.FromFile("Sally.json")) { }

    public override void Run()
    {
        // ========== WARNA UNIVERSAL PUTIH (sesuai saran asisten) ==========
        ScanColor = Color.White;
        
        TurretColor = ScanColor;
        BodyColor = ScanColor;
        BulletColor = ScanColor;
        RadarColor = ScanColor;
        // ==================================================================

        while (IsRunning)
        {
            int jmlMusuh = musuh.Count;
            
            if (!adaTarget)
            {
                SetTurnRadarLeft(45 * arahRadar);
                SetForward(3);
                if (++gerak % 30 == 0) arahRadar *= -1;
            }
            else
            {
                double jrk = DistanceTo(tx, ty);
                double arahM = BearingTo(tx, ty);
                
                SetTurnRadarLeft(RadarBearingTo(tx, ty));
                double arahMeriam = GunBearingTo(tx, ty);
                SetTurnGunLeft(arahMeriam);
                
                // DARURAT (Energi < 35)
                if (Energy < 35)
                {
                    SetTurnLeft(arahM + 180 + acak.Next(-40, 40));
                    SetForward(100);
                    if (Math.Abs(arahMeriam) < 15 && jrk < 300) SetFire(1);
                }
                // BANYAK MUSUH (>= 2)
                else if (jmlMusuh >= 2)
                {
                    if (jrk < 150)
                    {
                        SetTurnLeft(arahM + 180);
                        SetForward(100);
                    }
                    else
                    {
                        SetTurnLeft(arahM + (gerak % 25 < 12 ? 50 : -50));
                        SetForward(70);
                    }
                    if (Math.Abs(arahMeriam) < 10 && Energy > 45) SetFire(1.5);
                }
                // SNIPER (jarak > 200 & energi > 50)
                else if (jrk > 220 && Energy > 50)
                {
                    SetTurnLeft(arahM + (gerak % 30 < 15 ? 25 : -25));
                    SetForward(50);
                    if (Math.Abs(arahMeriam) < 4)
                        SetFire(jrk < 300 ? 2.5 : 2);
                }
                // RAMMING (jarak < 65 & energi lebih)
                else if (jrk < 65 && Energy > te + 12)
                {
                    SetTurnLeft(arahM);
                    SetForward(100);
                    if (Math.Abs(arahMeriam) < 10) SetFire(3);
                }
                // NORMAL (zigzag)
                else
                {
                    if (gerak % 30 < 15) SetTurnLeft(arahM + 45);
                    else SetTurnLeft(arahM - 45);
                    SetForward(jrk < 180 ? 100 : 85);
                    
                    if (Math.Abs(arahMeriam) < 5)
                    {
                        double p = jrk < 80 ? 3 : jrk < 170 ? 2.5 : jrk < 300 ? 2 : 1.5;
                        if (Energy > p + 1) SetFire(p);
                    }
                }
                
                // hindari tembok
                if (X < 70 || X > 930 || Y < 70 || Y > 930) SetTurnLeft(50);
                
                // kabur kalo kebanyakan kena tembak
                if (kenaTembak > 2 && Energy < 55)
                {
                    SetTurnLeft(arahM + 180);
                    SetForward(100);
                }
            }
            
            if (gerak % 80 == 0) musuh.Clear();
            if (kenaTembak > 0 && Energy > 60) kenaTembak--;
            gerak++;
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        string id = $"{e.X}_{e.Y}";
        if (!musuh.Contains(id)) musuh.Add(id);
        
        tx = e.X; ty = e.Y; te = e.Energy;
        adaTarget = true;
        
        if (DistanceTo(e.X, e.Y) < 45 && Energy > te + 8)
        {
            SetTurnLeft(BearingTo(e.X, e.Y));
            SetForward(100);
            SetFire(3);
        }
    }

    public override void OnTick(TickEvent e)
    {
        if (adaTarget && (DistanceTo(tx, ty) > 850 || te <= 0)) adaTarget = false;
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        kenaTembak++;
        SetTurnLeft(Energy < 35 ? 80 : 50);
        SetForward(100);
    }

    public override void OnHitWall(HitWallEvent e) { SetTurnLeft(50); SetForward(80); }
    public override void OnHitBot(HitBotEvent e) { SetTurnLeft(BearingTo(e.X, e.Y)); SetForward(100); SetFire(3); }
    public override void OnDeath(DeathEvent e) => adaTarget = false;
}