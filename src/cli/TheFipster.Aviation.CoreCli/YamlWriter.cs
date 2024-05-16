using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.CoreCli
{
    public class YamlWriter
    {
        private ISerializer serializer;

        public YamlWriter()
            => serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

        public string ToYaml(object data)
            => serializer.Serialize(data);

        public string ToFrontmatter(object data)
            => Const.FrontmatterDelimiter
                + Environment.NewLine
                + ToYaml(data) 
                + Const.FrontmatterDelimiter
                + Environment.NewLine;

        public void Write(string filepath, object data, bool overwrite = false)
        {
            if (File.Exists(filepath) && !overwrite)
                return;

            var yaml = ToYaml(data);
            File.WriteAllText(filepath, yaml);
        }
    }
}
