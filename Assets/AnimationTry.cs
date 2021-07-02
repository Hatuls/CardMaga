using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTry : MonoBehaviour
{
    [SerializeField] Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _anim.SetInteger("AnimNum", 13);
            _anim.CrossFade("HeadButt", 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _anim.SetInteger("AnimNum", 9);
            _anim.CrossFade("Jab", 0.1f);

        }
    }
}
