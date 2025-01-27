using TMPro;
using UnityEngine;

public class WobleText : MonoBehaviour
{
    [SerializeField]
    private int _woblines = 5;
    private TextMeshProUGUI _text;

    [SerializeField]
    private string _stringToWobble = "";
    private int _startAt;
    private int _endAt;


    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        CheckText();
    }

    private void Update()
    {
        ApplyWaveEffect();
    }

    private void CheckText()
    {
        string mainText = _text.text;
        if (mainText.Contains(_stringToWobble))
        {
            _startAt = mainText.IndexOf(_stringToWobble);
            _endAt = _startAt + _stringToWobble.Length - 1;
        }
        else
        {
            _startAt = 0;
            _endAt = _stringToWobble.Length - 1;
        }
    }

    private bool InBetween(int checkValue, int start, int end)
    {
        return checkValue >= start && checkValue <= end;
    }

    private void ApplyWaveEffect()
    {
        _text.ForceMeshUpdate();
        var textInfo = _text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible || !InBetween(i, _startAt, _endAt))
                continue;

            var verts = textInfo
                .meshInfo[textInfo.characterInfo[i].materialReferenceIndex]
                .vertices;
            for (int j = 0; j < 4; j++)
            {
                var orig = verts[textInfo.characterInfo[i].vertexIndex + j];
                verts[textInfo.characterInfo[i].vertexIndex + j] =
                    orig
                    + new Vector3(0, Mathf.Sin(Time.time * _woblines + orig.x * 0.01f) * 10, 0);
            }
        }

        _text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
