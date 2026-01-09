using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SocialPlatforms;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject blood;
    private Vector3 originalScaleBlood;

    public GameObject progress;
    private Vector3 originalScaleProgress;

    public GameObject checkpoint;
    private Vector3 originalScaleCheckPoint;

    public Canvas playerStatsCanvas;
    public Canvas gameCanvas;
    public Canvas homeCanvas;
    public Canvas pauseCanvas;
    public Canvas winCanvas;
    public Canvas loseCanvas;
    public Canvas statsCanvas;
    public Canvas defaultCanvas;

    public TextMeshProUGUI hp;
    public TextMeshProUGUI hpU;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI speedU;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI damageU;
    public TextMeshProUGUI cooldownBooster;
    public TextMeshProUGUI cooldownBoosterU;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI goldDefender;
    public TextMeshProUGUI goldAttacker;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI scouts;
    public TextMeshProUGUI checkpointsInNextWave;
    public TextMeshProUGUI checkpointsInGame;

    public PlayerStatsManager previewStats;
    public bool isPlaying = false;
    public bool winGame;

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
    // Start is called before the first frame update
    void Start()
    {
        originalScaleBlood = blood.transform.localScale;
        originalScaleProgress = progress.transform.localScale;
        originalScaleCheckPoint = checkpoint.transform.localScale;

        defaultCanvas.gameObject.SetActive(true);
        homeCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        playerStatsCanvas.gameObject.SetActive(false);
        statsCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);
        loseCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);

        previewStats = new PlayerStatsManager();
        Buy();
        Progress();
        AttackerManager.Instance.NewWave();
        CheckPointNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        BloodCastle();
        GoldStats();
    }
    //Blood of Castle
    public void BloodCastle()
    {
        float ratio = Castle.Instance.health / Castle.Instance.stayHealth;
        ratio = Mathf.Clamp01(ratio);

        Vector3 scale = originalScaleBlood;
        scale.x = originalScaleBlood.x * ratio;

        blood.transform.localScale = Vector3.Lerp(blood.transform.localScale, scale, Time.deltaTime * 8f);
    }
    //Progress
    public void Progress()
    {
        float ratio = (float)ManagerGame.Instance.currentWave / ManagerGame.Instance.checkPoint.Count;
        ratio = Mathf.Clamp01(ratio);

        Vector3 scale = originalScaleProgress;
        scale.x = originalScaleProgress.x * ratio;

        progress.transform.localScale = scale;

        progressText.text = "Progress: " + ManagerGame.Instance.currentWave.ToString() + "/" + ManagerGame.Instance.checkPoint.Count.ToString();
    }
    public void CheckPoint()
    {
        checkpointsInGame.text = ManagerGame.Instance.count.ToString() + "/" + ManagerGame.Instance.checkPoint[ManagerGame.Instance.currentWave].ToString();
        if (ManagerGame.Instance.count <= ManagerGame.Instance.checkPoint[ManagerGame.Instance.currentWave])
        {
            float ratio = (float)ManagerGame.Instance.count / ManagerGame.Instance.checkPoint[ManagerGame.Instance.currentWave]; ;
            ratio = Mathf.Clamp01(ratio);

            Vector3 scale = originalScaleCheckPoint;
            scale.x = originalScaleCheckPoint.x * ratio;

            checkpoint.transform.localScale = scale;
        }
    }

    public void Buy()
    {
        goldAttacker.text = (AttackerManager.Instance.baseGold + 
            ((int)ManagerGame.Instance.stats.attacker - 1) * 5).ToString();
        goldDefender.text = (DefenderManager.Instance.baseGold +
            ((int)ManagerGame.Instance.stats.defender - 1) * 5).ToString();
    }
    //scouts
    public void Scouts()
    {
        scouts.text = (AttackerManager.Instance.attacker.Count).ToString();
    }
    //Gold
    public void GoldStats()
    {
        gold.text = ManagerGame.Instance.stats.gold.ToString();
    }

    //Home Canvas
    //Tắt bảng chỉ số
    public void ExitStats()
    {
        playerStatsCanvas.gameObject.SetActive(false);
    }
    //Mở chỉ số
    public void OpenStats()
    {
        playerStatsCanvas.gameObject.SetActive(true);
        hp.text = ManagerGame.Instance.stats.castle.ToString();
        speed.text = ManagerGame.Instance.stats.attacker.ToString();
        damage.text = ManagerGame.Instance.stats.defender.ToString();
        cooldownBooster.text = ManagerGame.Instance.stats.booster.ToString();
    }
    //Play Game
    public void PlayGame()
    {
        homeCanvas.gameObject.SetActive(false);
        ManagerGame.Instance.isGame = true;
        CheckPoint();
        gameCanvas.gameObject.SetActive(true);
    }
    public void CheckPointNextWave()
    {
        checkpointsInNextWave.text = ManagerGame.Instance.checkPoint[ManagerGame.Instance.currentWave].ToString();
    }
    //BuyScout
    public void BuyScout()
    {
        if (AttackerManager.Instance.FullAttacker())
        {
            goldAttacker.text = "--";
        }
        else
        {
            AttackerManager.Instance.AddAttacker();
        }
        Scouts();
    }
    //BuyBomber
    public void BuyBomber()
    {
        if (DefenderManager.Instance.FullDefender())
        {
            goldDefender.text = "--";
        }
        else
        {
            DefenderManager.Instance.BuyBomber();
        }
    }

    //Game Canvas
    //Pause Game
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseCanvas.gameObject.SetActive(true);
    }
    //Back home when pause
    public void BackHomePause()
    {
        pauseCanvas.gameObject.SetActive(false);
        BackHome();
    }
    //Resume Game
    public void ResumeGame()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    //Reset Game
    public void Replay()
    {

    }
    //Lose Game
    public void LoseGame()
    {
        defaultCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        loseCanvas.gameObject.SetActive(true);
    }
    //Back Game when lose
    public void BackHomeLose()
    {
        loseCanvas.gameObject.SetActive(false);
        BackHome();
    }
    //Back Home = tap vao man hinh
    public void BackHome()
    {
        homeCanvas.gameObject.SetActive(true);
        defaultCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        Progress();
        Buy();
        Scouts();
        AttackerManager.Instance.NewWave();
        CheckPointNextWave();
    }

    //Win game
    public void WinGame()
    {
        defaultCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(true);
    }
    //Award
    public void Reward()
    {
        winCanvas.gameObject.SetActive(false);
        statsCanvas.gameObject.SetActive(true);
        OpenUpdateStats();
    }
    //Open UpdateStats || Reset
    public void OpenUpdateStats()
    {
        previewStats = ManagerGame.Instance.stats.Clone();
        UpdateUI();
    }
    //Add HP
    public void AddHP()
    {
        previewStats.castle++;
        UpdateUI();
    }
    //Add Speed Attacker
    public void AddSpeed()
    {
        previewStats.attacker++;
        UpdateUI();
    }
    //Add Damage Defender
    public void AddDamage()
    {
        previewStats.defender++;
        UpdateUI();
    }
    //Add cooldown booster
    public void AddCoolDown()
    {
        previewStats.booster++;
        UpdateUI();
    }
    //Complete
    public void Complete()
    {
        ManagerGame.Instance.stats.castle = previewStats.castle;
        ManagerGame.Instance.stats.attacker = previewStats.attacker;
        ManagerGame.Instance.stats.defender = previewStats.defender;
        ManagerGame.Instance.stats.booster = previewStats.booster;
        statsCanvas.gameObject.SetActive(false);
        ManagerGame.Instance.UpdateStats();
        BackHome();
    }
    //Reset
    public void ResetStats()
    {
        OpenUpdateStats();
    }
    private void UpdateUI()
    {
        hpU.text = previewStats.castle.ToString();
        speedU.text = previewStats.attacker.ToString();
        damageU.text = previewStats.defender.ToString();
        cooldownBoosterU.text = previewStats.booster.ToString();
    }
}
