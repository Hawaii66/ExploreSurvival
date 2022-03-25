using System.Collections;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName ="BlockSetting",menuName="Custom/Blocksetting")]
    public class BlockSettings : ScriptableObject
    {
        public BlockType type;
        public int index;
        public Coord[] TextureOffsets;
        public bool isSolid;
        public bool isFullblock;
    }
}