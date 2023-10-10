using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{


    [Header("OTHER")]
    private static Portal OrangePortal;
    private static Portal BluePortal;

    [SerializeField] public bool isOrange;

    void Start()
    {
        if (isOrange)
        {
            if (OrangePortal != null)
            {
                Debug.Log(OrangePortal);
                DestroyImmediate(OrangePortal.gameObject);
            }
            OrangePortal = this;
        }
        else
        {
            if (BluePortal != null)
            {
                Debug.Log(BluePortal);
                DestroyImmediate(BluePortal.gameObject);
            }
            BluePortal = this;
        }
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
