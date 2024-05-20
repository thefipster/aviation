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
    }
}
