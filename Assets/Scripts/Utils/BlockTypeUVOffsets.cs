using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class BlockTypeUVOffsets
    {
        public static List<Vector2> GetUVs(BlockType t, Direction d)
        {
            Coord[] offset = BlockManager.blockManager.GetSettings(t).TextureOffsets;

            if (d == Direction.North) { return NormalUp(offset[0]); }
            if (d == Direction.South) { return NormalDown(offset[1]); }
            if (d == Direction.East) { return NormalEast(offset[2]); }
            if (d == Direction.West) { return NormalWest(offset[3]); }
            if (d == Direction.Front) { return NormalFront(offset[4]); }
            if (d == Direction.Back) { return NormalBack(offset[5]); }

            return new List<Vector2>() { new Vector2(0, 0),
                new Vector2(1 ,0),
                new Vector2(0, 1),
                new Vector2(1, 1) };
        }

        public static List<Vector2> NormalUp(Coord offset)
        {
            return new List<Vector2>
            {
                new Vector2(0 + offset.x + 0.01f, 0 + offset.y + 0.01f),
                new Vector2(1 + offset.x - 0.01f ,0 + offset.y + 0.01f),
                new Vector2(0 + offset.x + 0.01f, 1 + offset.y - 0.01f),
                new Vector2(1 + offset.x - 0.01f, 1 + offset.y - 0.01f)
            };
        }

        public static List<Vector2> NormalDown(Coord offset)
        {
            return new List<Vector2>
            {
                new Vector2(0 + offset.x + 0.01f, 0 + offset.y + 0.01f),
                new Vector2(1 + offset.x - 0.01f ,0 + offset.y + 0.01f),
                new Vector2(0 + offset.x + 0.01f, 1 + offset.y - 0.01f),
                new Vector2(1 + offset.x - 0.01f, 1 + offset.y - 0.01f)
            };
        }

        public static List<Vector2> NormalBack(Coord offset)
        {
            return new List<Vector2>
            {
                new Vector2(0 + offset.x + 0.01f, 0 + offset.y + 0.01f),
                new Vector2(1 + offset.x - 0.01f, 0 + offset.y + 0.01f),
                new Vector2(0 + offset.x + 0.01f, 1 + offset.y - 0.01f),
                new Vector2(1 + offset.x - 0.01f, 1 + offset.y - 0.01f)
            };
        }

        public static List<Vector2> NormalFront(Coord offset)
        {
            return new List<Vector2>
            {
                new Vector2(1 + offset.x - 0.01f, 0 + offset.y + 0.01f),
                new Vector2(0 + offset.x + 0.01f, 0 + offset.y + 0.01f),
                new Vector2(1 + offset.x - 0.01f, 1 + offset.y - 0.01f),
                new Vector2(0 + offset.x + 0.01f, 1 + offset.y - 0.01f),
            };
        }
        public static List<Vector2> NormalWest(Coord offset)
        {
            return new List<Vector2>
            {
                new Vector2(1 + offset.x - 0.01f, 0 + offset.y + 0.01f),
                new Vector2(1 + offset.x - 0.01f, 1 + offset.y - 0.01f),
                new Vector2(0 + offset.x + 0.01f, 0 + offset.y + 0.01f),
                new Vector2(0 + offset.x + 0.01f, 1 + offset.y - 0.01f),
            };
        }
        public static List<Vector2> NormalEast(Coord offset)
        {
            return new List<Vector2>
            {
                new Vector2(0 + offset.x + 0.01f, 0 + offset.y + 0.01f),
                new Vector2(0 + offset.x + 0.01f, 1 + offset.y - 0.01f),
                new Vector2(1 + offset.x - 0.01f, 0 + offset.y + 0.01f),
                new Vector2(1 + offset.x - 0.01f, 1 + offset.y - 0.01f),
            };
        }
    }
}
