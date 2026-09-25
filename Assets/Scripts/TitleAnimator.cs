using System.Text;
using TMPro;
using UnityEngine;

/// Título del menú (objPortadaTitulo): flota suavemente y una letra brilla en secuencia.
public class TitleAnimator : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] string title = "LA PESCA INCREIBLE";
    [SerializeField] float bobAmplitude = 5f;      // en píxeles de UI
    [SerializeField] float bobSpeed = 1.8f;
    [SerializeField] float letterInterval = 0.1f;  // letra_delay 6 steps

    RectTransform rect;
    Vector2 startPos;
    int active;
    float timer;

    void Awake()
    {
        rect = (RectTransform)transform;
        startPos = rect.anchoredPosition;
    }

    void Start()
    {
        Rebuild();
    }

    void Update()
    {
        rect.anchoredPosition = startPos + Vector2.up * (Mathf.Sin(Time.time * bobSpeed) * bobAmplitude);

        timer += Time.deltaTime;
        if (timer >= letterInterval)
        {
            timer = 0f;
            active = (active + 1) % title.Length;
            Rebuild();
        }
    }

    void Rebuild()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < title.Length; i++)
        {
            if (i == active && title[i] != ' ')
                sb.Append("<color=#FFF0B4>").Append(title[i]).Append("</color>");
            else
                sb.Append(title[i]);
        }
        label.text = sb.ToString();
    }
}
