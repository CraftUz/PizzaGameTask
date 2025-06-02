using UnityEngine;
using PrimeTween;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private GameObject TargetForFadeAnim;
    private CanvasGroup canvasGroup;
    void Start()
    {
        Setup();
    }
    private void OnEnable()
    {
        if(canvasGroup == null)
        {
            Setup();
        }
        OpenPannel();
    }
    void Setup()
    {
        if (TargetForFadeAnim == null)
        {
            TargetForFadeAnim = gameObject;
            canvasGroup = GetComponent<CanvasGroup>();
        }
        else
        {
            canvasGroup = TargetForFadeAnim.GetComponent<CanvasGroup>();
        }
    }
    public void OpenPannel()
    {
        Debug.Log("Open pannel called by:" + gameObject.name);
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
        Tween.Alpha(canvasGroup, duration: 0.5f, endValue: 1);
    }
    public void OpenPannelWithDelay(float delay)
    {
        Debug.Log("open pannel with delay called");
        Tween.Delay(delay).OnComplete(target: this, target => target.OpenPannel());
    }
    public void ClosePannelWithDelay(float delay)
    {
        Tween.Delay(delay).OnComplete(target: this, target => target.ClosePannel());
    }
    public void ClosePannel()
    {
        Tween.Alpha(canvasGroup, duration: 0.5f, endValue: 0).OnComplete(() => gameObject.SetActive(false));
    }
}
