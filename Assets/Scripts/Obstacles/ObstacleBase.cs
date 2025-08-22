using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ObstacleBase, IObstacle 사용이 겹친다.
// 공통 기능을 준비한 것은 좋지만 사용 자체과 일관적이지는 않다.
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
