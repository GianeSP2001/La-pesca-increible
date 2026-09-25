using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// Pez de respuesta (objPezRespuesta). Requiere Collider2D, un EventSystem en la escena
/// y un Physics2DRaycaster en la cámara para recibir el clic o toque.
[RequireComponent(typeof(SpriteRenderer))]
public class AnswerFish : MonoBehaviour, IPointerClickHandler
{
    public int optionIndex;              // 0, 1 o 2 (lo asigna LevelController según el orden del arreglo)
    public bool Locked { get; set; }     // panel_creado: mientras está abierto el panel no se puede volver a tocar
    public Action<AnswerFish> Clicked;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Locked) Clicked?.Invoke(this);
    }

    public void SetVisible(bool visible)
    {
        sr.enabled = visible;
    }
}
