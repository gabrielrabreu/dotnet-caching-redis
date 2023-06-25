namespace DCR.WebApi.Models
{
    public class FruitModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public FruitModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public FruitModel(Guid id, string name) : this(name)
        {
            Id = id;
        }

        public FruitModel()
        {

        }
    }
}
