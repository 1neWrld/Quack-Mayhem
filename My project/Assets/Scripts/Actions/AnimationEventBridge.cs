using UnityEngine;

public class AnimationEventBridge : MonoBehaviour
{
    [SerializeField] LayAction layAction;


    public void OnLayAnimationComplete()
    {
        if(layAction != null)
        {
            layAction.OnLayEggAnimationComplete();
        }
    }
}
