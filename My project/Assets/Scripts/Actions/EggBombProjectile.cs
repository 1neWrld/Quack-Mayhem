using System;
using System.Data;
using UnityEngine;

public class EggBombProjectile : MonoBehaviour
{


    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Unit targetUnit;
    [SerializeField] private Transform eggBombExplosionVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    
    
    //[SerializeField] private BombAction bombAction;


    private Vector3 targetPosition;
    private int damageAmount;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;


    private void Update()
    {
        Vector3 moveDir = (targetPosition- positionXZ).normalized ;

        float moveSpeed = 25f;
        positionXZ += moveDir* moveSpeed* Time.deltaTime;



        /*
         * When projectile is spawned we check if the distance is less than a certain amount
         * Destroy it
         * Physics query to afflict damage only on target Units
        */ 

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance/ totalDistance;

        
        float maxHeight = totalDistance/ 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized)* maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float targetDistance = .2f;
        if(Vector3.Distance(positionXZ, targetPosition) < targetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    //Set the damageAmount to a random range between 10 and 40
                    damageAmount = UnityEngine.Random.Range(20, 31);
                    targetUnit.Damage(damageAmount);

                    
                    UnitWorldUI targetUnitWorldUI = targetUnit.GetComponentInChildren<UnitWorldUI>();

                    if (targetUnitWorldUI != null)
                    {
                        targetUnitWorldUI.ShowDamage(damageAmount);
                    }
                    
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;
            Instantiate(eggBombExplosionVFXPrefab, targetPosition + Vector3.up* 1f, Quaternion.identity);  

            Destroy(gameObject);
            onGrenadeBehaviourComplete();

        }

    }


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);   
    }


}
