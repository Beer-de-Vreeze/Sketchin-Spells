using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using  System.IO;
using TMPro;
using UnityEngine.UIElements;

public class Sketcher : MonoBehaviour
{
    [SerializeField] private Color color = Color.black;
    [SerializeField] private float width = 1.0f;
    [SerializeField] private UnityEngine.UI.Image image;
    private List<LineRenderer> lines = new List<LineRenderer>();
    private LineRenderer currentLine;
    private int numClicks = 0;

    [SerializeField] private RenderTexture renderTexture;

    void Start()
    {
        CreateNewLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SaveImage();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverImage())
            {
                CreateNewLine();
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                numClicks = 0;
                currentLine.positionCount = 0;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (IsMouseOverImage())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                numClicks++;
                currentLine.positionCount = numClicks;
                mousePos.z = 0;
                currentLine.SetPosition(numClicks - 1, mousePos);
            }
        }
    }

private void CreateNewLine()
{
    GameObject lineObject = new GameObject("Line");
    lineObject.transform.parent = this.transform;
    currentLine = lineObject.AddComponent<LineRenderer>();
    currentLine.material = new Material(Shader.Find("Sprites/Default"));
    currentLine.startColor = color;
    currentLine.endColor = color;
    currentLine.startWidth = width;
    currentLine.endWidth = width;
    currentLine.positionCount = 0;
    currentLine.useWorldSpace = true;
    lines.Add(currentLine);
}

    private bool IsMouseOverImage()
    {
        Vector2 localMousePos = image.rectTransform.InverseTransformPoint(Input.mousePosition);
        return image.rectTransform.rect.Contains(localMousePos);
    }

    private void SaveImage()
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        string path = Application.dataPath + "/../" + "sketch.png";
        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved image to: " + path);
    }
}