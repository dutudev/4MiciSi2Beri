using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private int _currentState = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeState(_currentState + 1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeState(_currentState - 1);
        }
    }

    void ChangeState(int state)
    {
        if (state < 0)
        {
            _currentState = 0;
            return;
        }
        
        if (state > 1)
        {
            _currentState = 1;
            return;
        }

        _currentState = state;
        
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalX(gameObject, 18 * _currentState, 0.5f).setEaseOutExpo();

    }
}
