using pd311_web_api.DAL.Entities;

namespace pd311_web_api.DAL.Repositories.Manufactures
{
    public class ManufactureRepository
        : GenericRepository<Manufacture, string>,
        IManufactureRepository
    {
        public ManufactureRepository(AppDbContext context)
            : base(context) { }
    }
}