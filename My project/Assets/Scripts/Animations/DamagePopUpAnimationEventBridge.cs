using UnityEngine;

public class DamagePopUpAnimationEventBridge : MonoBehaviour
{
    [SerializeField] private UnitWorldUI unitWorldUI;

    public void OnDamagePopUpAnimationComplete()
    {
        unitWorldUI.DamagePopUpAnimationComplete();
    }

}
