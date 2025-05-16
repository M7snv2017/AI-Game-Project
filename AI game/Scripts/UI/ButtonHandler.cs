using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes
using UnityEngine.UI; // For handling button events

public class ButtonHandler : MonoBehaviour
{
    // Reference to buttons (drag and drop in the Inspector)
    public Button playButton;
    public Button exitButton;

    void Start()
    {
        // Ensure buttons are assigned and add listeners for button clicks
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
    }

    // Function to handle play button
    void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // Function to handle exit button
    void ExitGame()
    {
        // Close the application
        Application.Quit();

        // If you're in the Unity Editor, call this to stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
