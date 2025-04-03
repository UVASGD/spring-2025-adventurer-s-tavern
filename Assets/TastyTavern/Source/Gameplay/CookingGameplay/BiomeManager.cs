using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour {
    public RoundManager roundManager;
    public void selectBiome(BiomeData biome) {
        roundManager.currentBiome = biome;
    }
}