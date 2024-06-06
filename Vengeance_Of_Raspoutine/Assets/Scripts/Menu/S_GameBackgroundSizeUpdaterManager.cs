using UnityEngine;

public class S_GameBackgroundSizeUpdaterManager : MonoBehaviour
{
    public static S_GameBackgroundSizeUpdaterManager Instance;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        AdjustSpriteSizeToScreen();
    }

    /// <summary> Will change the size of the GameBackground GameObject to fit the screen size </summary>
    public void AdjustSpriteSizeToScreen()
    {
        // Get the size of the sprite
        Sprite sprite = _spriteRenderer.sprite;
        if (sprite == null)
        {
            Debug.LogError("SpriteRenderer does not have a sprite assigned.");
            return;
        }

        // Get the world size of the sprite
        Vector2 spriteSize = sprite.bounds.size;

        // Get the size of the screen in world units
        Camera mainCamera = Camera.main;
        float screenHeight = mainCamera.orthographicSize * 2.0f; // The "* 2.0f is here because it's half the height"
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // Calculate the scale required to fit the sprite to the screen
        Vector2 scale = transform.localScale;
        scale.x = screenWidth / spriteSize.x;
        scale.y = screenHeight / spriteSize.y;

        // Apply the scale to the transform
        transform.localScale = scale;
    }

    // TODO : Remove the update, the function should be called only at start, and in settings (when the resolution is changed)
    void Update()
    {
        AdjustSpriteSizeToScreen();
    }
}