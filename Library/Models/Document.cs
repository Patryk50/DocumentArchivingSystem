namespace DocumentArchivingSystem
{
    public class Document
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public int NumOfCopies { get; set; }
    }
}
