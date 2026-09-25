using System.Collections;
using TMPro;
using UnityEngine;

/// Efecto de escritura letra por letra (velocidad_texto / texto_visible en GameMaker).
public class TypewriterText : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] float charsPerSecond = 24f;   // cartel de diálogo: 2.5 frames por letra ≈ 24/s

    public bool IsDone { get; private set; } = true;

    Coroutine routine;

    void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();
    }

    public void Play(string text)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Run(text));
    }

    /// Uso: yield return typewriter.PlayAndWait("texto");
    public IEnumerator PlayAndWait(string text)
    {
        Play(text);
        while (!IsDone) yield return null;
    }

    public void Clear()
    {
        if (routine != null) StopCoroutine(routine);
        label.text = "";
        IsDone = true;
    }

    IEnumerator Run(string text)
    {
        IsDone = false;
        label.text = "";
        float delay = 1f / charsPerSecond;
        for (int i = 1; i <= text.Length; i++)
        {
            label.text = text.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
        IsDone = true;
    }
}
