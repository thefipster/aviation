using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.DirtyLittleHelper.Stuff;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Started");
        Console.WriteLine();

        // do your dirty little things here
        new RecordBlackbox().Record("EDDL", "EGLL");

        Console.WriteLine();
        Console.WriteLine("Finished");
    }
}