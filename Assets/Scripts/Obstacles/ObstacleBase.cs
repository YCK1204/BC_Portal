using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour, IObstacle
{
    public bool isActive {  get; private set; }

    public virtual void Activate()
    {
        isActive = true;
    }

    public virtual void Deactivate()
    {
        isActive = false;
    }
}
