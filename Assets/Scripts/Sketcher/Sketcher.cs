using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum SketchType
{
    Enemy,
    Spell,
    Player,
    DarkLord,
    Test,
}

public class Sketcher : Singleton<Sketcher>
{
    [SerializeField]
    private Color _color = Color.black;

    [SerializeField]
    private Color _eraserColor = Color.red;

    [SerializeField]
    private float _width = 1.0f;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private RenderTexture RenderTexture;

    [SerializeField]
    private string _sketchName = "DefaultSketch";

    [SerializeField]
    private TextMeshProUGUI _sketchTitle;

    [SerializeField]
    private TextMeshProUGUI _sketchDescription;

    [SerializeField]
    private SketchType _sketchType = SketchType.Spell;

    private List<LineRenderer> _lines = new List<LineRenderer>();
    private LineRenderer _currentLine;
    private int _numClicks = 0;
    private bool _isErasing = false;
    private Stack<LineRenderer> _undoStack = new Stack<LineRenderer>();
    private bool _isSymmetryEnabled = false;
    private bool _isFilling = false;
    public UnityEvent OnImageSaved;

    #region Unity
    void Update()
    {
        HandleInput();
    }

    private void OnDisable()
    {
        OnImageSaved.RemoveAllListeners();
        foreach (var line in _lines)
        {
            Destroy(line.gameObject);
        }
        _lines.Clear();
        foreach (var line in _undoStack)
        {
            Destroy(line.gameObject);
        }
        _undoStack.Clear();
    }
    #endregion

    #region Input Handling
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SaveImage();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleSymmetry();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoLastLine();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            EraseAllLines();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleEraser();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverImage())
            {
                if (_isErasing)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0; // Ensure the eraser stays on the 2D plane

                    EraseAtPosition(mousePos);
                    DrawEraserFeedback(mousePos);
                }
                else
                {
                    StartNewLine();
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (IsMouseOverImage())
            {
                if (_isErasing)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0; // Ensure the eraser stays on the 2D plane

                    EraseAtPosition(mousePos);
                    DrawEraserFeedback(mousePos);
                }
                else
                {
                    if (_currentLine != null)
                    {
                        DrawLine();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!_isErasing && !_isFilling)
            {
                StopLineDraw();
            }
        }
    }
    #endregion

    #region Drawing
    private void CreateNewLine()
    {
        GameObject lineObject = new GameObject("Line");
        lineObject.transform.parent = this.transform;
        _currentLine = lineObject.AddComponent<LineRenderer>();
        _currentLine.material = new Material(Shader.Find("Sprites/Default"));
        _currentLine.startColor = _color;
        _currentLine.endColor = _color;
        _currentLine.startWidth = _width;
        _currentLine.endWidth = _width;
        _currentLine.positionCount = 0;
        _currentLine.useWorldSpace = true;
        _lines.Add(_currentLine);
        _undoStack.Push(_currentLine);

        if (_isSymmetryEnabled)
        {
            CreateMirroredLine();
        }
    }

    private void StartNewLine()
    {
        CreateNewLine();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _numClicks = 0;
        _currentLine.positionCount = 0;
    }

    private void StopLineDraw()
    {
        _currentLine = null;
    }

    private void DrawLine()
    {
        if (_currentLine == null)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _numClicks++;
        _currentLine.positionCount = _numClicks;
        mousePos.z = 0;
        _currentLine.SetPosition(_numClicks - 1, mousePos);

        if (_isSymmetryEnabled)
        {
            Vector3 mirroredPos = new Vector3(-mousePos.x, mousePos.y, mousePos.z);
            LineRenderer mirroredLine = _lines[_lines.Count - 1];
            mirroredLine.positionCount = _numClicks;
            mirroredLine.SetPosition(_numClicks - 1, mirroredPos);
        }

        EraseOutsideCanvas();
    }
    #endregion

    #region Erasing
    public void ToggleEraser()
    {
        _isErasing = !_isErasing;
    }

    private void EraseAtPosition(Vector3 position)
    {
        foreach (var line in _lines)
        {
            for (int i = 0; i < line.positionCount; i++)
            {
                // Check if the point is within the eraser's radius
                if (Vector3.Distance(line.GetPosition(i), position) < _width)
                {
                    // Store the erased line for undo
                    LineRenderer erasedLine = line;
                    _undoStack.Push(erasedLine);

                    // Remove the point by setting it to an invalid position (could also fade it, etc.)
                    line.SetPosition(
                        i,
                        new Vector3(float.MaxValue, float.MaxValue, float.MaxValue)
                    );
                }
            }
        }
    }

    private void EraseOutsideCanvas()
    {
        RectTransform rectTransform = _image.rectTransform;
        for (int i = 0; i < _currentLine.positionCount; i++)
        {
            Vector3 linePos = _currentLine.GetPosition(i);
            Vector2 localPos = rectTransform.InverseTransformPoint(
                Camera.main.WorldToScreenPoint(linePos)
            );
            if (!rectTransform.rect.Contains(localPos))
            {
                _currentLine.SetPosition(
                    i,
                    new Vector3(float.MaxValue, float.MaxValue, float.MaxValue)
                );
            }
        }
    }

    public void EraseAllLines()
    {
        foreach (var line in _lines)
        {
            Destroy(line.gameObject);
        }
        _lines.Clear();
        _undoStack.Clear();
    }

    private void DrawEraserFeedback(Vector3 position)
    {
        Gizmos.color = _eraserColor;
        Gizmos.DrawWireSphere(position, _width); // Visual feedback for the eraser region
    }
    #endregion

    #region Symmetry
    private void ApplySymmetry()
    {
        if (_currentLine == null)
            return;

        LineRenderer mirroredLine = _lines[_lines.Count - 1];
        for (int i = 0; i < _currentLine.positionCount; i++)
        {
            Vector3 originalPos = _currentLine.GetPosition(i);
            Vector3 mirroredPos = new Vector3(-originalPos.x, originalPos.y, originalPos.z);
            mirroredLine.SetPosition(i, mirroredPos);
        }
    }

    private LineRenderer CreateMirroredLine()
    {
        GameObject lineObject = new GameObject("MirroredLine");
        lineObject.transform.parent = this.transform;
        LineRenderer mirroredLine = lineObject.AddComponent<LineRenderer>();
        mirroredLine.material = new Material(Shader.Find("Sprites/Default"));
        mirroredLine.startColor = _color;
        mirroredLine.endColor = _color;
        mirroredLine.startWidth = _width;
        mirroredLine.endWidth = _width;
        mirroredLine.positionCount = _currentLine.positionCount;
        mirroredLine.useWorldSpace = true;
        _lines.Add(mirroredLine);
        _undoStack.Push(mirroredLine);
        return mirroredLine;
    }

    private void ToggleSymmetry()
    {
        _isSymmetryEnabled = !_isSymmetryEnabled;
        if (_isSymmetryEnabled)
        {
            ApplySymmetry();
        }
    }
    #endregion

    #region Filling
    // // Flood fill algorithm to fill an enclosed area (inside the drawn shape)
    // private void FloodFill(Vector2 start, Color fillColor)
    // {
    //     // Get the pixel grid based on the render texture or image
    //     Texture2D tex = new Texture2D(
    //         _renderTexture.width,
    //         _renderTexture.height,
    //         TextureFormat.RGBA32,
    //         false
    //     );
    //     RenderTexture.active = _renderTexture;
    //     tex.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
    //     tex.Apply();
    //     RenderTexture.active = null;

    //     // Convert the start position to texture coordinates
    //     Vector2Int startPixel = new Vector2Int((int)start.x, (int)start.y);

    //     // Get the original color at the start point
    //     Color originalColor = tex.GetPixel(startPixel.x, startPixel.y);

    //     // If the starting point is already the fill color, do nothing
    //     if (originalColor == fillColor)
    //         return;

    //     // Use a queue to implement BFS
    //     Queue<Vector2Int> pixelsToFill = new Queue<Vector2Int>();
    //     pixelsToFill.Enqueue(startPixel);

    //     // List of possible neighbor directions (up, down, left, right)
    //     Vector2Int[] directions =
    //     {
    //         new Vector2Int(0, 1),
    //         new Vector2Int(0, -1),
    //         new Vector2Int(1, 0),
    //         new Vector2Int(-1, 0),
    //     };

    //     // Start BFS
    //     while (pixelsToFill.Count > 0)
    //     {
    //         Vector2Int current = pixelsToFill.Dequeue();

    //         // Check if current pixel is out of bounds or already filled
    //         if (current.x < 0 || current.x >= tex.width || current.y < 0 || current.y >= tex.height)
    //             continue;
    //         if (tex.GetPixel(current.x, current.y) != originalColor)
    //             continue;

    //         // Fill the current pixel with the desired color
    //         tex.SetPixel(current.x, current.y, fillColor);

    //         // Add neighboring pixels to the queue
    //         foreach (var direction in directions)
    //         {
    //             Vector2Int neighbor = current + direction;
    //             pixelsToFill.Enqueue(neighbor);
    //         }
    //     }

    //     // Apply the changes to the texture
    //     tex.Apply();

    //     // Update the render texture with the filled texture
    //     RenderTexture.active = _renderTexture;
    //     Graphics.Blit(tex, _renderTexture);
    //     RenderTexture.active = null;
    // }

    // private void ToggleFill()
    // {
    //     _isFilling = !_isFilling;
    // }
    #endregion

    #region Utility
    private bool IsMouseOverImage()
    {
        Vector2 localMousePos = _image.rectTransform.InverseTransformPoint(Input.mousePosition);
        return _image.rectTransform.rect.Contains(localMousePos);
    }

    public void SetColor(Color color)
    {
        this._color = color;
    }

    public void SetWidth(float newWidth)
    {
        _width = newWidth;
        if (_currentLine != null)
        {
            _currentLine.startWidth = _width;
            _currentLine.endWidth = _width;
        }
    }

    public void UndoLastLine()
    {
        if (_undoStack.Count > 0)
        {
            LineRenderer lastLine = _undoStack.Pop();
            Destroy(lastLine.gameObject);
            _lines.Remove(lastLine);
        }
    }

    public void SaveImage()
    {
        RenderTexture tempRT = RenderTexture.GetTemporary(
            RenderTexture.width,
            RenderTexture.height,
            0,
            RenderTextureFormat.ARGB32
        );
        tempRT.antiAliasing = 8;

        Graphics.Blit(RenderTexture, tempRT);

        Texture2D tex = new Texture2D(tempRT.width, tempRT.height, TextureFormat.RGBA32, false);
        RenderTexture.active = tempRT;
        tex.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        Color[] pixels = tex.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] == Color.white)
            {
                pixels[i] = new Color(0, 0, 0, 0);
            }
        }
        tex.SetPixels(pixels);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        string folderPath = Path.Combine(
            Application.persistentDataPath,
            "sketches",
            _sketchType.ToString()
        );
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string path = Path.Combine(folderPath, _sketchName + ".png");

        int fileIndex = 1;
        while (File.Exists(path))
        {
            path = Path.Combine(folderPath, _sketchName + "_" + fileIndex + ".png");
            fileIndex++;
        }

        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved image to: " + path);

        OnImageSaved?.Invoke();

        RenderTexture.ReleaseTemporary(tempRT);

        UIManager.Instance.CloseSketchCanvas();
    }
    #endregion

    #region UI

    public void SetSketcher(SketchType type, string name, string description)
    {
        _sketchType = type;
        _sketchName = name;
        _sketchTitle.text = name;
        _sketchDescription.text = description;
    }

    #endregion
}
