using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderManager : MonoBehaviour
{
    public static DefenderManager Instance;
    public List<Defender> defenders = new List<Defender>();
    public List<Transform> point;            // danh sách điểm spawn
    private List<bool> check = new List<bool>(); // theo dõi điểm đã spawn chưa 
    public Defender def;
    public List<GameObject> ball;

    public int baseGold = 35;

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

    public GameObject vfxUseBooster;
    public Transform vfxTransform;

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
        baseGold = 35;

        for (int i = 0; i < point.Count; i++)
        {
            check.Add(false);
        }
        int startCount = Mathf.Min(ManagerGame.Instance.bomber, point.Count);
        for (int i = 0; i < startCount; i++)
        {
            Defender newDef = Instantiate(def, point[i].position, Quaternion.identity);
            defenders.Add(newDef);
            check[i] = true;
        }

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
        if (!isBooster || isUseBooster) return;

        AudioManager.Instance.PlayUseBooster();
        if (vfxUseBooster != null)
        {
            Instantiate(vfxUseBooster, vfxTransform.position, Quaternion.identity);
        }

        isUseBooster = true;
        isBooster = false;
        timerBooster = 0f;
        boosterFireTimer = 0f;
    }

    public void ResetBooster()
    {
        isUseBooster = false;
        isBooster = true;       // cho dùng lại booster
        timerBooster = 0f;
        timerCooldown = cooldown;
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
        int gold = baseGold + ((int)ManagerGame.Instance.stats.defender - 1) * 10;
        if (ManagerGame.Instance.stats.gold < gold) return;
        if (defenders.Count >= point.Count) return;

        int index = defenders.Count;

        Defender newDef = Instantiate(def, point[index].position, Quaternion.identity);
        defenders.Add(newDef);

        ManagerGame.Instance.bomber++;
        ManagerGame.Instance.stats.gold -= gold;
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
    public void DestroyBall()
    {
        for (int i = ball.Count - 1; i >= 0; i--)
        {
            if (ball[i] != null)
                Destroy(ball[i]);
            ball.RemoveAt(i);
        }
    }
}
