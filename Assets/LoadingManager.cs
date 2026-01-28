using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public GameObject loading;
    public TextMeshProUGUI percent;

    private Vector3 originalScale;
    public float fakeLoadTime = 3f;
    private float timer = 0f;

    private AsyncOperation loadOp;
    private bool isTransitioning = false;

    void Start()
    {
        originalScale = loading.transform.localScale;
        loading.transform.localScale = new Vector3(0, originalScale.y, originalScale.z);

        loadOp = SceneManager.LoadSceneAsync("GameScene");
        loadOp.allowSceneActivation = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float fakeRatio = Mathf.Clamp01(timer / fakeLoadTime);

        Vector3 scale = originalScale;
        scale.x = originalScale.x * fakeRatio;

        loading.transform.localScale = Vector3.Lerp(
            loading.transform.localScale,
            scale,
            Time.deltaTime * 6f
        );

        percent.text = Mathf.RoundToInt(fakeRatio * 100f) + "%";

        if (fakeRatio >= 1f && loadOp.progress >= 0.9f && !isTransitioning)
        {
            loadOp.allowSceneActivation = true;
        }
    }
}
