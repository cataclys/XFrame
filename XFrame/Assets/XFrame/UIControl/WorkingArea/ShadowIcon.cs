using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowIcon : MonoBehaviour
{
    public bool IsFixed = false;
    public Transform Background;
    protected void Start()
    {
        Background = transform.parent;
    }
    private void FixedUpdate()
    {
        if (IsFixed)
        {
            float size = 1f / Background.localScale.x;
            transform.localScale = new Vector3(size, size, size);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
}
