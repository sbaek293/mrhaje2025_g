using UnityEngine;

public class PauseScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool paused = false;
    public GameObject pauseMenu;
    public Look lookScript;
    void Start()
    {
        paused = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
    }

    public void togglePause()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        lookScript.UpdateCursorLock();
    }

}
