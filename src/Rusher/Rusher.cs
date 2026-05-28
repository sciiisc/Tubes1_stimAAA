using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Rusher : Bot
{
    double tx, ty;
    bool adaTarget;
    Random r = new Random();

    static void Main(string[] args) => new Rusher().Start();
    public Rusher() : base(BotInfo.FromFile("Rusher.json")) { }

    public override void Run()
    {
        BodyColor = Color.DarkRed;
        TurretColor = Color.Red;
        RadarColor = Color.Orange;

        while (IsRunning)
        {
            if (!adaTarget) SetTurnRadarLeft(45);
            else
            {
                double jrk = DistanceTo(tx, ty);
                double arah = BearingTo(tx, ty);
                
                // Radar & gun lock
                SetTurnRadarLeft(RadarBearingTo(tx, ty));
                SetTurnGunLeft(GunBearingTo(tx, ty));
                
                // GREEDY: Kejar terus, ga peduli zigzag
                SetTurnLeft(arah);
                SetForward(jrk < 50 ? 100 : 100);
                
                // Tembak kalo deket
                if (jrk < 150 && Math.Abs(GunBearingTo(tx, ty)) < 10)
                    SetFire(jrk < 80 ? 3 : 2);
            }
            Go();
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        tx = e.X; ty = e.Y;
        adaTarget = true;
        if (DistanceTo(e.X, e.Y) < 60) SetFire(3);
    }
    public override void OnHitBot(HitBotEvent e) { SetFire(3); SetForward(100); }
    public override void OnHitByBullet(HitByBulletEvent e) { SetTurnLeft(180); SetForward(100); }
    public override void OnHitWall(HitWallEvent e) { SetTurnLeft(90); SetForward(100); }
    public override void OnDeath(DeathEvent e) => adaTarget = false;
}