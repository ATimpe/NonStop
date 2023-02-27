using System.Collections;
using UnityEngine.Events;
using UnityEngine;

// Attach to objects that are triggered by being hit by a bullet
public class BulletTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent OnHit;

    public void Trigger()
    {
        OnHit.Invoke();
    }
}
