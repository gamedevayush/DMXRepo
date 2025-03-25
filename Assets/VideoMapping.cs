using UnityEngine;

public class VideoMapping : MonoBehaviour
{
    public int screenIndex;  // Unique index for each screen
    public int totalScreens = 6;  // Total number of screens
    public MeshRenderer screenRenderer;  // Renderer of the screen

    void Start()
    {
        if (screenRenderer == null || totalScreens <= 0) return;

        // Calculate UV mapping dynamically
        float uvWidth = 1f / totalScreens;  // Split the video evenly
        float uvOffsetX = screenIndex * uvWidth;  // Offset for each screen

        // Use MaterialPropertyBlock to prevent modifying shared material
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        screenRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetVector("_UVOffset", new Vector4(uvOffsetX, 0, uvWidth, 1));
        screenRenderer.SetPropertyBlock(propertyBlock);
    }
}
