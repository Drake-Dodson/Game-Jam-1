using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultySettings
{
    [Header("General")]
    public string difficultyName;
    public int value;

    [Header("Gameplay")]

    public int twoSpacesUntilLevel = 1;
    public int oneSpaceUntilLevel = 3;

    [Space]

    public int spawnBombsAfterLevel = 3;
    [Range(0f, 1f)] public float spawnBombChance = 0.1f;

    [Space]

    public int spawnAddArrowsAfterLevel = 4;
    [Range(0f, 1f)] public float minAddArrowsChance = 0.25f;
    [Range(0f, 1f)] public float maxAddArrowsChance = 0.5f;
    [Min(0.01f)] public float addArrowsIncrementPerLevel = 0.05f;

    [Space]
    [Min(1f)] public float speedMultiplier = 1f;
    [Range(0.01f, 1f)] public float arrowHitboxSize = 1f;

    [Space]
    public int totalBeatsForGame = 200;
    public int levelUpAfterLevels = 10;
}