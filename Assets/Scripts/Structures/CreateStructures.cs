using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Structures
{
    public class CreateStructures : MonoBehaviour
    {
        public Coord s;
        public Coord e;
        public Chunk c;
        public Coord cen;
        public new string name;
        public string result;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                string structure = CreateStructure(c, s, e, cen, name);
                Debug.Log(structure);
                result = structure;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(s + e / 2, e);
            Gizmos.DrawWireSphere(cen, 0.5f);
        }

        public static string CreateStructure(Chunk c, Coord start, Coord end, Coord center, string name)
        {
            string output = name + "," + end.x + "," + end.y + "," + end.z + "," + center.x + "," + center.y + "," + center.z + ",";
            Block[,,] blocks = c.blocks;

            for (int y = 0; y < end.y; y++)
            {
                for (int z = 0; z < end.z; z++)
                {
                    for (int x = 0; x < end.x; x++)
                    {
                        Coord gridPos = new Coord(x, y, z) + start;
                        Block b = blocks[gridPos.x, gridPos.y, gridPos.z];
                        int index = BlockManager.blockManager.GetSettings(b.type).index;
                        Coord buildCoord = gridPos - center - start;

                        output += buildCoord.x + ",";
                        output += buildCoord.y + ",";
                        output += buildCoord.z + ",";
                        output += index + ",";
                    }
                }
            }

            output = output.Remove(output.Length - 1);

            return output;
        }
    }
}
