using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    //public NetworkVariable<int> TotalCoins = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int>dummyCoins = new NetworkVariable<int>(5,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private int temporaryCoins;

    public override void OnNetworkSpawn()
    {
        UIController.instance.modelCoinWallet = this;
        UIController.instance.SetCoinText(GetTotalCoins());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.TryGetComponent<Coin>(out Coin coin)) return;
        
        temporaryCoins = coin.Collect();

        //TotalCoins.Value += temporaryCoins;
        AddedDummyCoinsServerRpc(temporaryCoins);
        //dummyCoins.Value = temporaryCoins;
        UIController.instance.SetUITotalCoinClientRpc(dummyCoins.Value);

        //if (!IsServer) // client
        //{
        //    //TotalCoins.Value += temporaryCoins;
        //    AddedDummyCoinsServerRpc(temporaryCoins);
        //    //dummyCoins.Value = temporaryCoins;
        //    UIController.instance.SetUITotalCoin(dummyCoins.Value);
        //}

        //else if (IsServer) // server
        //{
        //    //TotalCoins.Value += temporaryCoins;
        //    AddedDummyCoinsServerRpc(temporaryCoins);
        //    //dummyCoins.Value = temporaryCoins;
        //    UIController.instance.SetUITotalCoin(dummyCoins.Value);
        //}

        //if (IsOwner)
        //{
        //    dummyCoins.Value += temporaryCoins;
        //    UIController.instance.SetUITotalCoin(dummyCoins.Value);
        //}
      

    }
    //[ClientRpc]
    //public void SetWalletClientRpc(int coin)
    //{
    //    if(TotalCoins.Value  <= 0) return;
    //    TotalCoins.Value -= coin;
    //}

    [ServerRpc(RequireOwnership = false)]
    public void SetDummyCoinsServerRpc(int coin)
    {
        if (dummyCoins.Value <= 0) return;
        dummyCoins.Value -= coin;
    }
    [ServerRpc(RequireOwnership =false)]
    public void AddedDummyCoinsServerRpc(int coin)
    {
        dummyCoins.Value += coin;
    }

    public int GetTotalCoins()
    {
        return dummyCoins.Value;
    }
}
