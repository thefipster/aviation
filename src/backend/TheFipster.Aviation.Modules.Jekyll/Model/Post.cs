namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    public class Post
    {
        public Post(string name, string frontmatter)
        {
            Name = name;
            Frontmatter = frontmatter;
        }

        public string Name { get; set; }
        public string Frontmatter { get; set; }
    }
}
