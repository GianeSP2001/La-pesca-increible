using UnityEngine;

/// Versión simple del efecto de agua (objAgua): la imagen del agua se mece suavemente.
/// El original deformaba el sprite línea por línea; si lo quieres idéntico se puede hacer con un shader.
public class WaterSway : MonoBehaviour
{
    [SerializeField] float amplitudeX = 0.03f;
    [SerializeField] float amplitudeY = 0.04f;
    [SerializeField] float speed = 1.5f;

    Vector3 basePos;

    void Start()
    {
        basePos = transform.position;
    }

    void Update()
    {
        float t = Time.time * speed;
        transform.position = basePos + new Vector3(
            Mathf.Cos(t * 0.3f) * amplitudeX + Mathf.Sin(t) * amplitudeX,
            Mathf.Sin(t * 0.8f) * amplitudeY,
            0f);
    }
}
