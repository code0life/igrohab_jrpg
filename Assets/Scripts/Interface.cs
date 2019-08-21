using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    Game game;
    Director boss;

    public GameObject win_panel;
    public GameObject lose_panel;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        boss = GameObject.Find("Director").GetComponent<Director>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called once per frame
    public void ShowWindow()
    {
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
        win_panel.SetActive(true);
        boss.ShowWinMessage();
    }

    void ShowWindowLose()
    {
        lose_panel.SetActive(true);
        boss.ShowLoseMessage();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

}
