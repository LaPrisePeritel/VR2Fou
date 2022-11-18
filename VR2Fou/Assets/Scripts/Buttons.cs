using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buttons : MonoBehaviour
{
    private Animation animation;
    [HideInInspector] public UnityEvent Event;

    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    private void OnMouseDown()
    {
        Event.Invoke();
        animation.Play();
    }
}