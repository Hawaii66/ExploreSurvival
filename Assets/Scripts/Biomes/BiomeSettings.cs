using System;
using UnityEngine;

namespace Game.Biomes
{
    [CreateAssetMenu(fileName="Biome",menuName="Custom/Biome")]
    public class BiomeSettings : ScriptableObject
    {
        public new string name;
        public BiomeType type;
        public float noiseScale;
        public float noiseHeight;
    }
}
