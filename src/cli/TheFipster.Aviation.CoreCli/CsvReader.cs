using Microsoft.VisualBasic.FileIO;

namespace TheFipster.Aviation.CoreCli
{
    public class CsvReader
    {
        private const string CommentToken = "#";

        public List<string[]> FromFile(string filepath, string delimiter = ",", bool skipHeader = true)
        {
            var lines = new List<string[]>();
            using (TextFieldParser csvParser = new TextFieldParser(filepath))
            {
                csvParser.CommentTokens = [CommentToken];
                csvParser.SetDelimiters([delimiter]);
                csvParser.HasFieldsEnclosedInQuotes = true;

                if (skipHeader)
                    csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    lines.Add(fields);
                }
            }

            return lines;
        }
    }
}
