using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DefenderManager : MonoBehaviour
{
    public static DefenderManager Instance;
    public List<Defender> defenders = new List<Defender>();
    public List<Transform> point;            // danh sách điểm spawn
    private List<bool> check = new List<bool>(); // theo dõi điểm đã spawn chưa 
    public Defender def;

    public int baseGold = 50;

    public float cooldown = 18f;
    public float timerCooldown;

    public float timeUse = 3;
    public float timerBooster;

    public float boosterFireRate = 0.1f; 
    private float boosterFireTimer;

    public bool isBooster = false;
    public bool isUseBooster = false;

    public Image imageButton;
    public Image imageBooster;

    private Color colorButton;
    private Color colorBooster;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        timerCooldown = cooldown;
        isBooster = true;
        baseGold = 50;

        for (int i = 0; i < point.Count; i++)
        {
            check.Add(false);
        }
        Defender newDef = Instantiate(def, point[0].position, Quaternion.identity);
        defenders.Add(newDef);
        check[0] = true;

        cooldown = 18f;
        timeUse = 3f;
    }

    public void Update()
    {
        if (isUseBooster)
        {
            boosterFireTimer += Time.deltaTime;
            timerBooster += Time.deltaTime;

            if (boosterFireTimer >= boosterFireRate)
            {
                FireAllDefenders();
                boosterFireTimer = 0f;
            }

            if (timerBooster >= timeUse)
            {
                isUseBooster = false;
                isBooster = false;
                timerCooldown = 0f;
            }
        }

        // Hồi booster
        if (!isBooster && !isUseBooster)
        {
            timerCooldown += Time.deltaTime;
            if (timerCooldown >= cooldown)
            {
                isBooster = true;
                timerCooldown = 0f;
            }
        }

        Booster();
    }
    private void FireAllDefenders()
    {
        foreach (var d in defenders)
        {
            d.Fire();
        }
    }

    public void ButtonShoot()
    {
        if (isUseBooster) return;
        FireAllDefenders();
    }

    public void UseBooster()
    {
        if (!isBooster) return;

        isUseBooster = true;
        timerBooster = 0f;
        boosterFireTimer = 0f;
    }

    private void Booster()
    {
        if (isBooster)
        {
            ColorUtility.TryParseHtmlString("#FFFFFF", out colorButton);
            imageButton.color = colorButton;
            ColorUtility.TryParseHtmlString("#FFF40C", out colorBooster);
            imageBooster.color = colorBooster;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#A87878", out colorButton);
            imageButton.color = colorButton;
            ColorUtility.TryParseHtmlString("#000000", out colorBooster);
            imageBooster.color = colorBooster;
        }
    }
    public void BuyBomber()
    {
        int gold = baseGold + ((int)ManagerGame.Instance.stats.defender - 1) * 25;
        if (ManagerGame.Instance.stats.gold >= gold && def != null)
        {
            for (int i = 0; i < point.Count; i++)
            {
                if (!check[i] && point[i] != null)
                {
                    // instantiate tại vị trí point[i]
                    Defender newDef = Instantiate(def, point[i].position, Quaternion.identity);
                    defenders.Add(newDef);

                    // đánh dấu đã spawn 
                    check[i] = true;
                    ManagerGame.Instance.stats.gold -= gold;
                    return;
                }
            }
        }
    }
    public bool FullDefender()
    {
        return (point.Count == defenders.Count);
    }

    public void ApplyStatsAll(PlayerStatsManager stats)
    {
        cooldown = (float)(18 - (stats.booster - 1) * 1.5);
        timeUse = (float)(3 + (stats.booster - 1) * 0.8);
    }
}
