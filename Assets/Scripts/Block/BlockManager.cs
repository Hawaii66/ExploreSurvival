using System;
using UnityEngine;

namespace Game
{
    public class BlockManager : MonoBehaviour
    {
        public static BlockManager blockManager;

        public BlockSettings[] blocks;

        private void Awake()
        {
            blockManager = this;
        }

        public BlockSettings GetSettings(BlockType type)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].type == type)
                {
                    return blocks[i];
                }
            }

            return null;
        }

        public BlockSettings GetSettings(int index)
        {
            for(int i = 0; i < blocks.Length; i ++)
            {
                if(blocks[i].index == index)
                {
                    return blocks[i];
                }
            }

            return blocks[0];
        }
    }
}
