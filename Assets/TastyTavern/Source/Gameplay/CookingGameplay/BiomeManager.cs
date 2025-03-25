using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour {
    public PlayerManager playerManager;
    public void selectBiome(BiomeData biome) {
        playerManager.currentBiome = biome;
    }
}