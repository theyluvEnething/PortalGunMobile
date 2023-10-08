using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{


    [Header("OTHER")]
    private static Portal OrangePortal;
    private static Portal BluePortal;

    [HideInInspector] public bool isOrange;

    void Start()
    {
        if (isOrange)
            OrangePortal = this;
        else
            BluePortal = this;
    }

    void Update()
    {
        
    }
    public void SetBlue()
    {
        isOrange = false;
        BluePortal = this;
    }
}
