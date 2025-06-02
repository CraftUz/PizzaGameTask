using UnityEngine;

public class Stickman : MonoBehaviour
{
    [SerializeField] private ColorEnum.Color color;
    //Add Type Food Enum if Needed!
    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponent<Food>();
        if(food != null && food.GetFoodColor() == color)
        {
            AudioManager.Instance.PlaySFX("Eat");
            food.DestroyAndEatFood();
        }
    }

}
