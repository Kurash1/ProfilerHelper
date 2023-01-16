using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ProfilerHelper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException("Not Enough Arguments");
            List<instantce> instantceList = new List<instantce>();
            Regex regex = new Regex("([^ ]+) *([^ ]+) *([^ ]+) *([^ ]+) *([^ ]+) *([^ ]+) *([^ ]+)");
            string[] file = File.ReadAllLines(args[0]);

            for (int i = 0; i < file.Length; i++) 
            {
                if (file[i].StartsWith("-"))
                    continue;
                if (file[i].StartsWith(" "))
                {
                    if (instantceList.Count > 0)
                        instantceList[instantceList.Count - 1].sub.Add(file[i]);
                    continue;
                }
                Match match = regex.Match(file[i]);
                instantceList.Add(new instantce(
                        match.Groups[1].Value,
                        float.Parse(match.Groups[2].Value.Substring(0,match.Groups[2].Value.Length-1)),
                        float.Parse(match.Groups[3].Value.Substring(0,match.Groups[3].Value.Length-1)),
                        float.Parse(match.Groups[4].Value.Substring(0,match.Groups[4].Value.Length-1)),
                        float.Parse(match.Groups[5].Value.Substring(0,match.Groups[5].Value.Length-2)),
                        float.Parse(match.Groups[6].Value.Substring(0,match.Groups[6].Value.Length-2)),
                        int.Parse(match.Groups[7].Value),
                        i,
                        new List<string>()
                    ));
            }

            IEnumerable<instantce> sortedList = sort(args, instantceList);

            Console.WriteLine(
                "line".PadRight(5) + "|" + "name".PadRight(105) + "root%" + "parent%".PadLeft(10) + "other%".PadLeft(10) + "total_time".PadLeft(16) + "single_time".PadLeft(16) + "hits".PadLeft(20)
            );
            foreach(instantce instantce in sortedList)
            {
                Console.WriteLine(instantce.toString());
            }

            Console.WriteLine("Press any to exit");
            Console.ReadKey();
        }
        static IEnumerable<instantce> sort(string[] args, List<instantce> instantceList)
        {
            switch (args[1].ToUpper())
            {
                case "LINE":
                    return from instantce in instantceList orderby instantce.line descending select instantce;
                case "NAME":
                    return from instantce in instantceList orderby instantce.name descending select instantce;
                case "ROOT":
                    return from instantce in instantceList orderby instantce.root descending select instantce;
                case "PARENT":
                    return from instantce in instantceList orderby instantce.parent descending select instantce;
                case "OTHER":
                    return from instantce in instantceList orderby instantce.other descending select instantce;
                case "SINGLE_TIME":
                    return from instantce in instantceList orderby instantce.single_time descending select instantce;
                case "HITS":
                    return from instantce in instantceList orderby instantce.hits descending select instantce;
                case "TOTAL_TIME":
                default:
                    return from instantce in instantceList orderby instantce.total_time descending select instantce;
            }
        }
    }
    class instantce
    {
        public string name;
        public float root;
        public float parent;
        public float other;
        public float total_time;
        public float single_time;
        public int hits;
        public int line;
        public List<string> sub;

        public instantce(string n, float r, float p, float o, float tt, float st, int h, int l, List<string> s)
        {
            name = n;
            root = r;
            parent = p;
            other = o;
            total_time = tt;
            single_time = st;
            hits = h;
            line = l;
            sub = s;
        }
        public string toString()
        {
            return line.ToString().PadRight(5) + "|" + name.PadRight(105) + (root + "%") + (parent + "%").PadLeft(10) + (other + "%").PadLeft(10) + (total_time + "ms").PadLeft(16) + (single_time + "ns").PadLeft(16) + hits.ToString().PadLeft(20);
        }
    }
}
