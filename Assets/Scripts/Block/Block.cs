using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum BlockType { Air, Stone,Dirt,Wood,Leave,Worbbench,Stonebench};

    public class Block
    {
        public bool isSolid;
        public bool isFullBlock;
        public BlockType type;

        public Block(BlockType t, bool i, bool f)
        {
            type = t;
            isSolid = i;
            isFullBlock = f;
        }

        public Block(BlockSettings settings)
        {
            type = settings.type;
            isSolid = settings.isSolid;
            isFullBlock = settings.isFullblock;
        }
    }
}
