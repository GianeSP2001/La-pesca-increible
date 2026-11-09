# La Pesca Increíble 🎣

Juego serio educativo que entrena la atención en niños con TDAH mediante una mecánica simple y repetible: sesiones cortas y retroalimentación inmediata.
Nació como mi proyecto de tesis (artículo en la revista EIRCON, actualmente en revisión); este repositorio es la reescritura completa del juego original de GameMaker a **Unity/C#**, con una arquitectura orientada a componentes.

🔗 **Jugar en el navegador:** (https://gp-stars01-studio.itch.io/la-pesca-increble)

## Sobre el juego

El niño avanza por 3 niveles pescando peces que contienen preguntas y luego pescando la respuesta correcta entre varias opciones. El diseño busca mantener la atención mediante retroalimentación inmediata, sesiones cortas y una mecánica simple y repetible.

- **Mecánica:** elegir una pregunta → responder pescando la opción correcta
- **Puntaje:** 3 puntos al acertar en el primer intento, 1 punto en el segundo
- **Progresión:** 3 vidas; al perderlas todas, el juego reinicia desde el nivel 1

## Tecnologías

- **Motor:** Unity 2023 (2D, URP)
- **Lenguaje:** C#
- **UI:** TextMeshPro
- **Plataformas:** WebGL (jugable en itch.io), Android

## Capturas
<img width="450" height="296" alt="image" src="https://github.com/user-attachments/assets/51534347-b84d-4520-84ce-3f431426848e" />

<img width="394" height="252" alt="image" src="https://github.com/user-attachments/assets/451b30c4-2d6c-46bb-af5e-afed98bd0de8" />

<img width="394" height="254" alt="image" src="https://github.com/user-attachments/assets/3b88864e-fe84-4ad8-b0b3-d27b68dd26db" />


## Arquitectura

| Script | Responsabilidad |
|---|---|
| `GameManager` | Estado global: nivel, vidas, puntaje, transición entre escenas |
| `GameConfig` / `LevelData` | Datos de niveles y preguntas (ScriptableObject) |
| `CutsceneController` | Diálogo del pescador, reglas y presentación de la pregunta |
| `LevelController` | Lógica de cada nivel: selección de pez, evaluación de respuesta |
| `SwimmingFish` / `AnswerFish` | Movimiento y detección de toque/clic en los peces |
| `HUDController` | Vidas y puntaje en pantalla |

## Origen del proyecto

Este juego nació como parte de mi tesis de titulación y fue documentado en un artículo enviado a la revista EIRCON. La versión original fue desarrollada en GameMaker; este repositorio contiene la reescritura completa en Unity/C#, incluyendo una nueva arquitectura de código orientada a componentes.

## Autora

**Gianella Sandoval Palomino** · Ingeniera de Software | Desarrolladora de videojuegos
[LinkedIn](https://www.linkedin.com/in/gianella-sandoval-palomino-3965a61b5/) · [itch.io](https://gp-stars01-studio.itch.io)
