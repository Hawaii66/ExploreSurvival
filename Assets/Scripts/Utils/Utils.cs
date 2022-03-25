using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum Direction { North, South, East, West, Front, Back }
    public static class Utils
    {
        public static int TextureAtlasSize = 8;
        public static Coord GetOffset(Direction d)
        {
            if(d == Direction.North) { return new Coord(0, 1, 0); }
            if(d == Direction.South) { return new Coord(0, -1, 0); }
            if(d == Direction.East) { return new Coord(1, 0, 0); }
            if(d == Direction.West) { return new Coord(-1, 0, 0); }
            if(d == Direction.Front) { return new Coord(0, 0, 1); }
            if(d == Direction.Back) { return new Coord(0, 0, -1); }
            return Coord.Zero();
        }

        public static Coord[] CartesianOffsets()
        {
            return new Coord[] { new Coord(1, 0, 0), new Coord(-1, 0, 0), new Coord(0, 0, 1), new Coord(0, 0, -1) };
        }

        public static Coord[] CartesianOffsetsZero()
        {
            return new Coord[] { 
                new Coord(0,0,0), 
                new Coord(1, 0, 0), 
                new Coord(-1, 0, 0), 
                new Coord(0, 0, 1),
                new Coord(0, 0, -1) ,
                new Coord(1, 0, 1) ,
                new Coord(1, 0, -1) ,
                new Coord(-1, 0, 1) ,
                new Coord(-1, 0, -1)

            };
        }

        public static Coord CenterOfVectors(this Coord[] vectors)
        {
            Coord sum = Coord.Zero();
            if (vectors == null || vectors.Length == 0)
            {
                return sum;
            }

            foreach (Coord vec in vectors)
            {
                sum += vec;
            }
            return sum / vectors.Length;
        }

        public static Coord GetGridCoordFromWorldCoord(Coord c)
        {
            return new Coord(Mathf.FloorToInt(c.x / (float)Chunk.ChunkSize), 0, Mathf.FloorToInt(c.z / (float)Chunk.ChunkSize));
        }

        public static Chunk GetChunkFromWorldCoord(Coord c)
        {
            Coord gridCoord = GetGridCoordFromWorldCoord(c);
            return TerrainManager.terrain.GetChunk(gridCoord);
        }

        public static bool IsCorner(Coord c)
        {
            if(c.x == c.z && c.x == 0) { return true; }
            if (c.x == c.z && c.x == Chunk.ChunkSize - 1) { return true; }
            if (c.x == 0 && c.z == Chunk.ChunkSize - 1) { return true; }
            if (c.z == 0 && c.x == Chunk.ChunkSize - 1) { return true; }
            return false;
        }

        public static bool IsBorder(Coord c)
        {
            return (c.x == 0 || c.z == 0 || c.x == Chunk.ChunkSize - 1 || c.z == Chunk.ChunkSize - 1);
        }

        public static Coord GetGridCoordFromWorld(Coord pos)
        {
            int xCoord = pos.x % Chunk.ChunkSize;
            int yCoord = pos.y % Chunk.ChunkHeight;
            int zCoord = pos.z % Chunk.ChunkSize;
            return new Coord(xCoord < 0 ? xCoord + Chunk.ChunkSize : xCoord, yCoord < 0 ? 0 : yCoord, zCoord < 0 ? zCoord + Chunk.ChunkSize : zCoord);
        }

        public static List<int> AddFaceOffset(List<int> tris, int offset)
        {
            for(int i = 0; i < tris.Count; i ++)
            {
                tris[i] += offset * 4;
            }
            return tris;
        }

        public  static List<Vector3> AddPosOffset(List<Vector3> v, Coord offset)
        {
            for (int i = 0; i < v.Count; i++)
            {
                v[i] += offset;
            }
            return v;
        }

        public static MeshData GetFace(Direction d, BlockType t)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Color> colors = new List<Color>();
            List<Vector2> uvs = BlockTypeUVOffsets.GetUVs(t, d);

            if (d == Direction.North)
            {
                verts.Add(new Vector3(0, 1, 0));
                verts.Add(new Vector3(1, 1, 0));
                verts.Add(new Vector3(0, 1, 1));
                verts.Add(new Vector3(1, 1, 1));
                triangles.Add(0);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(3);
                triangles.Add(1);

                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
            }
            if (d == Direction.South)
            {
                verts.Add(new Vector3(0, 0, 0));
                verts.Add(new Vector3(1, 0, 0));
                verts.Add(new Vector3(0, 0, 1));
                verts.Add(new Vector3(1, 0, 1));
                triangles.Add(0);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(3);

                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
            }
            if (d == Direction.Front)
            {
                verts.Add(new Vector3(0, 0, 1));
                verts.Add(new Vector3(1, 0, 1));
                verts.Add(new Vector3(0, 1, 1));
                verts.Add(new Vector3(1, 1, 1));
                triangles.Add(3);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(0);
                triangles.Add(1);

                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
            }
            if (d == Direction.Back)
            {
                verts.Add(new Vector3(0, 0, 0));
                verts.Add(new Vector3(1, 0, 0));
                verts.Add(new Vector3(0, 1, 0));
                verts.Add(new Vector3(1, 1, 0));
                triangles.Add(0);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(3);
                triangles.Add(1);

                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
            }
            if (d == Direction.East)
            {
                verts.Add(new Vector3(1, 0, 0));
                verts.Add(new Vector3(1, 1, 0));
                verts.Add(new Vector3(1, 0, 1));
                verts.Add(new Vector3(1, 1, 1));
                triangles.Add(0);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(3);

                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
            }
            if (d == Direction.West)
            {
                verts.Add(new Vector3(0, 0, 0));
                verts.Add(new Vector3(0, 1, 0));
                verts.Add(new Vector3(0, 0, 1));
                verts.Add(new Vector3(0, 1, 1));
                triangles.Add(0);
                triangles.Add(2);
                triangles.Add(1);
                triangles.Add(2);
                triangles.Add(3);
                triangles.Add(1);

                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
                colors.Add(Color.black);
            }


            for (int i = 0; i < uvs.Count; i++)
            {
                uvs[i] /= (float)TextureAtlasSize;
            }

            for (int i = 0; i < verts.Count; i++)
            {
                verts[i] -= new Vector3(0.5f, 0.5f, 0.5f);
            }

            return new MeshData(verts, triangles, colors, uvs);
        }
    }

    public struct MeshData
    {
        public List<Vector3> verticies;
        public List<int> triangles;
        public List<Color> colors;
        public List<Vector2> uvs;

        public MeshData(List<Vector3> v, List<int> t, List<Color> c, List<Vector2> u)
        {
            verticies = v;
            triangles = t;
            colors = c;
            uvs = u;
        }

        public void Add(MeshData d)
        {
            verticies.AddRange(d.verticies);
            triangles.AddRange(d.triangles);
            colors.AddRange(d.colors);
            uvs.AddRange(d.uvs);
        }
    }
}
