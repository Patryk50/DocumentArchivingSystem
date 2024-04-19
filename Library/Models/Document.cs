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

        public Document()
        {
            
        }

        internal Document(int id, Guid uuid, string title, string year, string category, string location, int numOfCopies)
        {
            Id = id;
            UUID = uuid;
            Title = title;
            Year = year;
            Category = category;
            Location = location;
            NumOfCopies = numOfCopies;
        }
    }
}
