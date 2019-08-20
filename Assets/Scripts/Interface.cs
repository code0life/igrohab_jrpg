using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    Game game;

    public GameObject win_panel;
    public GameObject lose_panel;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called once per frame
    public void ShowWindow()
    {
        Debug.Log("ShowWindow");
        if (game.evil_units.Count == 0)
        {
            ShowWindowWin();
        }
        else if(game.good_units.Count == 0)
        {
            ShowWindowLose();
        }
    }

    void ShowWindowWin()
    {
        Debug.Log("ShowWindowWin");
        win_panel.SetActive(true);
    }

    void ShowWindowLose()
    {
        Debug.Log("ShowWindowLose");
        lose_panel.SetActive(true);
    }

}
