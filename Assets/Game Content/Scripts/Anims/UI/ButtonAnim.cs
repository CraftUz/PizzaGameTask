using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField] private float ScaleValue;
    [SerializeField] private Quaternion RotationValue;
    [SerializeField] private float duratioon;

    private Tween scaleTween;
    private Tween rotationTween;
    private void Start()
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
    }

    public void ButtonPress()
    {
        AudioManager.Instance.PlaySFX("Button1");
        scaleTween.Stop();
        rotationTween.Stop();

        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;

        scaleTween = Tween.Scale(transform, endValue: ScaleValue, duration: duratioon, cycleMode: CycleMode.Yoyo, cycles: 2);
        rotationTween = Tween.Rotation(transform, endValue: RotationValue, duration: duratioon, cycleMode: CycleMode.Yoyo, cycles: 2);
    }
    public void CustomAnimForLamp()
    {
        scaleTween.Stop();
        rotationTween.Stop();

        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;

        scaleTween = Tween.Scale(transform, endValue: ScaleValue, duration: duratioon, cycleMode: CycleMode.Yoyo, cycles: 2);
        rotationTween = Tween.Rotation(transform, endValue: RotationValue, duration: duratioon, cycleMode: CycleMode.Yoyo, cycles: 2);
    }
}