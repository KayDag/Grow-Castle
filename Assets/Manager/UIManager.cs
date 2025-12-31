using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameObject blood;
    private Vector3 originalScale;

    public Canvas statsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = blood.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        BloodCastle();
    }

    public void BloodCastle()
    {
        float ratio = Castle.Instance.health / Castle.Instance.healthStay;
        ratio = Mathf.Clamp01(ratio);

        Vector3 scale = originalScale;
        scale.x = originalScale.x * ratio;

        blood.transform.localScale = Vector3.Lerp(blood.transform.localScale, scale, Time.deltaTime * 8f);
    }

    public void ExitStats()
    {
        statsCanvas.gameObject.SetActive(false);
    }

    public void OpenStats()
    {
        statsCanvas.gameObject.SetActive(true);
    }
}
