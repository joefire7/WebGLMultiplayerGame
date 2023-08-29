using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIViews : MonoBehaviour
{
    public GameObject storeView;
    public GameObject coinView;

    public Button closeStoreView;
    public Button openStoreView;
    public Button buyBtn;
    public TMP_Text coinText;

    private void Start()
    {
        openStoreView.onClick.AddListener(() => { ShowStoreView(true); });
        closeStoreView.onClick.AddListener(() => { ShowStoreView(false); });
        buyBtn.onClick.AddListener(() => { UIController.instance.BuyBullet(); });
    }
    public void ShowStoreView(bool show) => storeView.SetActive(show);
    public void ShowCoinView(bool show) => coinView.SetActive(show);
   
}
