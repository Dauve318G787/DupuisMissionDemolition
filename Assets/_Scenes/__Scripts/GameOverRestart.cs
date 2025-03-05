// MODULE PURPOSE:
// This module contains logic for the button on the Game Over screen
// This script is near-identical to ReturnToScene0.cs, save for the
// class name, to avoid conflicts in the global namespace

// Boilerplate Unity includes
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonReturnToScene : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // Find the Button component on this GameObject
        Button button = GetComponent<Button>();

        // Check if the button is found and add the listener to the onClick event
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    // This method will be called when the button is clicked
    void OnButtonClick()
    {
        // Load _Scene_0
        SceneManager.LoadScene("_Scene_0");
    }
}
