using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        Time.timeScale = 1;

        if(scene.name == "MainMenu")
        {
            AudioManager.instance.bgMusic.Stop();
            AudioManager.instance.PlayAudio(AudioManager.instance.mainMenu);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void ShowSettings()
    {
        anim.SetBool("showSettings", true);
    }

    public void HideSettings()
    {
        anim.SetBool("showSettings", false);
    }
}
