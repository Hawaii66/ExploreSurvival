using System.Collections.Generic;
using UnityEngine;

namespace Game.Biomes
{
    public class BiomeManager : MonoBehaviour
    {
        public static BiomeManager biomeManager;
        public BiomeSettings[] biomes;

        private Dictionary<BiomeType, BiomeSettings> loadedSettings = new Dictionary<BiomeType, BiomeSettings>();
        private Dictionary<Coord, BiomeType> loadedBiomes = new Dictionary<Coord, BiomeType>();

        private static readonly int HumidityOffset = 1736;

        private void Awake()
        {
            biomeManager = this;

            for(int i = 0; i < biomes.Length; i ++)
            {
                loadedSettings.Add(biomes[i].type, biomes[i]);
            }
        }

        public BiomeType GetRandomBiome()
        {
            int index = Random.Range(0, biomes.Length);
            return biomes[index].type;
        }

        public BiomeSettings GetBiome(BiomeType type)
        {
            if(loadedSettings.TryGetValue(type, out BiomeSettings b))
            {
                return b;
            }
            return null;
        }

        public BiomeSettings GetBiomeAtChunk(Coord c)
        {
            if(loadedBiomes.TryGetValue(c, out BiomeType b))
            {
                return GetBiome(b);
            }
            
            float temperature = SimplexNoise.Noise.CalcPixel2D(c.x, c.z, 0.10512880927831023f) / 255f;
            float humidity = SimplexNoise.Noise.CalcPixel2D(c.x + HumidityOffset, c.z + HumidityOffset, 0.123830924023742374f) / 255f;

            if(temperature < 0.5f)
            {
                loadedBiomes.Add(c, BiomeType.Grass);
                return GetBiome(BiomeType.Grass);
            }
            if(humidity < 0.5f)
            {
                loadedBiomes.Add(c, BiomeType.Hills);
                return GetBiome(BiomeType.Hills);
            }

            loadedBiomes.Add(c, BiomeType.Stone);
            return GetBiome(BiomeType.Stone);
        }
    }
}