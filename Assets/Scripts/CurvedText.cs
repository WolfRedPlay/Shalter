using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurvedText : MonoBehaviour
{
    public AnimationCurve curve = new AnimationCurve(); // кривая для определения искажения
    private TMP_Text textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
        textMeshPro.ForceMeshUpdate(); // Обновляем меш текста
    }

    private void OnEnable()
    {
        WarpText();
    }

    void WarpText()
    {
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        Vector3[] vertices;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                Vector3 original = vertices[vertexIndex + j];
                float offset = curve.Evaluate(original.x / textMeshPro.bounds.size.x) * 10; // Применяем кривую деформации
                vertices[vertexIndex + j] = new Vector3(original.x, original.y + offset, original.z);
            }
        }

        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
