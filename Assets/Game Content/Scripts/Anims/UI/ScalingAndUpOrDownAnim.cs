using UnityEngine;
using PrimeTween;
public class ScalingAndUpOrDownAnim : MonoBehaviour
{
    [SerializeField] private bool onStartAnimate;
    [Tooltip("if you do not assign Target, this Script use it self transform!")]
    [SerializeField] private Transform targetForAnimating;
    [SerializeField] private Ease easyy;
    [SerializeField] private float scalingValue;
    [SerializeField] private float UpMovementValue;
    [SerializeField] private float duratioon;
    private void Awake()
    {
        Setup();
    }
    void Setup()
    {
        if (targetForAnimating == null)
        {
            targetForAnimating = transform;
        }
        if (onStartAnimate)
        {
            ScalingAndUpAndDownAnimLoop();
        }
    }

    void ScalingAndUpAndDownAnimLoop()
    {
        Tween.Scale(targetForAnimating, endValue: scalingValue, duration: duratioon, easyy, cycleMode: CycleMode.Yoyo, cycles: -1);
        Tween.UIAnchoredPositionY(targetForAnimating.GetComponent<RectTransform>(), endValue: UpMovementValue, duration: duratioon, ease: easyy, cycleMode: CycleMode.Yoyo, cycles: -1);
    }
}
