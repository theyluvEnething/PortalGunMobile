using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject PortalPrefab;
    public bool isOrange = true;

    private void OnCollisionEnter(Collision collision)
    {
        Portal portal = Instantiate(PortalPrefab, transform.position, collision.collider.transform.rotation).GetComponent<Portal>();
        portal.isOrange = isOrange;

        Destroy(gameObject);
    }
}
