using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] 
    private GameObject counter;
    [SerializeField] 
    private List<FoodAssembler> foodAssemblers1 = new List<FoodAssembler>();
    [SerializeField] 
    private List<FoodAssembler> foodAssemblers2 = new List<FoodAssembler>();
    
    private int _currentState = 0;
    private bool _animatingCounter = false;
    private bool _shownCounter = false;

    private int _idMoveX;
    private int _idMoveY;
    

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowCounter(!_shownCounter);
        }
        
        counter.transform.position = new Vector3(transform.position.x, 10.14f, 0);
    }

    void ChangeState(int state)
    {
        if (_shownCounter || _animatingCounter)
        {
            return;
        }
        
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

        if (_currentState == 0)
        {
            foreach (var asm in foodAssemblers2)
            {
                asm.MoveObjects();
            }
        }
        else
        {
            foreach (var asm in foodAssemblers1)
            {
                asm.MoveObjects();
            }
        }
        
        LeanTween.cancel(_idMoveX);
        _idMoveX = LeanTween.moveLocalX(gameObject, 18 * _currentState, 0.5f).setEaseOutExpo().id;

    }

    void ShowCounter(bool show)
    {
        GameManager.gameManager.ShowCounterCanvas(show);
        _shownCounter = !_shownCounter;
        _animatingCounter = true;
        LeanTween.cancel(_idMoveY);
        _idMoveY = LeanTween.moveLocalY(gameObject, 10.16f * (show?1:0), 0.5f).setEaseOutExpo().setOnComplete(() =>
        {
            _animatingCounter = false;
        }).id;
    }

    public bool GetCounterState()
    {
        return _shownCounter;
    }

    public void ResetCamera()
    {
        _shownCounter = false;
        _currentState = 0;
        gameObject.transform.position = new Vector3(0, 0, -10);
    }

}
