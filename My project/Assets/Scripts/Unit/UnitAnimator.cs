using System;
using UnityEngine;

/*
 * Script that controlls all the animations for the ducks
 * Execute these animation by subscribing to events
*/

public class UnitAnimator : MonoBehaviour
{

    private const string IS_WALKING = "IsMoving";
    private const string SHOOT = "Shoot";
    private const string LAY_EGG = "Lay";
    private const string DAMAGE_POPUP = "PopUp";


    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private UnitWorldUI unitWorldUI;

    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
        if(TryGetComponent<LayAction>(out LayAction layAction))
        {
            layAction.OnLayEgg += LayAction_OnLayEgg;
        }
        


    }

    private void Start()
    {

        unitWorldUI= GetComponentInChildren<UnitWorldUI>();

        unitWorldUI.OnDamagePopUp += UnitWorldUI_OnDamagePopUp;
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool(IS_WALKING, true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool(IS_WALKING, false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger(SHOOT);

        Transform bulletProjectileTransform = 
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        // the bullet moves at the exact horizontal height 
        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        // Pass through the world position of the target unit as an argument 
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void LayAction_OnLayEgg(object sender, EventArgs e)
    {
        animator.SetTrigger(LAY_EGG);
    }

    private void UnitWorldUI_OnDamagePopUp(object sender, EventArgs e)
    {
        Debug.Log("DamagePopUp Activate");
        animator.SetTrigger(DAMAGE_POPUP);
    }

}
