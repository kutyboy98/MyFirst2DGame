using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public delegate void CurrencyChanged();

public class GameManager : SingleTon<GameManager>
{
    public event CurrencyChanged CurrencyChangedI;
    public TowerBtn TowerBtnSelection { get; set; }
    public int Currency
    {
        get => currency;
        set
        {
            currency = value;
            this.currencyTxt.text = $"{currency} <color=yellow>$</color>";
            OnCurrencyChanged();
        }
    }
    private int currency;
    [SerializeField]
    private Text currencyTxt;
    [SerializeField]
    private GameObject waveBtn;
    [SerializeField]
    private Text waveTxt;
    [SerializeField]
    private Text heartTxt;
    [SerializeField]
    private GameObject menuImage;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private GameObject upgradePanel;
    [SerializeField]
    private Text sellText;
    [SerializeField]
    private Text upgradeText;
    [SerializeField]
    private GameObject statsPanel;
    [SerializeField]
    private Text statsText;
    [SerializeField]
    private GameObject inGameMenu;
    [SerializeField]
    private GameObject optionMenu;
    private List<MonsterManager> monsterManagers = new List<MonsterManager>();
    private TowerManager towerSelected;
    private int lives;
    private int totalMonsters;
    public ObjectPool Pool { get; set; }
    public int wave = 0;
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
            heartTxt.text = lives.ToString();
            if (lives <= 0)
            {
                lives = 0;
                menuImage.SetActive(true);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lives = 2;
        Currency = 100;
        menuImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        OnKeyPressed();
    }

    public void SelectTower(TowerBtn towerSelected)
    {
        Hover.Instance.DeactiveSprite();
        if (Currency >= towerSelected.Price && waveBtn.activeInHierarchy)
        {
            this.TowerBtnSelection = towerSelected;
            Hover.Instance.ActiveSprite(towerSelected.Sprite);
        }
    }

    public void BuyTower()
    {
        Currency = Currency - TowerBtnSelection.Price;
        Hover.Instance.DeactiveSprite();
    }

    public void StartWave()
    {
        wave++;
        waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        waveBtn.SetActive(false);
        upgradePanel.SetActive(false);
        GrassManager.Instance.Path = null;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        totalMonsters = wave;
        for (int i = 0; i < wave; i++)
        {
            var random = new System.Random();
            //MonsterManager monsterManager = Pool.GetObject<MonsterTypeEnumes>(random.NextEnum<MonsterTypeEnumes>()).GetComponent<MonsterManager>();
            MonsterManager monsterManager = Pool.GetObject<MonsterTypeEnumes>(MonsterTypeEnumes.BaldCrime).GetComponent<MonsterManager>();
            monsterManager.Spawn(maxHealth);
            monsterManagers.Add(monsterManager);
            if (wave % 3 == 0)
            {
                maxHealth += 5;
            }
            yield return new WaitForSeconds(2.5f);
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    private void OnKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TowerBtnSelection != null && Hover.Instance.IsVisible)
            {
                DropTower();
            }
            else if (towerSelected != null && upgradePanel.activeSelf)
            {
                DeselectTower();
            }
            else if(optionMenu.activeSelf)
            {
                ShowOptionMenu();
            }
            else
            {
                ShowInGameMenu();
            }
        }
    }

    public void RemoveMonster(MonsterManager monsterManager)
    {
        monsterManagers.Remove(monsterManager);
        totalMonsters--;
        Debug.Log(totalMonsters);
        if (!monsterManagers.Any() && !menuImage.activeSelf && totalMonsters <= 0)
        {
            waveBtn.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SelectTower(TowerManager tower)
    {
        DeselectTower();
        towerSelected = tower;
        towerSelected.Select();
        if (waveBtn.activeInHierarchy)
        {
            upgradePanel.SetActive(true);
            sellText.text = towerSelected.PriceText;
            upgradeText.text = towerSelected.UpgradeText;
            UpgradeStats();
        }
    }

    public void DeselectTower()
    {
        if (towerSelected != null)
        {
            towerSelected.Deselect();
        }
        upgradePanel.SetActive(false);
    }

    public void SellTower()
    {
        if (towerSelected != null)
        {
            Currency += (towerSelected.Price / 2);
            towerSelected.GetComponentInParent<PointManager>().isEmpty = true;
            Destroy(towerSelected.transform.parent.gameObject);
            DeselectTower();
        }
    }

    public void OnCurrencyChanged()
    {
        if (CurrencyChangedI != null)
        {
            CurrencyChangedI();
        }
    }

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void ShowTowerStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpgradeStats();
    }

    public void SetStatsText(string txt)
    {
        statsText.text = txt;
    }

    public void UpgradeStats()
    {
        if (towerSelected != null)
        {
            SetStatsText(towerSelected.GetStats());
        }
    }

    public void UpgradeTowerHandle()
    {
        if (towerSelected != null)
        {
            if (towerSelected.NextUpgradeStats != null && currency > towerSelected.NextUpgradeStats.Price)
            {
                towerSelected.Upgrade();
                sellText.text = towerSelected.PriceText;
                upgradeText.text = towerSelected.UpgradeText;
            }
        }
    }

    public void ShowInGameMenu()
    {
        inGameMenu.SetActive(!inGameMenu.activeSelf);
        if (inGameMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ShowOptionMenu()
    {
        inGameMenu.SetActive(!inGameMenu.activeSelf);
        optionMenu.SetActive(!inGameMenu.activeSelf);
    }

    private void DropTower()
    {
        TowerBtnSelection = null;
        Hover.Instance.DeactiveSprite();
    }
}
