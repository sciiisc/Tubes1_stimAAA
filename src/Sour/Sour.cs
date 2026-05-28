using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Sour : Bot
{
    double tx, ty;
    bool adaTarget;
    int gerak;

    static void Main(string[] args) => new Sour().Start();
    public Sour() : base(BotInfo.FromFile("Sour.json")) { }

    public override void Run()
    {
        BodyColor = Color.DarkGreen;
        TurretColor = Color.Green;
        RadarColor = Color.LightGreen;

        while (IsRunning)
        {
            if (!adaTarget) SetTurnRadarLeft(45);
            else
            {
                double jrk = DistanceTo(tx, ty);
                double arah = BearingTo(tx, ty);
                
                SetTurnRadarLeft(RadarBearingTo(tx, ty));
                double arahMeriam = GunBearingTo(tx, ty);
                SetTurnGunLeft(arahMeriam);
                
                // GREEDY: Jaga jarak 200-400
                if (jrk < 200)
                {
                    // Mundur kalo terlalu deket
                    SetTurnLeft(arah + 180);
                    SetBack(80);
                }
                else if (jrk > 400)
                {
                    // Maju kalo terlalu jauh
                    SetTurnLeft(arah);
                    SetForward(60);
                }
                else
                {
                    // Zigzag pelan di jarang aman
                    SetTurnLeft(arah + (gerak % 30 < 15 ? 20 : -20));
                    SetForward(50);
                }
                
                // Tembak kalo meriam udah ngarah
                if (Math.Abs(arahMeriam) < 5)
                {
                    double p = jrk < 250 ? 2.5 : jrk < 400 ? 2 : 1.5;
                    if (Energy > p + 1) SetFire(p);
                }
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
    public override void OnHitByBullet(HitByBulletEvent e) { SetTurnLeft(45); SetForward(80); }
    public override void OnHitWall(HitWallEvent e) { SetTurnLeft(45); SetBack(50); }
    public override void OnHitBot(HitBotEvent e) { SetBack(80); SetTurnLeft(90); }
    public override void OnDeath(DeathEvent e) => adaTarget = false;
}