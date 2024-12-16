using UnityEngine;

public class LayAction : BaseAction
{
    //Defines a delegate of type void
    // The signature of the delegate must match the one(function) you pass through
    public delegate void LayEggComplete();

    private Animator animator;
    private LayEggComplete onLayEggComplete;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public void LayEggAnimation(LayEggComplete onLayayEggComplete)
    {
        //this.onLayEggComplete = onLayEggComplete;
        animator.SetTrigger("Lay");
        isActive = false;

    }

    public void OnLayEggAnimationComplete()
    {
        isActive = false;

        onLayEggComplete?.Invoke();
    }
}
