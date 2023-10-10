using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform MuzzlePoint;
    [SerializeField] private GameObject BluePortalProjectilePrefab;
    [SerializeField] private GameObject OrangePortalProjectilePrefab;

    [Header("ATTRIBUTES")]
    [SerializeField, Range(0f, 20f)] private float ProjVelocity = 20f;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject portalProjectile = Instantiate(BluePortalProjectilePrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            Rigidbody portalRB = portalProjectile.GetComponent<Rigidbody>();
            if (portalRB)
            {
                portalRB.AddForce(MuzzlePoint.forward * ProjVelocity, ForceMode.Impulse);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameObject portalProjectile = Instantiate(OrangePortalProjectilePrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            Rigidbody portalRB = portalProjectile.GetComponent<Rigidbody>();
            if (portalRB)
            {
                portalRB.AddForce(MuzzlePoint.forward * ProjVelocity, ForceMode.Impulse);
            }
        }
    }
}
