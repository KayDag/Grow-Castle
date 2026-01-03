using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SocialPlatforms;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public GameObject blood;
    private Vector3 originalScaleBlood;

    public GameObject progress;
    private Vector3 originalScaleProgress;

    public Canvas playerStatsCanvas;
    public Canvas gameCanvas;
    public Canvas homeCanvas;
    public Canvas pauseCanvas;
    public Canvas winCanvas;
    public Canvas loseCanvas;
    public Canvas statsCanvas;
    public Canvas defaultCanvas;

    public TextMeshProUGUI hp;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI cooldownBooster;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI goldDefender;
    public TextMeshProUGUI goldAttacker;
    public TextMeshProUGUI goldNeed;
    public TextMeshProUGUI addSkills;

    public PlayerStatsManager previewStats;
    private List<int> scoreSkill = new List<int>() { 1, 2, 2, 3};

    // Start is called before the first frame update
    void Start()
    {
        originalScaleBlood = blood.transform.localScale;

        defaultCanvas.gameObject.SetActive(true);
        homeCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        playerStatsCanvas.gameObject.SetActive(false);
        statsCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);
        loseCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);

        previewStats = new PlayerStatsManager();
    }

    // Update is called once per frame
    void Update()
    {
        BloodCastle();
    }
    //Blood of Castle
    public void BloodCastle()
    {
        float ratio = Castle.Instance.health / Castle.Instance.healthStay;
        ratio = Mathf.Clamp01(ratio);

        Vector3 scale = originalScaleBlood;
        scale.x = originalScaleBlood.x * ratio;

        blood.transform.localScale = Vector3.Lerp(blood.transform.localScale, scale, Time.deltaTime * 8f);
    }
    //Progress
    public void Progress()
    {
        float ratio = ManagerGame.Instance.wave / ManagerGame.Instance.checkPointWave.Count;
        ratio = Mathf.Clamp01(ratio);

        Vector3 scale = originalScaleProgress;
        scale.x = originalScaleProgress.x * ratio;

        progress.transform.localScale = scale;
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
        hp.text = ManagerGame.Instance.stats.healthCastle.ToString();
        speed.text = ManagerGame.Instance.stats.speedAttacker.ToString();
        damage.text = ManagerGame.Instance.stats.damageDefender.ToString();
        cooldownBooster.text = ManagerGame.Instance.stats.cooldownBooster.ToString();
    }
    //Play Game
    public void PlayGame()
    {
        homeCanvas.gameObject.SetActive(false);
        ManagerGame.Instance.isGame = true;
        gameCanvas.gameObject.SetActive(true);
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
    }
    //Back home after reward
    public void BackHomeReward()
    {
        statsCanvas.gameObject.SetActive(false);
        BackHome();
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
        previewStats.healthCastle++;
        UpdateUI();
    }
    //Add Speed Attacker
    public void AddSpeed()
    {
        previewStats.speedAttacker++;
        UpdateUI();
    }
    //Add Damage Defender
    public void AddDamage()
    {
        previewStats.damageDefender++;
        UpdateUI();
    }
    //Add cooldown booster
    public void AddCoolDown()
    {
        previewStats.cooldownBooster++;
        UpdateUI();
    }
    //Complete
    public void Complete()
    {
        ManagerGame.Instance.stats.healthCastle = previewStats.healthCastle;
        ManagerGame.Instance.stats.speedAttacker = previewStats.speedAttacker;
        ManagerGame.Instance.stats.damageDefender = previewStats.damageDefender;
        ManagerGame.Instance.stats.cooldownBooster = previewStats.cooldownBooster;
        statsCanvas.gameObject.SetActive(false);
        BackHome();
    }
    private void UpdateUI()
    {
        hp.text = previewStats.healthCastle.ToString();
        speed.text = previewStats.speedAttacker.ToString();
        damage.text = previewStats.damageDefender.ToString();
        cooldownBooster.text = previewStats.cooldownBooster.ToString();
    }
}
