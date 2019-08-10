using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.1f;
    private Material background = null;
    private Vector2 offset;

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

            background = GetComponent<Renderer>().material;
            offset = new Vector2(0f, scrollSpeed);
        }
    }

    void Update()
    {
        background.mainTextureOffset += offset * Time.deltaTime;
    }
}