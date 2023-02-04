using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureDetector : MonoBehaviour
{
    [SerializeField] private float minUmbralTime = 0.1f, maxUmbralTime = 1f, minUmbralDistance = 1f;
    public UnityEvent onLeft, onRight, onLeftDown, onRightDown, onDown, onNone;

    private Vector2 initialPositionFirstTouch, initialPositionSecondTouch;
    private int touches = 0;
    private float timeGesture = 0f;

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
                    case Directions.Left:
                        onLeft.Invoke();
                        break;
                    case Directions.Right:
                        onRight.Invoke();
                        break;
                    case Directions.LeftDown:
                        onLeftDown.Invoke();
                        break;
                    case Directions.Down:
                        onDown.Invoke();
                        break;
                    case Directions.RightDown:
                        onRightDown.Invoke();
                        break;
                    default:
                        onNone.Invoke();
                        break;
                }
            }
        }
        else
        {
            touches = Input.touchCount;
        }
        
    }

    private Directions DetectDirection()
    {
        Touch t = Input.touches[0];
        Vector2 direction = t.position - initialPositionFirstTouch;
        //float swipeVertical = (new Vector2(0f, t.position.y) - new Vector2(0f, initialPositionFirstTouch.y)).magnitude;
        //float swipeHorizontal = (new Vector2(t.position.x, 0f) - new Vector2(initialPositionFirstTouch.x, 0f)).magnitude;
        //if (swipeVertical > minUmbralDistance && swipeHorizontal < minUmbralDistance)
        //{
        //    Debug.Log(initialPositionFirstTouch + " 1to " + t.position);
        //    if (initialPositionFirstTouch.y < t.position.y)
        //        return Direction.None;
        //    else
        //        return Direction.Down;
        //}
        //else if (swipeHorizontal > minUmbralDistance && swipeVertical < minUmbralDistance)
        //{
        //    Debug.Log(initialPositionFirstTouch + " 2to " + t.position);
        //    if (initialPositionFirstTouch.x < t.position.x)
        //        return Direction.Right;
        //    else
        //        return Direction.Left;
        //}
        //else if (swipeHorizontal > minUmbralDistance && swipeVertical > minUmbralDistance)
        //{
        //    Debug.Log(initialPositionFirstTouch + " 3to " + t.position);
        //    if (initialPositionFirstTouch.y < t.position.y)
        //        return Direction.None;
        //    else if (initialPositionFirstTouch.x < t.position.x)
        //        return Direction.RightDown;
        //    else
        //        return Direction.LeftDown;
        //}
        //else
        //    return Direction.None;

        if (direction.magnitude < minUmbralDistance)
            return Directions.None;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Debug.Log(angle);

        if (angle >= -15f && angle <= 15f)
            return Directions.Right;
        if (angle > 15f && angle <= 75f)
            return Directions.RightUp;
        if (angle > 75f && angle <= 105f)
            return Directions.Up;
        if (angle > 105f && angle <= 165f)
            return Directions.LeftUp;
        if ((angle > 165f && angle <= 180f) || (angle >= -180f && angle <= -165f))
            return Directions.Left;
        if (angle > -165f && angle <= -105f)
            return Directions.LeftDown;
        if (angle > -105f && angle <= -75f)
            return Directions.Down;
        if (angle > -75f && angle <= -15f)
            return Directions.RightDown;
        return Directions.None;
    }
}
