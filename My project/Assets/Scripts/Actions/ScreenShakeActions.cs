using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
  
    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        EggBombProjectile.OnAnyGrenadeExploded += EggBombProjectile_OnAnyGrenadeExploded;
    }

    private void EggBombProjectile_OnAnyGrenadeExploded(object sender, System.EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(0.7f);
    }



}
