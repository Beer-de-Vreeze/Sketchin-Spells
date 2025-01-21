using UnityEngine;
using TMPro;

public class WobleText : MonoBehaviour
{
    [SerializeField]
    private int m_woblines = 5;
    private TextMeshProUGUI m_text;

    [SerializeField] 
    private string m_stringToWobble = "";
    private int m_startAt;
    private int m_endAt;

    private void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        CheckText();
    }

    private void Update()
    {
        ApplyWaveEffect();
    }

    private void CheckText()
    {
        string mainText = m_text.text;
        if (mainText.Contains(m_stringToWobble))
        {
            m_startAt = mainText.IndexOf(m_stringToWobble);
            m_endAt = m_startAt + m_stringToWobble.Length - 1;
        }
        else
        {
            m_startAt = 0;
            m_endAt = m_stringToWobble.Length - 1;
        }
    }

    private bool InBetween(int checkValue, int start, int end)
    {
        return checkValue >= start && checkValue <= end;
    }

    private void ApplyWaveEffect()
    {
        m_text.ForceMeshUpdate();
        var textInfo = m_text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible || !InBetween(i, m_startAt, m_endAt))
                continue;

            var verts = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++)
            {
                var orig = verts[textInfo.characterInfo[i].vertexIndex + j];
                verts[textInfo.characterInfo[i].vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * m_woblines + orig.x * 0.01f) * 10, 0);
            }
        }

        m_text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}