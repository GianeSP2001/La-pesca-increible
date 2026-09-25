using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Vidas (corazones) y puntos arriba a la derecha (objControlador / Draw GUI).
public class HUDController : MonoBehaviour
{
    [SerializeField] Image[] lifeIcons;     // 3 imágenes con sprVida
    [SerializeField] TMP_Text scoreText;

    void Start()
    {
        GameManager.Instance.OnStatsChanged += Refresh;
        Refresh();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStatsChanged -= Refresh;
    }

    void Refresh()
    {
        GameManager gm = GameManager.Instance;
        for (int i = 0; i < lifeIcons.Length; i++)
            lifeIcons[i].enabled = i < gm.Lives;
        scoreText.text = gm.Score.ToString();
    }
}
