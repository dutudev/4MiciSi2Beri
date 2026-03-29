using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private RectTransform logo;
    [SerializeField] 
    private float logoSpeed;
    [SerializeField] 
    private float logoRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        logo.transform.localPosition = new Vector3(logo.transform.localPosition.x, logoRange * Mathf.Sin(Time.time * logoSpeed), logo.transform.localPosition.z);
    }

    public void StartGame()
    {
        SceneManagerTransition.instance.MoveToScene("Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
