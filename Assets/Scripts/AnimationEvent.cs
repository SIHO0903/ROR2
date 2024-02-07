using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent OnHit = new UnityEvent();
    public UnityEvent OnFootR = new UnityEvent();
    public UnityEvent OnFootL = new UnityEvent();
    public UnityEvent OnLand = new UnityEvent();
    public UnityEvent OnShoot = new UnityEvent();

    public void Hit()
    { OnHit.Invoke(); }

    public void FootR()
    { OnFootR.Invoke(); }

    public void FootL()
    { OnFootL.Invoke(); }

    public void Land()
    { OnLand.Invoke(); }

    public void Shoot()
    { OnShoot.Invoke(); }
}
