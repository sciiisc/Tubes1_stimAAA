using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Kicau : Bot
{
    double tx, ty;
    bool adaTarget;
    int gerak;
    Random r = new Random();

    static void Main(string[] args) => new Kicau().Start();
    public Kicau() : base(BotInfo.FromFile("Kicau.json")) { }

    public override void Run()
    {
        BodyColor = Color.DarkBlue;
        TurretColor = Color.Blue;
        RadarColor = Color.Cyan;

        while (IsRunning)
        {
            if (!adaTarget) SetTurnRadarLeft(45);
            else
            {
                double jrk = DistanceTo(tx, ty);
                double arah = BearingTo(tx, ty);
                
                SetTurnRadarLeft(RadarBearingTo(tx, ty));
                SetTurnGunLeft(GunBearingTo(tx, ty));
                
                // GREEDY: Prioritas SELAMAT
                if (Energy < 40)
                {
                    // LARI!
                    SetTurnLeft(arah + 180);
                    SetForward(100);
                }
                else if (jrk < 120)
                {
                    // Musuh deket, mundur
                    SetTurnLeft(arah + 180);
                    SetBack(80);
                }
                else
                {
                    // Zigzag random biar susah kena
                    SetTurnLeft(arah + (r.Next(-60, 60)));
                    SetForward(r.Next(50, 100));
                }
                
                // Tembak dikit aja (hemat energi)
                if (Math.Abs(GunBearingTo(tx, ty)) < 10 && Energy > 50)
                    SetFire(1);
            }
            gerak++;
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        tx = e.X; ty = e.Y;
        adaTarget = true;
    }
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Kena tembak -> langsung kabur
        SetTurnLeft(r.Next(60, 120));
        SetForward(100);
    }
    public override void OnHitWall(HitWallEvent e) { SetTurnLeft(60); SetForward(80); }
    public override void OnHitBot(HitBotEvent e) { SetBack(80); SetTurnLeft(90); }
    public override void OnDeath(DeathEvent e) => adaTarget = false;
}