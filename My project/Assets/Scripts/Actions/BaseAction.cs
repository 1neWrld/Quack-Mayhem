using System;
using UnityEngine;

// Abstract forbids us to create an instance of the BaseAction class
public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;

    protected Action OnActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    
}
