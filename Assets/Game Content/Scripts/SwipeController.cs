using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float swipeSpeed;
    [SerializeField] private LayerMask FoodLayer;
    private bool isSwiping;
    private Vector3 lastTouchPos;
    private Food selectedFood;
    private Vector3 lastmousePos;
    void Update()
    {
        HandleTouch();
        
    }
    private void FixedUpdate()
    {
        
    }
    void HandleSelectFood()
    {
        
        //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, FoodLayer))
        {
            if(hit.collider != null)
            {
                if (selectedFood == null && !isSwiping)
                {
                    selectedFood = hit.collider.gameObject.GetComponent<Food>();
                    selectedFood.SwipeStart();
                    isSwiping = true;
                }
                else if(selectedFood != null && !isSwiping)
                {
                    selectedFood.SwipeOver();
                    selectedFood = null;
                }
                else if(selectedFood == null && isSwiping)
                {
                    selectedFood = hit.collider.gameObject.GetComponent<Food>();
                    selectedFood.SwipeStart();
                }
            }
            else
            {
                selectedFood = null;
            }
        }
    }
    void HandleTouch()
    {
        /*
        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("ResetMousePosition", 0.2f, 0.1f);
            Debug.Log("Mouse Working");
            HandleSelectFood();
        }
        else if (Input.GetMouseButton(0))
        {
            
            if (selectedFood == null)  return;

            if (selectedFood.swipeLeftOrRight)
            {
                if (mousePos.x < lastmousePos.x)
                {
                    selectedFood.SwipeLeftOrRight(swipeSpeed);
                }
                else if (mousePos.x > lastmousePos.x)
                {
                    selectedFood.SwipeLeftOrRight(swipeSpeed, false);
                }

            }
            else if (selectedFood.swipeUpOrDown)
            {
                if (mousePos.y < lastmousePos.y)
                {
                    selectedFood.SwipeUpOrDown(swipeSpeed);
                }
                else if (mousePos.y > lastmousePos.y)
                {
                    selectedFood.SwipeUpOrDown(swipeSpeed, false);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(selectedFood != null)
            {
                selectedFood.SwipeOver();
                selectedFood = null;
                isSwiping = false;
            }
        }*/
        
        if(Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
            {
                lastTouchPos = Input.GetTouch(0).position;
                //InvokeRepeating("ResetTouchPosition", 0.2f, 0.1f);
                HandleSelectFood();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (selectedFood == null) return;

                if (selectedFood.swipeLeftOrRight)
                {
                    if (touch.position.x < lastTouchPos.x)
                    {
                        selectedFood.SwipeLeftOrRight(swipeSpeed);
                    }
                    else if (touch.position.x > lastTouchPos.x)
                    {
                        selectedFood.SwipeLeftOrRight(swipeSpeed, false);
                    }
                }
                else if (selectedFood.swipeUpOrDown)
                {
                    if (touch.position.y < lastTouchPos.y)
                    {
                        selectedFood.SwipeUpOrDown(swipeSpeed);
                    }
                    else if (touch.position.y > lastmousePos.y)
                    {
                        selectedFood.SwipeUpOrDown(swipeSpeed, false);
                    }
                }

            }
            else if(touch.phase == TouchPhase.Ended)
            {
                if(selectedFood != null)
                {
                    selectedFood.SwipeOver();
                    selectedFood = null;
                    isSwiping = false;
                }
                
            }
        }
        else
        {
            if (selectedFood != null)
            {
                selectedFood.SwipeOver();
                selectedFood = null;
                isSwiping = false;
            }
        }
        
        
    }
    void ResetMousePosition()
    {
        lastmousePos = Input.mousePosition;
    }
    void ResetTouchPosition()
    {
        lastTouchPos = Input.GetTouch(0).position;
    }
}
