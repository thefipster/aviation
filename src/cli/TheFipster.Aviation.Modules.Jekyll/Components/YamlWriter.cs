using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class YamlWriter
    {
        public string ConvertToFrontmatter(object data)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(data);

            var frontmatter = 
                Const.FrontmatterDelimiter
                + Environment.NewLine
                + yaml
                + Const.FrontmatterDelimiter
                + Environment.NewLine;

            return frontmatter;
        }
    }
}
