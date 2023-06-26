using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator mAnimator;
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mAnimator != null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                mAnimator.SetTrigger("StartRotation");
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                mAnimator.SetTrigger("StopRotation");
            }
        }
    }
}
