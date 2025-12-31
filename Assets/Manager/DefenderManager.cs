using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderManager : MonoBehaviour
{
    public List<Defender> defenders;


    public float cooldown;
    public float timerCooldown;

    public float timeUse = 5;
    public float timerBooster;

    public float boosterFireRate = 0.15f; 
    private float boosterFireTimer;

    public bool isBooster = false;
    public bool isUseBooster = false;

    public Image imageButton;
    public Image imageBooster;

    private Color colorButton;
    private Color colorBooster;

    private void Start()
    {
        timerCooldown = 0;
        timerCooldown = cooldown;
        isBooster = true;
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

}
