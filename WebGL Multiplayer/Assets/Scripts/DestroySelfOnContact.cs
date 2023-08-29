using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DestroySelfOnContact : NetworkBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D col = collision;
        if(col != null)
        {
             Destroy(gameObject);
        }
        
    }

    
}
