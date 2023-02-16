namespace PaginationSample
{
    public class Discipline
    {
        public int ID { get; internal set; }
        public string Title { get; internal set; }

        public override string ToString()  => Title;
    }
}