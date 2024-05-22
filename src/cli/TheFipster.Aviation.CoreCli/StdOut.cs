using TheFipster.Aviation.Domain.Simbrief.Kml;

namespace TheFipster.Aviation.CoreCli
{
    public static class StdOut
    {
        public static void Write(int indent, string emoji, string message)
        {
            var text = string.Join("", Enumerable.Repeat("\t", indent));
            text += " " + emoji;
            text += " " + message;
            Console.WriteLine(text);
        }

        public static void Write(int indent, string message)
        {
            var text = string.Join("", Enumerable.Repeat("\t", indent));
            text += " " + message;
            Console.WriteLine(text);
        }

        public static bool YesNoDecision(string message, bool selected = true)
        {
            var confirmMsg = selected ? "[Y/n]: " : "[y/N]: ";
            Console.Write(message + " " + confirmMsg);
            var input = Console.ReadLine();

            if (selected)
            {
                if (input.ToLower() != "n")
                    return true;
                else
                    return false;
            }
            else
            {
                if (input.ToLower() != "y")
                    return false;
                else
                    return true;
            }
        }

        public static string Choser(IEnumerable<string> options)
        {
            Console.WriteLine($"There are multiple option:");

            var map = new Dictionary<int, string>();
            int index = 0;

            Console.WriteLine();
            for (int i = 1; i <= options.Count(); i++)
            {
                var file = options.Skip(i - 1).First();
                var json = new XmlReader().ReadToJson(file);
                var data = new JsonReader<SimbriefKmlRaw>().FromText(json);
                map.Add(i, file);
                Console.WriteLine($"{i}: {data.Kml.Document.Name}");
            }

            while (true)
            {
                Console.WriteLine();
                Console.Write($"make your choice [1 - {options.Count()}]: ");

                var input = Console.ReadLine();
                int.TryParse(input, out index);
                if (index > 0 && index <= map.Count)
                    break;

                Console.WriteLine("invalid option, go again.");
            }

            var flight = map[index];
            return flight;
        }
    }
}
