using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UIController : NetworkBehaviour
{
    [Header("MVC Design Pattern - Model - View - Controller")]
    public UIViews uiViews;
    public bool isViewsActivate;
    [SerializeField] private int totalCoins;
    public CoinWallet modelCoinWallet;
    public FireProjectile fireProjectile;
    public static UIController instance;
    public bool autoHost;
    [SerializeField] private ConnectionButtons connectionButtons;
    private void Awake()
    {
        if(instance == null)  instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        if (autoHost) connectionButtons.StartHost();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiViews.storeView.activeSelf)
        {
            isViewsActivate = true;
        }else isViewsActivate = false;
    }

    public void BuyBullet()
    {
        if (modelCoinWallet.dummyCoins.Value <= 0)
        {
            Debug.Log("Cant Buy Bullet, dummy coin = 0");
            return;
        }

        modelCoinWallet.SetDummyCoinsServerRpc(1);
        SetCoinText(modelCoinWallet.GetTotalCoins());
        fireProjectile.AddedBulletServerRpc(1);
    }

    [ClientRpc]
    public void SetUITotalCoinClientRpc(int coin)
    {
        SetCoinText(coin);
    }

    public void SetCoinText(int coin)
    {
        uiViews.coinText.text = $"Coins: {coin}";
    }
}
