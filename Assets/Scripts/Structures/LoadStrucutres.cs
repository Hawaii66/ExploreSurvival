using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Structures
{
    public static class LoadStrucutres
    {
        public static Structure LoadStructure(string file)
        {
            string[] sections = file.Split(',');
            string name = sections[0];
            int x = Int32.Parse(sections[1]);
            int y = Int32.Parse(sections[2]);
            int z = Int32.Parse(sections[3]);
            int xCenter = Int32.Parse(sections[4]);
            int yCenter = Int32.Parse(sections[5]);
            int zCenter = Int32.Parse(sections[6]);

            List<SimpleBlock> blocks = new List<SimpleBlock>();
            for(int i = 7; i < sections.Length; i += 4)
            {
                Coord gridPos = new Coord(Int32.Parse(sections[i]), Int32.Parse(sections[i + 1]), Int32.Parse(sections[i + 2]));
                int blockIndex = Int32.Parse(sections[i + 3]);
                blocks.Add(new SimpleBlock(blockIndex, gridPos));
            }

            return new Structure(name, new Coord(x, y, z), new Coord(xCenter, yCenter, zCenter), blocks.ToArray());
        }
    }

    public struct Structure
    {
        public string name;
        public Coord size;
        public Coord center;
        public SimpleBlock[] blocks;

        public Structure(string n, Coord s, Coord c, SimpleBlock[] b)
        {
            name = n;
            size = s;
            center = c;
            blocks = b;
        }
    }

    public struct SimpleBlock
    {
        public int index;
        public Coord position;

        public SimpleBlock(int i, Coord p)
        {
            index = i;
            position = p;
        }
    }
}
