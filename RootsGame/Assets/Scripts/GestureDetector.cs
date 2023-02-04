using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureDetector : MonoBehaviour
{
    [SerializeField] private float minUmbralTime = 0.1f, maxUmbralTime = 1f, minUmbralDistance = 1f;
    public UnityEvent onLeft, onRight, onLeftDown, onRightDown, onDown;

    private Vector2 initialPositionFirstTouch, initialPositionSecondTouch;
    private int touches = 0;
    private float timeGesture = 0f;

    public enum Direction
    {
        None, Left, Right, LeftDown, Down, RightDown
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.touches[0];
            if (t.phase == TouchPhase.Began)
            {
                initialPositionFirstTouch = t.position;
                touches = 1;
                timeGesture = Time.time;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (touches != 1)
                    return;
                if (Time.time - timeGesture < minUmbralTime)
                    return;
                if (Time.time - timeGesture > maxUmbralTime)
                    return;

                switch (DetectDirection())
                {
                    case Direction.Left:
                        onLeft.Invoke();
                        break;
                    case Direction.Right:
                        onRight.Invoke();
                        break;
                    case Direction.LeftDown:
                        onLeftDown.Invoke();
                        break;
                    case Direction.Down:
                        onDown.Invoke();
                        break;
                    case Direction.RightDown:
                        onRightDown.Invoke();
                        break;
                }
            }
        }
        else
        {
            touches = Input.touchCount;
        }
        
    }

    private Direction DetectDirection()
    {
        Touch t = Input.touches[0];
        float swipeVertical = (new Vector2(0f, t.position.y) - new Vector2(0f, initialPositionFirstTouch.y)).magnitude;
        float swipeHorizontal = (new Vector2(t.position.x, 0f) - new Vector2(initialPositionFirstTouch.x, 0f)).magnitude;
        if (swipeVertical > minUmbralDistance && swipeHorizontal < minUmbralDistance)
        {
            if (initialPositionFirstTouch.y < t.position.y)
                return Direction.None;
            else
                return Direction.Down;
        }
        else if (swipeHorizontal > minUmbralDistance && swipeVertical > minUmbralDistance)
        {
            if (initialPositionFirstTouch.x < t.position.x)
                return Direction.Right;
            else
                return Direction.Left;
        }
        else if (swipeHorizontal > minUmbralDistance && swipeVertical > minUmbralDistance)
        {
            if (initialPositionFirstTouch.y < t.position.y)
                return Direction.None;
            else if (initialPositionFirstTouch.x < t.position.x)
                return Direction.RightDown;
            else
                return Direction.LeftDown;
        }
        else
            return Direction.None;



    }
}
