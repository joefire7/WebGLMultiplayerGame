using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;

    private Vector3 previousPosition;
    private void Update()
    {
        if(previousPosition != transform.position)
        {
            Show(true);
        }
        previousPosition = transform.position;
    }
    public override int Collect()
    {
        if (!IsServer) // if not server
        {
            Show(false); // hide coin
            return 0; // return 0
        }

        if (alreadyCollected) return 0;

        alreadyCollected = true; //is collected

        OnCollected?.Invoke(this); // fire a Events 

        return coinValue; // return the coin value
    }

    public void Reset()
    {
        alreadyCollected = false;
    }
}
