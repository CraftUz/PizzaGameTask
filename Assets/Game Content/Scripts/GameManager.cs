using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent onWinGame;
    [SerializeField] private UnityEvent onWinGameForParticle;
    
    public void Win()
    {
        AudioManager.Instance.PlaySFX("Win");
        onWinGameForParticle.Invoke();
        Tween.Delay(1).OnComplete(target: this, target => onWinGame.Invoke());
    }
}
