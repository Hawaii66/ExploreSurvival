using System.Collections.Generic;
using UnityEngine;
using Game.Structures;
using Game.Biomes;

namespace Game
{
    public class Chunk : MonoBehaviour
    {
        public static int ChunkSize = 16;
        public static int ChunkHeight = 100;
        public Coord position;

        public Block[,,] blocks;
        public BiomeType biome;

        public void BootUp()
        {
            biome = BiomeManager.biomeManager.GetBiomeAtChunk(position).type;
            blocks = new Block[ChunkSize, ChunkHeight, ChunkSize];
        }

        public void GenerateTerrain()
        {
            //BiomeSettings biomeSettings = BiomeManager.biomeManager.GetBiome(biome);
            //float scale = biomeSettings.noiseScale;
            //float height = biomeSettings.noiseHeight;

            //float[,,] noise = SimplexNoise.Noise.Calc3D(ChunkSize, ChunkHeight, ChunkSize, scale, position.x * ChunkSize, 0, position.z * ChunkSize);
            //Coord[] biomeOffsets = Utils.CartesianOffsetsZero();
            //biomeOffsets = new Coord[] { new Coord(0, 0, 0) };

            for (int y = 0; y < ChunkHeight; y++)
            {
                for (int z = 0; z < ChunkSize; z++)
                {
                    for (int x = 0; x < ChunkSize; x++)
                    {
                        if(blocks[x,y,z] != null) { continue; }

                        if (y < 60)
                        {
                            blocks[x, y, z] = new Block(BlockType.Stone, true, true);
                        }
                        else if (y == 60)
                        {
                            //float noiseLevel = noise[x, y, z] / 255 * height;
                            Coord[] biomeOffsets = GetSmoothNeightbourChunks(new Coord(x, y, z));
                            float[] weights = new float[biomeOffsets.Length];
                            float totalWeight = 0;
                            float totalHeight = 0;
                            float maxValue = 0;
                            for (int i = 0; i < biomeOffsets.Length; i++)
                            {
                                Coord distanceFromCenter = ((position + biomeOffsets[i]) * ChunkSize + new Coord(ChunkSize / 2, 0, ChunkSize / 2)) - (new Coord(x,y,z) + position * ChunkSize);
                                distanceFromCenter = (new Coord(x, y, z) + position * ChunkSize) - ((position + biomeOffsets[i]) * ChunkSize + new Coord(ChunkSize / 2, 0, ChunkSize / 2));// + new Coord(ChunkSize / 2, 0, ChunkSize / 2));
                                float distance = Mathf.Abs(distanceFromCenter.x * distanceFromCenter.x) + Mathf.Abs(distanceFromCenter.z * distanceFromCenter.z);
                                
                                weights[i] = distance;
                                totalWeight += distance;

                                if(maxValue < distance) { maxValue = distance; }
                            }

                            totalWeight = 0;
                            for(int i = 0; i < biomeOffsets.Length; i ++)
                            {
                                weights[i] = 1 + maxValue - weights[i];
                                totalWeight += weights[i];
                            }

                            string debug = "";

                            for (int i = 0; i < biomeOffsets.Length; i++)
                            {
                                //Coord distanceFromCenter = position + biomeOffsets[i] + new Coord(ChunkSize / 2, 0, ChunkSize / 2);
                                //Chunk c = TerrainManager.terrain.GetChunk(position + biomeOffsets[i]);
                                //if (c == null) { continue; }
                                //BiomeSettings chunkBiome = BiomeManager.biomeManager.GetBiome(c.biome);
                                BiomeSettings chunkBiome = BiomeManager.biomeManager.GetBiomeAtChunk(position + biomeOffsets[i]);

                                if(position == Coord.Zero() && z < 4) { Debug.Log(chunkBiome.type); }

                                float noiseAtPoint = SimplexNoise.Noise.CalcPixel3D(x, y, z, chunkBiome.noiseScale, position.x * ChunkSize, position.z * ChunkSize) / 255 * chunkBiome.noiseHeight;
                                float weighted = noiseAtPoint * ((weights[i] / totalWeight));
                                totalHeight += weighted;

                                if(weights[i] == 0f && new Coord(x,0,z) == new Coord(8, 0, 8)) { Debug.Log("ZER=" + weights[i] + " : " + totalWeight + " : " + weights[i] / totalWeight); }

                                debug += "Point: " + new Coord(x,y,z) + "Noise: " + noiseAtPoint + " : Weigheted: " + weighted + "weight: " + weights[i] + "\n "; 
                            }

                            if (position == Coord.Zero())
                            {
                                debug += "total: " + totalHeight;
                                //Debug.Log(debug);
                            }

                            float noiseLevel = Mathf.Ceil(totalHeight);

                            //if(noiseLevel <= 0) { Debug.Log("Neagitavete " + noiseLevel + " . " + new Coord(x,y,z)); }


                            for (int i = y; i < noiseLevel + y; i++)
                            {
                                //if (i == 60 && position == Coord.Zero()) { Debug.Log(new Coord(x, y, z)); }
                                blocks[x, i, z] = new Block(BlockType.Dirt, true, true);
                            }
                        }
                        else
                        {
                            blocks[x, y, z] = new Block(BlockType.Air, false, false);
                        }
                    }
                }
            }

            SimpleBlock[] blocksInNullChunk = TerrainManager.terrain.GetBlocksInChunk(position);
            for(int i = 0; i < blocksInNullChunk.Length; i ++)
            {
                Coord pos = blocksInNullChunk[i].position;
                blocks[pos.x, pos.y, pos.z] = new Block(BlockManager.blockManager.GetSettings(blocksInNullChunk[i].index));
            }

            Structure s = LoadStrucutres.LoadStructure("Tree,5,8,5,2,0,2,-2,0,-2,4,-1,0,-2,0,0,0,-2,0,1,0,-2,0,2,0,-2,0,-2,0,-1,0,-1,0,-1,0,0,0,-1,0,1,0,-1,0,2,0,-1,0,-2,0,0,0,-1,0,0,0,0,0,0,3,1,0,0,0,2,0,0,0,-2,0,1,0,-1,0,1,0,0,0,1,0,1,0,1,0,2,0,1,0,-2,0,2,0,-1,0,2,0,0,0,2,0,1,0,2,0,2,0,2,0,-2,1,-2,0,-1,1,-2,0,0,1,-2,0,1,1,-2,0,2,1,-2,0,-2,1,-1,0,-1,1,-1,0,0,1,-1,0,1,1,-1,0,2,1,-1,0,-2,1,0,0,-1,1,0,0,0,1,0,3,1,1,0,0,2,1,0,0,-2,1,1,0,-1,1,1,0,0,1,1,0,1,1,1,0,2,1,1,0,-2,1,2,0,-1,1,2,0,0,1,2,0,1,1,2,0,2,1,2,0,-2,2,-2,0,-1,2,-2,0,0,2,-2,0,1,2,-2,0,2,2,-2,0,-2,2,-1,0,-1,2,-1,0,0,2,-1,0,1,2,-1,0,2,2,-1,0,-2,2,0,0,-1,2,0,0,0,2,0,3,1,2,0,0,2,2,0,0,-2,2,1,0,-1,2,1,0,0,2,1,0,1,2,1,0,2,2,1,0,-2,2,2,0,-1,2,2,0,0,2,2,0,1,2,2,0,2,2,2,0,-2,3,-2,4,-1,3,-2,4,0,3,-2,4,1,3,-2,4,2,3,-2,4,-2,3,-1,4,-1,3,-1,4,0,3,-1,4,1,3,-1,4,2,3,-1,4,-2,3,0,4,-1,3,0,4,0,3,0,4,1,3,0,4,2,3,0,4,-2,3,1,4,-1,3,1,4,0,3,1,4,1,3,1,4,2,3,1,4,-2,3,2,4,-1,3,2,4,0,3,2,4,1,3,2,4,2,3,2,4,-2,4,-2,0,-1,4,-2,0,0,4,-2,0,1,4,-2,4,2,4,-2,0,-2,4,-1,0,-1,4,-1,4,0,4,-1,4,1,4,-1,4,2,4,-1,0,-2,4,0,0,-1,4,0,4,0,4,0,4,1,4,0,4,2,4,0,0,-2,4,1,0,-1,4,1,4,0,4,1,4,1,4,1,4,2,4,1,4,-2,4,2,0,-1,4,2,0,0,4,2,0,1,4,2,0,2,4,2,0,-2,5,-2,0,-1,5,-2,0,0,5,-2,0,1,5,-2,0,2,5,-2,0,-2,5,-1,0,-1,5,-1,0,0,5,-1,4,1,5,-1,0,2,5,-1,0,-2,5,0,0,-1,5,0,0,0,5,0,4,1,5,0,4,2,5,0,0,-2,5,1,0,-1,5,1,0,0,5,1,0,1,5,1,0,2,5,1,0,-2,5,2,0,-1,5,2,0,0,5,2,0,1,5,2,0,2,5,2,0,-2,6,-2,0,-1,6,-2,0,0,6,-2,0,1,6,-2,0,2,6,-2,0,-2,6,-1,0,-1,6,-1,0,0,6,-1,4,1,6,-1,0,2,6,-1,0,-2,6,0,0,-1,6,0,0,0,6,0,0,1,6,0,0,2,6,0,0,-2,6,1,0,-1,6,1,0,0,6,1,0,1,6,1,0,2,6,1,0,-2,6,2,0,-1,6,2,0,0,6,2,0,1,6,2,0,2,6,2,0,-2,7,-2,0,-1,7,-2,0,0,7,-2,0,1,7,-2,0,2,7,-2,0,-2,7,-1,0,-1,7,-1,0,0,7,-1,0,1,7,-1,0,2,7,-1,0,-2,7,0,0,-1,7,0,0,0,7,0,0,1,7,0,0,2,7,0,0,-2,7,1,0,-1,7,1,0,0,7,1,0,1,7,1,0,2,7,1,0,-2,7,2,0,-1,7,2,0,0,7,2,0,1,7,2,0,2,7,2,0");
            Coord customOffset = new Coord(-1, 12, -1);
            for(int i = 0; i < s.blocks.Length; i ++)
            {
                Coord offset = s.center + s.blocks[i].position + customOffset;
                Chunk c = Utils.GetChunkFromWorldCoord(offset + position * ChunkSize);
                Coord pos = Utils.GetGridCoordFromWorld(offset);
                if(c == null)
                {
                    Coord newChunkPos = Utils.GetGridCoordFromWorldCoord(offset + position * ChunkSize);
                    TerrainManager.terrain.AddBlockToChunk(newChunkPos, new SimpleBlock(s.blocks[i].index, new Coord(pos.x,pos.y,pos.z)));
                    continue;
                }

                c.blocks[pos.x, pos.y, pos.z] = new Block(BlockManager.blockManager.GetSettings(s.blocks[i].index));

                if(c != this)
                {
                    TerrainManager.terrain.AddChunkToRender(c);
                }
            }


            UpdateMesh();
        }

        private Coord[] GetSmoothNeightbourChunks(Coord c)
        {
            int buffertZone = 4;
            List<Coord> coords = new List<Coord>() { new Coord(0, 0, 0) };
            
            if(c.x < buffertZone) { coords.Add(new Coord(-1, 0, 0)); }
            if(c.x >= ChunkSize - buffertZone) { coords.Add(new Coord(1, 0, 0)); }
            if(c.z < buffertZone) { coords.Add(new Coord(0, 0, -1)); }
            if(c.z >= ChunkSize - buffertZone) { coords.Add(new Coord(0, 0, 1)); }

            if(c.x < buffertZone && c.z < buffertZone) { coords.Add(new Coord(-1, 0, -1)); }
            if(c.x < buffertZone && c.z >= ChunkSize - buffertZone) { coords.Add(new Coord(-1, 0, 1)); }
            if(c.x >= ChunkSize - buffertZone && c.z < buffertZone) { coords.Add(new Coord(1, 0, -1)); }
            if(c.x >= ChunkSize - buffertZone && c.z >= ChunkSize - buffertZone) { coords.Add(new Coord(1, 0, 1)); }

            if (position == Coord.Zero())
            {
                //Debug.Log("Point: " + c + " : Coords: " + coords.Count);
            }

            return coords.ToArray();
        }

        public enum ModifyAction { Add, Delete}

        public void Modify(ModifyAction action, Coord pos, BlockType type)
        {
            if(action == ModifyAction.Add)
            {
                blocks[pos.x, pos.y, pos.z] = new Block(type, true, type == BlockType.Leave ? false : true);
            }
            if(action == ModifyAction.Delete)
            {
                blocks[pos.x, pos.y, pos.z] = new Block(type, false, false);
            }

            UpdateMesh();
        }

        public void UpdateMesh()
        {
            MeshData data = GetMeshData();
            Mesh mesh = new Mesh();
            mesh.vertices = data.verticies.ToArray();
            mesh.triangles = data.triangles.ToArray();
            mesh.uv = data.uvs.ToArray();

            GetComponent<MeshFilter>().sharedMesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        private MeshData GetMeshData()
        {
            MeshData data = new MeshData(new List<Vector3>(), new List<int>(),new List<Color>(), new List<Vector2>());

            int faceCount = 0;
            for (int y = 0; y < ChunkHeight; y ++)
            {
                for(int z = 0; z < ChunkSize; z ++)
                {
                    for(int x = 0; x < ChunkSize; x ++)
                    {
                        if (!blocks[x, y, z].isSolid) { continue; }
                         
                        if(!IsSolidNeighbour(new Coord(x,y,z), Direction.North))
                        {
                            MeshData d = Utils.GetFace(Direction.North, blocks[x,y,z].type);
                            d.verticies = Utils.AddPosOffset(d.verticies, new Coord(x, y, z));
                            d.triangles = Utils.AddFaceOffset(d.triangles, faceCount);
                            data.Add(d);
                            faceCount += 1;
                        }
                        if (!IsSolidNeighbour(new Coord(x, y, z), Direction.South))
                        {
                            MeshData d = Utils.GetFace(Direction.South, blocks[x, y, z].type);
                            d.verticies = Utils.AddPosOffset(d.verticies, new Coord(x, y, z));
                            d.triangles = Utils.AddFaceOffset(d.triangles, faceCount);
                            data.Add(d);
                            faceCount += 1;
                        }
                        if (!IsSolidNeighbour(new Coord(x, y, z), Direction.Front))
                        {
                            MeshData d = Utils.GetFace(Direction.Front, blocks[x, y, z].type);
                            d.verticies = Utils.AddPosOffset(d.verticies, new Coord(x, y, z));
                            d.triangles = Utils.AddFaceOffset(d.triangles, faceCount);
                            data.Add(d);
                            faceCount += 1;
                        }
                        if (!IsSolidNeighbour(new Coord(x, y, z), Direction.Back))
                        {
                            MeshData d = Utils.GetFace(Direction.Back, blocks[x, y, z].type);
                            d.verticies = Utils.AddPosOffset(d.verticies, new Coord(x, y, z));
                            d.triangles = Utils.AddFaceOffset(d.triangles, faceCount);
                            data.Add(d);
                            faceCount += 1;
                        }
                        if (!IsSolidNeighbour(new Coord(x, y, z), Direction.East))
                        {
                            MeshData d = Utils.GetFace(Direction.East, blocks[x, y, z].type);
                            d.verticies = Utils.AddPosOffset(d.verticies, new Coord(x, y, z));
                            d.triangles = Utils.AddFaceOffset(d.triangles, faceCount);
                            data.Add(d);
                            faceCount += 1;
                        }
                        if (!IsSolidNeighbour(new Coord(x, y, z), Direction.West))
                        {
                            MeshData d = Utils.GetFace(Direction.West, blocks[x, y, z].type);
                            d.verticies = Utils.AddPosOffset(d.verticies, new Coord(x, y, z));
                            d.triangles = Utils.AddFaceOffset(d.triangles, faceCount);
                            data.Add(d);
                            faceCount += 1;
                        }
                    }
                }
            }

            return data;
        }

        public bool IsSolidNeighbour(Coord pos, Direction d)
        {
            Coord offset = pos + Utils.GetOffset(d);
            if(offset.x >= 0 && offset.y >= 0 && offset.z >= 0)
            {
                if(offset.x < ChunkSize && offset.y < ChunkHeight && offset.z < ChunkSize)
                {
                    if(blocks[offset.x,offset.y,offset.z] != null)
                    {
                        return blocks[offset.x, offset.y, offset.z].isFullBlock;
                    }
                }
            }

            Chunk offsetChunk = Utils.GetChunkFromWorldCoord(offset + position * ChunkSize);
            Coord offsetGrid = Utils.GetGridCoordFromWorld(offset);

            //Debug.Log(offsetChunk.position + " : " + offsetGrid);

            if(offsetChunk == null || offsetChunk.blocks == null) { return false; }
            if(offsetChunk.blocks[offsetGrid.x, offsetGrid.y, offsetGrid.z] == null) { return false; }
            return offsetChunk.blocks[offsetGrid.x, offsetGrid.y, offsetGrid.z].isFullBlock;
        }

        
    }
}
