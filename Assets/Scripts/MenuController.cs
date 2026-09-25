using UnityEngine;
using UnityEngine.UI;

/// Escena "Inicio": el botón JUGAR (objBotonJugar) reinicia el progreso y va a la cutscene.
public class MenuController : MonoBehaviour
{
    [SerializeField] Button playButton;

    void Awake()
    {
        playButton.onClick.AddListener(OnPlay);
    }

    void OnPlay()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("No hay GameManager: crea el GameConfig con el menú Juego > 2. Crear GameConfig.");
            return;
        }
        if (GameManager.Instance.Paused) return;   // durante una transición
        GameManager.Instance.StartNewGame();
    }
}
