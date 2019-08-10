using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [Header("OnClick Add Event:")]
    [SerializeField] private bool play = true;
    [SerializeField] private bool mainMenu = false;

    private Button button = null;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (play)
        {
            button.onClick.AddListener(delegate { LevelManager.instance.StartGame(); });
        }
        else if (mainMenu)
        {
            button.onClick.AddListener(delegate { LevelManager.instance.LoadStartMenu(); });
        }
    }
}
