using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerRpcMessage : NetworkBehaviour
{
    public static ServerRpcMessage Instance;
    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.M))
        {
            if (!IsOwner)
            {
                return;
            }else
            {
            MessageServerRpc("hello Server...");
            }
            
        }
    }

    [ServerRpc]
    private void MessageServerRpc(string sometext)
    {
        if(!IsOwner) { return; }
        Debug.Log($"This Message is from client to --> server: {sometext}");

        MessageClientRpc("hello Client...");
        
    }

    [ClientRpc]
    private void MessageClientRpc(string sometext)
    {
        if(IsOwner) { return; }
        Debug.Log($"This Message is from server to --> client: {sometext}");
    }

    [ServerRpc]
    public void MessageHitOtherPlayerServerRpc(string msg)
    {
        if (!IsOwner) { return; }
        Debug.Log($"{msg}");
        MessageHitOtherPlayerClientRpc("You Hit Other Player");
        
    }

    [ClientRpc]
    public void MessageHitOtherPlayerClientRpc(string msg)
    {
        if (!IsOwner) return;
    }


    [ServerRpc]
    public void CalculateHitResultServerRpc()
    {
        if (!IsOwner) return;
        float hitProbalility = 0.7f;
        float rgn = Random.value;
        if (rgn <= hitProbalility)
        {
            Debug.Log("RGN Status: Hit Success");
            SendHitResultClientRpc("RGN Resullt: Suscess ", rgn);
        }
        else
        {
            Debug.Log("RGN Status: Hit Fail");
            SendHitResultClientRpc("RGN Resullt: Fail ", rgn);
        }
    }

    [ClientRpc]
    public void SendHitResultClientRpc(string msg, float rgn)
    {
        if (IsOwner) return;
        Debug.Log(msg + " " + rgn);
    }
}
