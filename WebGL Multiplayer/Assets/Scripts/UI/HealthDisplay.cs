using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;

public class HealthDisplay : NetworkBehaviour
{
    [Header("Reference")]
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

    public override void OnNetworkSpawn()
    {
        //if (!IsServer) return;
        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
        HandleHealthChanged(0, health.CurrentHealth.Value);
    }
    public override void OnNetworkDespawn()
    {
        //if (!IsServer) return;
        health.CurrentHealth.OnValueChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(int oldHealth, int newHealth)
    {
        healthBarImage.fillAmount = (float)newHealth / health.maxHealth;
    }
}
