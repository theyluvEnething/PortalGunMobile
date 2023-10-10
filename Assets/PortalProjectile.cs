using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PortalProjectile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject PortalPrefab;
    public bool isOrange = true;
    Quaternion targetRotation, playerRotation;

    void Start()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Level/PortalMaterial");
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, layerMask);
        targetRotation = hit.transform.rotation;
        Debug.Log(targetRotation);

        Vector2 initPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 hitPos = new Vector2(hit.point.x, hit.point.z);
        Debug.Log("Init: " + initPos);
        Debug.Log("Hit: " + hitPos);

        float angle = Vector2.SignedAngle(initPos, hitPos);
        Debug.Log(Mathf.Round(angle* Mathf.PI/180));

        targetRotation.x = 0f;
        //targetRotation.y = (targetRotation.y + angle);
        targetRotation.z = 0f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != (LayerMask.NameToLayer("Level/PortalMaterial")))
        {
            Destroy(gameObject);
            return;
        }

        Portal portal = Instantiate(PortalPrefab, transform.position, targetRotation).GetComponent<Portal>();
        portal.isOrange = isOrange;

        Destroy(gameObject);
    }
}
