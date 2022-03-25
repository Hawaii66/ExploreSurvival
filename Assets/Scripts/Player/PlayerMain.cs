using System.Collections;
using UnityEngine;

namespace Game.Player
{
    public class PlayerMain : MonoBehaviour
    {
        public bool canMove;

        public float speed;
        public float rotationSpeed;

        public Transform eyes;
        public LayerMask groundMask;
        public Coord point;

        public Chunk c;

        public BlockType[] blocks;
        public int selectedBlock;

        /*private void OnDrawGizmos()
        {
            Vector3 pos = (offsetChunkPos * Chunk.ChunkSize) + new Vector3(Chunk.ChunkSize / 2, Chunk.ChunkSize / 2, Chunk.ChunkSize / 2) - Vector3.one * 0.5f;
            Gizmos.DrawWireCube(pos, Coord.One() * Chunk.ChunkSize);   
        }*/

        private void Update()
        {
            UpdatePoint();

            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = new Ray(eyes.position, eyes.forward);
                if(Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
                {
                    point = new Coord(Mathf.FloorToInt(hit.point.x - hit.normal.x / 2 + 0.5f), Mathf.FloorToInt(hit.point.y - hit.normal.y / 2 + 0.5f), Mathf.FloorToInt(hit.point.z - hit.normal.z / 2 + 0.5f));

                    Chunk currentChunk = Utils.GetChunkFromWorldCoord(point);
                    Coord currentChunkPoint = Utils.GetGridCoordFromWorld(point);
                    currentChunk.Modify(Chunk.ModifyAction.Delete, currentChunkPoint, blocks[selectedBlock]);

                    UpdateNeighbourChunk(currentChunkPoint);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = new Ray(eyes.position, eyes.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 10f, groundMask))
                {
                    point = new Coord(Mathf.FloorToInt(hit.point.x + hit.normal.x / 2 + 0.5f), Mathf.FloorToInt(hit.point.y + hit.normal.y / 2 + 0.5f), Mathf.FloorToInt(hit.point.z + hit.normal.z / 2 + 0.5f));

                    Chunk currentChunk = Utils.GetChunkFromWorldCoord(point);
                    Coord currentChunkPoint = Utils.GetGridCoordFromWorld(point);
                    currentChunk.Modify(Chunk.ModifyAction.Add, currentChunkPoint, blocks[selectedBlock]);

                    UpdateNeighbourChunk(currentChunkPoint);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                selectedBlock += 1;
                if(selectedBlock >= blocks.Length)
                {
                    selectedBlock = 0;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                selectedBlock -= 1;
                if(selectedBlock < 0)
                {
                    selectedBlock = blocks.Length - 1;
                }
            }

            UpdateCurrentChunk();
        }

        private void UpdateNeighbourChunk(Coord c)
        {
            if (Utils.IsCorner(c))
            {
                Chunk neighbourChunk = Utils.GetChunkFromWorldCoord(point +
                    new Coord(
                        0,
                        0,
                        c.z == 0 ? -1 : 1
                    ));
                neighbourChunk.UpdateMesh();
                neighbourChunk = Utils.GetChunkFromWorldCoord(point +
                    new Coord(
                        c.x == 0 ? -1 : 1,
                        0,
                        0
                    ));
                neighbourChunk.UpdateMesh();
            }
            if (Utils.IsBorder(c))
            {
                Chunk neighbourChunk = Utils.GetChunkFromWorldCoord(point +
                    new Coord(
                        c.x == 0 ? -1 : 1,
                        0,
                        c.z == 0 ? -1 : 1
                    ));
                neighbourChunk.UpdateMesh();
            }
        }
        private void UpdatePoint()
        {
            point = new Coord(transform.position);
        }

        private Chunk GetChunk(Vector3 point)
        {
            int chunkPosX = Mathf.FloorToInt(transform.position.x / Chunk.ChunkSize);
            int chunkPosZ = Mathf.FloorToInt(transform.position.z / Chunk.ChunkSize);
             return TerrainManager.terrain.GetChunk(new Coord(chunkPosX, 0, chunkPosZ));
        }

        private void UpdateCurrentChunk()
        {
            Chunk newChunk = GetChunk(transform.position);   
            if(c == null || newChunk.position != c.position)
            {
                c = newChunk;
                TerrainManager.terrain.UpdateChunks();
            }
        }
    }
}