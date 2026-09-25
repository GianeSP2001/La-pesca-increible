using UnityEngine;
using UnityEngine.UI;

public static class SpriteUtil
{
    /// Muestra un sprite en una Image de modo que su ORIGEN (pivote) quede en el centro de su contenedor,
    /// igual que draw_sprite(spr, 0, x, y) en GameMaker. La Image debe tener anclas en el centro (0.5, 0.5).
    public static void ShowAtOrigin(Image image, Sprite sprite)
    {
        image.sprite = sprite;
        image.SetNativeSize();
        image.rectTransform.pivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
        image.rectTransform.anchoredPosition = Vector2.zero;
    }
}
