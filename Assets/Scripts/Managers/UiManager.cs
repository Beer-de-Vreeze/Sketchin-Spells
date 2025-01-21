using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    [SerializeField]
    private GameObject sketchCanvas;
    void Start()
    {
        sketchCanvas.GetComponentInChildren<Sketcher>().OnImageSaved += (path) =>
        {
            sketchCanvas.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Handle UI updates here
    }

    public void OpenCloseSketchCanvas(SketchType sketchType, string name)
    {
        Sketcher.Instance.SetSketcher(sketchType, name);
        sketchCanvas.SetActive(true);
    }
}
