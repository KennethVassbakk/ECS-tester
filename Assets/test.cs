using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnBecameVisible()
    {
        rend.enabled = true;
    }


    private void OnBecameInvisible()
    {
        rend.enabled = false;
    }
}
