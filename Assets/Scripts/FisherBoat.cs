using System.Collections;
using UnityEngine;

/// El pescador en su bote de la cutscene (objPescador): lanza la caña lentamente
/// (0.05 frames por step = 3 fps), mantiene el último frame ~0.7 s y vuelve al primero.
[RequireComponent(typeof(SpriteRenderer))]
public class FisherBoat : MonoBehaviour
{
    [SerializeField] Sprite[] frames;        // los 6 frames de sprPescador en orden
    [SerializeField] float fps = 3f;
    [SerializeField] float holdSeconds = 0.67f;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0) sr.sprite = frames[0];
    }

    /// Uso: yield return boat.Play();
    public IEnumerator Play()
    {
        for (int i = 0; i < frames.Length; i++)
        {
            sr.sprite = frames[i];
            if (i < frames.Length - 1) yield return new WaitForSeconds(1f / fps);
        }
        yield return new WaitForSeconds(holdSeconds);
        sr.sprite = frames[0];
    }
}
