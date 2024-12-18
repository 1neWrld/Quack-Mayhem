using System;
using UnityEngine;


public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private GameObject isBusyVisual;


    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        UnitActionSystem.Instance.OnActionBusy += UnitActionSystem_OnActionBusy;
        Hide();
    }


    public void UnitActionSystem_OnActionBusy(object sender, bool isBusy)
    {
        if (isBusy) 
        { 
            Show();
        }
        else
        {
            Hide();
        }

    }

    private void Show()
    {
        isBusyVisual.SetActive(true);
        animator.SetBool("IsBusy", true);
    }

    private void Hide()
    {
        isBusyVisual.SetActive(false);
        animator.SetBool("IsBusy", false);
    }

}
