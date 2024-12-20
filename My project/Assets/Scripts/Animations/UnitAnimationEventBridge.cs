using UnityEngine;

/*
 * As the logic and visual code are seperated on my duck prefabs.
 * This script works as a bridge.
 * Sine animator is on a child object this script acts as a bridge between unity's animation events and my scripts  
 * Which calls the OnLayAnimationComplete function.
 * 
*/ 

public class UnitAnimationEventBridge : MonoBehaviour
{
    [SerializeField] LayAction layAction;


    /*
     * This function informs layAction
     * via LayAction.OnLayEggAnimationComplete 
    */
    public void OnLayAnimationComplete()
    {
        if(layAction != null)
        {
            layAction.OnLayEggAnimationComplete();
        }
    }
}
