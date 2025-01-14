using UnityEngine;
using UnityEngine.UI;

public class BrickPatternGenerator : MonoBehaviour
{
    public GameObject brickPrefab;
    public RectTransform container;
    public int brickHeight = 50;
    public int spacing = 2;
    [SerializeField] private Texture2D brickTexture;
    public int rows = 5;
    public int columns = 5;

    public void SetBrickTexture(Texture2D texture)
    {
        if (texture != null)
        {
            brickTexture = texture; // Assign the loaded texture
        }
        else
        {
            Debug.LogError("Attempted to set a null texture.");
        }
    }

    public void GenerateBricks()
    {
        ClearBricks();
        //Generate Bricks
        float brickWidth = (brickTexture.width / columns);
        float totalWidth = (brickWidth + spacing) * columns - spacing;

        float xOffset = 0;
        float yOffset = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                CreateBrick(xOffset, yOffset, brickWidth, row, col);
                xOffset += brickWidth + spacing;
            }
            xOffset = 0;
            yOffset += brickHeight + spacing;
        }
    }

    void CreateBrick(float x, float y, float width, int row, int col)
    {
        GameObject brick = Instantiate(brickPrefab, container);
        RectTransform rect = brick.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, brickHeight);
        rect.anchoredPosition = new Vector2(x, -y); // Adjust y for UI coordinates

        var rawImage = brick.GetComponent<RawImage>();
        if (rawImage != null && brickTexture != null)
        {
            rawImage.texture = brickTexture; // Set the texture
            SetBrickUV(rawImage, row, col); // Set the UV mapping for this brick
        }
        else
        {
            Debug.LogError("RawImage component is missing or brickTexture is null.");
        }
    }

    void SetBrickUV(RawImage rawImage, int row, int col)
    {
        // Calculate the UV coordinates for the specified row and column
        float uvXStart = (float)col / columns;
        float uvYStart = 1f - (float)(row + 1) / rows;
        float uvWidth = 1f / columns;
        float uvHeight = 1f / rows;

        // Create a new Rect for the UVs
        Rect uvRect = new Rect(uvXStart, uvYStart, uvWidth, uvHeight);
        rawImage.uvRect = uvRect;
    }

    void ClearBricks()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}
