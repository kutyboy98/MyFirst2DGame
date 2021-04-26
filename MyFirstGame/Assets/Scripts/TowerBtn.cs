using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject tower;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Text priceTxt;
    [SerializeField]
    private int price;
    public GameObject Tower { get => tower; }
    public Sprite Sprite { get => sprite; }
    public int Price { get => price; }
    private Color32 redColor = new Color32(255, 101, 0, 255);
    private Color32 noneColor = new Color32(96, 255, 90, 255);

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.priceTxt.text = $"{this.price}<color=yellow>$</color>";
        PriceCheck();
        GameManager.Instance.CurrencyChangedI += PriceCheck;
    }

    private void SetBackground(Color32 color)
    {
        spriteRenderer.color = color;
    }

    private void PriceCheck()
    {
        if (price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceTxt.color = Color.grey;
        }
    }

    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;
        switch (type)
        {
            case "Fire":
                FireTowerManager fireTowerManager = tower.GetComponentInChildren<FireTowerManager>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Fire</b></size></color>\nDamage: {0}\nTick Time: {1}\nTick Damage: {2}\nProc: {3}\nSpeed: {4}", fireTowerManager.Damage, fireTowerManager.TickTime, fireTowerManager.TickDamage,fireTowerManager.Proc, fireTowerManager.AttackSpeed);
                break;
            case "Headache":
                HeadacheTowerManager headacheTowerManager = tower.GetComponentInChildren<HeadacheTowerManager>();
                tooltip = string.Format("<color=#00ff00ff><size=20><b>Headache</b></size></color>\nDamage: {0}\nTick Time: {1}\nTick Damage: {2}\nProc: {3}\nSpeed: {4}", headacheTowerManager.Damage, headacheTowerManager.TickTime, headacheTowerManager.TickDamage,headacheTowerManager.Proc, headacheTowerManager.AttackSpeed);
                break;
            case "Ice":
                FrostTowerManager frostTowerManager = tower.GetComponentInChildren<FrostTowerManager>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Ice</b></size></color>\nDamage: {0}\nSlowing Factor: {1}\nProc: {2}\nSpeed: {3}", frostTowerManager.Damage, frostTowerManager.SlowingFactor, frostTowerManager.Proc, frostTowerManager.AttackSpeed);
                break;
        }
        GameManager.Instance.SetStatsText(tooltip);
        GameManager.Instance.ShowStats();
    }
}
