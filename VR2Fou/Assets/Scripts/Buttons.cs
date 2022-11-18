using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buttons : MonoBehaviour
{
    private Animator animator;
    [HideInInspector] public UnityEvent Event;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        Event.Invoke();
        animator.SetTrigger("ButtonClicked");
    }
}