using UnityEngine.SceneManagement;
using UnityEngine;
using PrimeTween;
using UnityEngine.UI;
using TMPro;
public class Loading : MonoBehaviour
{
    [SerializeField] private float timeToLoading;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingProgressText;
    [SerializeField] private Ease easyy;
    [SerializeField] private LevelManager levelManager;
    Tween tween;
    void Start()
    {
        loadingBar.fillAmount = 0;
        StartLoading();
    }
    private void Update()
    {
        if (tween.isAlive)
        {
            float progress = loadingBar.fillAmount * 100;
            loadingProgressText.text = progress.ToString("0.0") + "%";
        }
    }
    void StartLoading()
    {
        tween = Tween.UIFillAmount(loadingBar, duration: timeToLoading, endValue: 1, ease: easyy).OnComplete(() => LoadNextScene());
    }
    void LoadNextScene()
    {
        loadingProgressText.text = "Complete";
        Invoke("LoadGameScene", 1);
        //check player auth and load register scene or game scene
    }
    void LoadGameScene()
    {
        levelManager.LoadLevel();
    }
}
