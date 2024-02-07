using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using WarriorAnims;

public class AnimationParentMove : MonoBehaviour
{
    Animator anim;
    PlayerAttack playerAttack;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAttack = GetComponentInParent<PlayerAttack>();
    }
    /// <summary>
    /// If there is translation in the Root Motion Node, as defined in each animation file under
    /// Motion > Root Motion Node, then use that motion to move the Warrior if the controller is
    /// set to useRootMotion.
    /// </summary>
    void OnAnimatorMove()
    {
        if (playerAttack.useRootMotion)
        {
            transform.parent.rotation = anim.rootRotation;
            transform.parent.position += anim.deltaPosition;

        }

    }
}
