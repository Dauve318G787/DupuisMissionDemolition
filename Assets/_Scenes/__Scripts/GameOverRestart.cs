using UnityEngine;
using UnityEngine.SceneManagement; // Import this for scene management
using UnityEngine.UI; // Import this for button functionality

public class ButtonReturnToScene : MonoBehaviour
{
    public string sceneName = "_Scene_0"; // You can set the scene name in the Unity Inspector or hard-code it

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
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
