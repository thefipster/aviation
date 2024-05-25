using System.Diagnostics;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class JekyllBuildCommand : ICommand<JekyllBuildOptions>
    {
        private IConfig config;

        public void Run(JekyllBuildOptions options, IConfig config)
        {
            Console.WriteLine(JekyllBuildOptions.Welcome);
            Console.WriteLine();

            this.config = Guard.EnsureConfig(config);

            runTerminalCommand("npm run fonts", "Bundling css.");
            runTerminalCommand("npm run css");
            runTerminalCommand("npm run js", "Bundling js.");
            runTerminalCommand("bundle exec jekyll build --incremental", "Building jekyll");
        }

        private void runTerminalCommand(string command, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                Console.WriteLine(message);

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.WorkingDirectory = config.JekyllFolder;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            Console.WriteLine();
        }
    }
}
