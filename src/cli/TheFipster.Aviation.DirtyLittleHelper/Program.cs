using TheFipster.Aviation.DirtyLittleHelper.Stuff;
using TheFipster.Aviation.Modules.SimConnectClient.Components;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Started");
        Console.WriteLine();

        // do your dirty little things here
        var recorder = new SimConnectRecorder();
        recorder.Do();

        Console.WriteLine();
        Console.WriteLine("Finished");
    }
}