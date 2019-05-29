using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ASD2_1
{

    class Vec3D
    {
        public int x, y, z;
        public Vec3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    class Program
    {
        static int lc(Vec3D pos, Vec3D bounds, ref string[,] c, ref int vel, bool up)
        {
            vel = vel + 1;

            char[] temp = c[pos.z, pos.y].ToCharArray();
            temp[pos.x] = 'x';
            c[pos.z, pos.y] = new string(temp);

            int my = 0;
            int p0 = 0, p1 = 0, p2 = 0, p3 = 0, p4 = 0;
            if( pos.z + 1 < bounds.z )
                if (c[pos.z + 1, pos.y].ElementAt<char>(pos.x) == 'o')
                {
                    my = lc(new Vec3D(pos.x, pos.y, pos.z + 1), new Vec3D(bounds.x, bounds.y, bounds.z), ref c, ref vel, up);
                }
            if( pos.z - 1 >= 0 )
                if(c[pos.z - 1, pos.y].ElementAt<char>(pos.x) == 'o')
                {
                    p0 = lc(new Vec3D(pos.x, pos.y, pos.z - 1), new Vec3D(bounds.x, bounds.y, bounds.z), ref c, ref vel, up);
                }

            if ( pos.x - 1 >= 0 )
                    if (c[pos.z, pos.y].ElementAt<char>(pos.x - 1) == 'o')
                {
                    p1 = lc(new Vec3D(pos.x - 1, pos.y, pos.z), new Vec3D(bounds.x, bounds.y, bounds.z), ref c, ref vel, up);
                }

            if ( pos.x + 1 < bounds.x )
                    if (c[pos.z, pos.y].ElementAt<char>(pos.x + 1) == 'o')
                {
                    p2 = lc(new Vec3D(pos.x + 1, pos.y, pos.z), new Vec3D(bounds.x, bounds.y, bounds.z), ref c, ref vel, up);
                }

            if ( pos.y + 1 < bounds.y ) 
                    if (c[pos.z, pos.y + 1].ElementAt<char>(pos.x) == 'o')
                {
                    p3 = lc(new Vec3D(pos.x, pos.y + 1, pos.z), new Vec3D(bounds.x, bounds.y, bounds.z), ref c, ref vel, up);
                }

            if ( pos.y - 1 >= 0 )
                    if (c[pos.z, pos.y-1].ElementAt<char>(pos.x) == 'o')
                {
                    p4 = lc(new Vec3D(pos.x, pos.y-1, pos.z), new Vec3D(bounds.x, bounds.y, bounds.z), ref c, ref vel, up);
                }

            if (up)
            {
                var papa = new List<int>();
                papa.Add(p0); papa.Add(p1); papa.Add(p2); papa.Add(p3); papa.Add(p4); papa.Add(my); papa.Add(pos.z);
                my = papa.Max();
            }
            return my;
        }

        static void Main(string[] args)
        {
            DateTime loadS = DateTime.Now;
            int x, y, z;
            string[,] cave;
            using (StreamReader sr = File.OpenText("in3.txt"))
            {
                string s = sr.ReadLine();
                string []dims = s.Split(' ');
                x = int.Parse(dims[0]);
                y = int.Parse(dims[1]);
                z = int.Parse(dims[2]);

                cave = new string[z, y];
                for (int i = 0; i < z; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        cave[i, j] = sr.ReadLine();
                        while (cave[i, j] == "") cave[i, j] = sr.ReadLine();
                    }
                }
            }

            DateTime loadE = DateTime.Now;

            int izolated = 0;
            bool up = true;
            var dipp = new List<int>();
            var bigg = new List<int>();
            for (int i = 0; i < z; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    for (int k = 0; k < x; k++)
                    {
                        if (cave[i, j].ElementAt(k) != 'o') continue;
                        if (i != 0)
                        {
                            izolated++;
                            up = false;
                        }

                        int vel = 0;
                        int pom = lc(new Vec3D(k, j, i), new Vec3D(x, y, z), ref cave, ref vel, up) + 1;
                        if (i == 0 && pom != 0) dipp.Add(pom);
                        bigg.Add(vel);
                    }
                }
            }

            Console.WriteLine("Dippest: "+dipp.Max()+"\nBiggest: "+bigg.Max()+"\nIzolated caves: "+izolated);

            DateTime algEnd = DateTime.Now;

            TimeSpan load = loadE - loadS;
            TimeSpan alg = algEnd - loadE;
            if (load.TotalMilliseconds >= 1000)
                Console.WriteLine("File loading time: "+load.TotalSeconds + " s.");
            else
                Console.WriteLine("File loading time: " + load.TotalMilliseconds + " ms.");
            if (alg.TotalMilliseconds >= 1000)
                Console.WriteLine("Algorithm run time: " + alg.TotalSeconds + " s.");
            else
                Console.WriteLine("Algorithm run time: " + alg.TotalMilliseconds + " ms.");

            using (StreamWriter sw = File.CreateText("out.txt"))
            {
                sw.WriteLine(dipp.Max() + " " + bigg.Max() + " " + izolated);
            }

            Console.ReadKey();
        }
    }
}
