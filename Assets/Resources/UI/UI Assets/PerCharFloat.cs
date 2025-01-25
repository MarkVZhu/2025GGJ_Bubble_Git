using UnityEngine;
using TMPro;

public class PerCharFloat : MonoBehaviour
{
    public TMP_Text tmpText;
    public float amplitude = 5f;
    public float frequency = 2f;

    void Update()
    {
        tmpText.ForceMeshUpdate();
        var textInfo = tmpText.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            Vector3 offset = Vector3.up * Mathf.Sin(Time.time * frequency + i * 0.1f) * amplitude;

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        // Update the mesh with the new vertex data
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            tmpText.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
