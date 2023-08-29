using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; } = 100;

    // Sync Var
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    private bool isDead;

    public Action<Health> onDie;
    public override void OnNetworkSpawn()
    {
        //if(!IsServer) return;
        CurrentHealth.Value = maxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealt(int healVaue)
    {
        ModifyHealth(healVaue);
    }
    private void ModifyHealth(int value)
    {
        if(isDead) return;

        int newHealth = CurrentHealth.Value + value;
        // sync var -> Network Variabl
        CurrentHealth.Value = Math.Clamp(newHealth, 0, maxHealth);

        if(CurrentHealth.Value == 0)
        {
            onDie?.Invoke(this);
            isDead = true;
        }
    }
}
