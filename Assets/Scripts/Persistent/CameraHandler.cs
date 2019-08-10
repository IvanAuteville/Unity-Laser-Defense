using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Set the desired aspect ratio 
        float targetaspect = 9f / 16f;

        // Determine the game window's current aspect ratio
        float windowaspect = Screen.width / (float)Screen.height;

        // Current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // Obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // If scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;

            rect.y = (1.0f - scaleheight) / 2.0f;
            camera.rect = rect;
        }
        else // Add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;

            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}