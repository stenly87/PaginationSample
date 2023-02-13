namespace PaginationSample
{
    public class Prepod
    {
        public int ID { get; internal set; }
        public string LastName { get; internal set; }
        public string FirstName { get; internal set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}