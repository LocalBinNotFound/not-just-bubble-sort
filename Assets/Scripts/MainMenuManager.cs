using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject infoDialog; // Assign this in the inspector within Info Dialog Panel
    public Animation anim;
    void Start()
    {
        // Ensure the dialog is initially hidden
        infoDialog.SetActive(false);
        anim = FindObjectOfType<Animation>();
    }
    // Toggle function for the dialog
    public void ToggleInfoDialog()
    {
        if (infoDialog != null)
            anim.Play("Window-In");
            infoDialog.SetActive(!infoDialog.activeSelf); // Toggle the active state
    }
}