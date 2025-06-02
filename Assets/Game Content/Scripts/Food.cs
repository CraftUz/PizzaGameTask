using UnityEngine;


public class Food : MonoBehaviour
{
    [Header("Swipe Side Settings")]
    [Space(10)]
    public bool swipeLeftOrRight;
    public bool swipeUpOrDown;

    [Space(20)]

    [Header("Color Settings")]
    [Space(10)]
    [SerializeField] private ColorEnum.Color color;

    [Space(20)]

    [Header("Swipe Movement Settings")]

    [Space(10)]

    [SerializeField] private float minXSwipePos;
    [SerializeField] private float maxXSwipePos;
    [SerializeField] private float minZSwipePos;
    [SerializeField] private float maxZSwipePos;

    [SerializeField] private LevelMovementData levelMovementData;
    [Header("Destroy Effect")]
    [SerializeField] private GameObject DestroyEffect;


    private Rigidbody rb;
    private FoodManager foodManager;


    private void Awake()
    {
        LoadLevelMovementData();
        foodManager = FindObjectOfType<FoodManager>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }



    public void SwipeLeftOrRight(float Speed,bool LeftSwipe = true)
    {
        if (LeftSwipe)
        {
            if(transform.position.x > minXSwipePos)
            {
                rb.position = new Vector3(transform.position.x - 0.01f * Speed, transform.position.y, transform.position.z);
            }
        }
        else if (!LeftSwipe)
        {
            if(transform.position.x < maxXSwipePos)
            {
                rb.position = new Vector3(transform.position.x + 0.01f * Speed, transform.position.y, transform.position.z);
            }
        }
    }





    public void SwipeUpOrDown(float Speed, bool UpSwipe = true)
    {
        if (UpSwipe)
        {
            if (transform.position.z > minZSwipePos)
            {
                rb.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f * Speed);
            }
        }
        else if (!UpSwipe)
        {
            if (transform.position.z < maxZSwipePos)
            {
                rb.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.01f * Speed);
            }
        }
    }




    public void LoadLevelMovementData()
    {
        if(levelMovementData == null)
        {
            //Debug.LogWarning("Level Data is Null , Please Assign Level Data First!");
            return;
        }
        minXSwipePos = levelMovementData.minX;
        maxXSwipePos = levelMovementData.maxX;
        minZSwipePos = levelMovementData.minZ;
        maxZSwipePos = levelMovementData.maxZ;
        //Debug.Log("Level Data Loading Complete!");
    }





    public void SwipeStart()
    {
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
    }


    public void SwipeOver()
    {
        rb.isKinematic = true;
    }


    public void DestroyAndEatFood()
    {
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        foodManager.FoodEaten();
        Destroy(gameObject);
        //add effects or sound Effects if you want
    }


    public ColorEnum.Color GetFoodColor()
    {
        return color;
    }
}
