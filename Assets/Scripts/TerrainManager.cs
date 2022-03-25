using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.Structures;

namespace Game
{
    public class TerrainManager : MonoBehaviour
    {
        public static TerrainManager terrain;
        public int renderSize;

        private Dictionary<Coord, Chunk> activeChunks = new Dictionary<Coord, Chunk>();
        private Dictionary<Coord, Chunk> chunks = new Dictionary<Coord, Chunk>();
        private UniqueQueue<Chunk> ChunksToGenerateMesh = new UniqueQueue<Chunk>();
        private UniqueQueue<Chunk> ChunksToUpdateMesh = new UniqueQueue<Chunk>();
        private Dictionary<Coord, SimpleBlock[]> blocksInNullChunks = new Dictionary<Coord, SimpleBlock[]>();

        public PlayerMain[] players;
        public GameObject chunk;

        public float noiseScale;
        public float heightScale;


        private void Awake()
        {
            terrain = this;
        }

        private void Start()
        {
            UpdateChunks();
        }

        private void Update()
        {
            if(ChunksToGenerateMesh.Count > 0)
            {
                Chunk c = ChunksToGenerateMesh.Dequeue();
                c.GenerateTerrain();
            }else if(ChunksToUpdateMesh.Count > 0)
            {
                Chunk c = ChunksToUpdateMesh.Dequeue();
                c.UpdateMesh();
            }
        }

        public Chunk GetChunk(Coord pos)
        {
            chunks.TryGetValue(pos, out Chunk c);
            return c;
        }

        public void AddBlockToChunk(Coord grid, SimpleBlock b)
        {
            if(blocksInNullChunks.TryGetValue(grid, out SimpleBlock[] blocks))
            {
                SimpleBlock[] newBlocks = new SimpleBlock[blocks.Length + 1];
                for(int i = 0; i < blocks.Length; i ++)
                {
                    newBlocks[i] = blocks[i];
                }
                newBlocks[newBlocks.Length - 1] = b;
                blocksInNullChunks.Remove(grid);
                blocksInNullChunks.Add(grid, newBlocks);
            }
            else
            {
                blocksInNullChunks.Add(grid, new SimpleBlock[] { b });
            }
        }

        public SimpleBlock[] GetBlocksInChunk(Coord grid)
        {
            if(blocksInNullChunks.TryGetValue(grid, out SimpleBlock[] blocks))
            {
                return blocks;
            }
            return new SimpleBlock[0];
        }

        public void AddChunkToRender(Chunk c)
        {
            ChunksToUpdateMesh.Enqueue(c);
        }

        public void UpdateChunks()
        {
            activeChunks.Clear();

            for (int i = 0; i < players.Length; i++)
            {
                for (int x = -renderSize; x < renderSize + 1; x++)
                {
                    for (int y = -renderSize; y < renderSize + 1; y++)
                    {
                        if (y < Mathf.Sqrt(renderSize * renderSize))
                        {
                            Coord pos = new Coord(x,0,y);
                            if (players[i].c != null)
                            {
                                pos = new Coord(x + players[i].c.position.x, 0, y + players[i].c.position.z);
                            }
                            
                            if (!activeChunks.ContainsKey(pos))
                            {
                                if (chunks.TryGetValue(pos, out Chunk c))
                                {
                                    activeChunks.Add(pos, c);
                                }
                                else
                                {
                                    Chunk newChunk = CreateChunk(pos);
                                    newChunk.BootUp();
                                    activeChunks.Add(pos, newChunk);
                                    chunks.Add(pos, newChunk);
                                }
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<Coord, Chunk> val in chunks)
            {
                if (activeChunks.ContainsKey(val.Key))
                {
                    val.Value.gameObject.SetActive(true);
                }
                else
                {
                    val.Value.gameObject.SetActive(false);
                }
            }
        }

        private Chunk CreateChunk(Coord pos)
        {
            GameObject temp = Instantiate(chunk, pos * new Coord(Chunk.ChunkSize, 0, Chunk.ChunkSize), Quaternion.identity);
            temp.transform.SetParent(transform);
            temp.name = "Chunk: " + pos.ToString();

            Chunk c = temp.GetComponent<Chunk>();
            c.position = pos;
            ChunksToGenerateMesh.Enqueue(c);

            return c;
        }
    }
}