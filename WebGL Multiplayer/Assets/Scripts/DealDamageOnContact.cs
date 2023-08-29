using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;
    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody == null) return;

        if(collision.attachedRigidbody.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact netObj))
        {
            if (ownerClientId == netObj.ownerClientId) return;
        }

        if(collision.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
            ServerRpcMessage.Instance.CalculateHitResultServerRpc();
        }
    }
}
