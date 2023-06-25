using DCR.WebApi.Models;

namespace DCR.WebApi.Repositories
{
    public interface IFruitRepository
    {
        List<FruitModel> GetAll();
        FruitModel? GetById(Guid id);

        void Add(FruitModel model);
        void Update(FruitModel model);
        void Delete(FruitModel model);
    }

    public class FruitRepository : IFruitRepository
    {
        private readonly List<FruitModel> Fruits = new();

        public FruitRepository() { }

        public List<FruitModel> GetAll()
        {
            return Fruits;
        }

        public FruitModel? GetById(Guid id)
        {
            return Fruits.SingleOrDefault(x => x.Id == id);
        }

        public void Add(FruitModel model)
        {
            Fruits.Add(model);
        }

        public void Update(FruitModel model)
        {
        }

        public void Delete(FruitModel model) 
        {
            Fruits.Remove(model);
        }
    }
}
