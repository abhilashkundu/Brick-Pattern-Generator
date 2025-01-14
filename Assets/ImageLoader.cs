using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SimpleFileBrowser;

public class ImageLoader : MonoBehaviour
{
    public RawImage previewImage;
    [SerializeField] GameObject generateButton;
    public BrickPatternGenerator brickPatternGenerator;

    public void OpenFileBrowser()
    {
        // Open the file browser to select an image file
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".png", ".jpg", ".jpeg"));
        FileBrowser.SetDefaultFilter(".png");

        // Show the file browser
        FileBrowser.ShowLoadDialog(
            (paths) =>
            {
                if (paths.Length > 0)
                {
                    string path = paths[0];
                    LoadImage(path);
                }
            },
            () => Debug.Log("File selection canceled"),
            FileBrowser.PickMode.Files
        );

        generateButton.SetActive(true);
    }

    private void LoadImage(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist: " + path);
            return;
        }

        // Load the image as a Texture2D
        byte[] imageBytes = File.ReadAllBytes(path);
        Texture2D loadedTexture = new Texture2D(2, 2);
        if (loadedTexture.LoadImage(imageBytes))
        {
            previewImage.texture = loadedTexture; // Set it as the RawImage texture
            previewImage.enabled = true;

            // Pass the loaded texture to the BrickPatternGenerator
            brickPatternGenerator.SetBrickTexture(loadedTexture);
        }
        else
        {
            Debug.LogError("Failed to load image from path: " + path);
        }
    }
}
