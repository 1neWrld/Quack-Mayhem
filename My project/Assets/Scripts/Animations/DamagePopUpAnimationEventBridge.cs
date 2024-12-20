using UnityEngine;

public class DamagePopUpAnimationEventBridge : MonoBehaviour
{
    [SerializeField] private UnitWorldUI unitWorldUI;

    private void OnDamagePopUpAnimationComplete()
    {
        unitWorldUI.DamagePopUpAnimationComplete();
    }

}
