using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private int foodCount;
    [SerializeField] private Food[] foods;
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        foods = FindObjectsOfType<Food>();
        foodCount = foods.Length;
        //if you want you can add foods and gamemanager in inspector for optimization
    }
    public void FoodEaten()
    {
        foodCount -= 1;
        if(foodCount <= 0)
        {
            gameManager.Win();
        }
    }
}
