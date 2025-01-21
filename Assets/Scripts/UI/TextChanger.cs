using UnityEngine;
using TMPro;
using System.Collections;

public class TextChanger : MonoBehaviour
{
    //bend the text
    private int curve = 5;
    private TextMeshProUGUI text;

    [SerializeField] private Gradient textGradient;
    [SerializeField] private float gradientSpeed = .1f;
    private float _totalTime = 0f;
    private int currentTextSize;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        currentTextSize = text.text.Length;
    }

    private void Update()
    {
        ApplyWaveEffect();
    }

    private void ApplyWaveEffect()
    {
        text.ForceMeshUpdate();
        var textInfo = text.textInfo;
        var vertices = textInfo.meshInfo[0].vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            var offset = Mathf.Sin(Time.time + i * 0.1f) * curve;
            vertices[i].y += offset;
        }

        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
