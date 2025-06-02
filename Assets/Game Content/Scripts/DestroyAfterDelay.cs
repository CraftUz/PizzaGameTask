using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] private float delayForDestroy;
    void Start()
    {
        Destroy(gameObject, delayForDestroy);
    }
}
