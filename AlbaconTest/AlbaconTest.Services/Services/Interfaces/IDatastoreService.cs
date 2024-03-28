using AlbaconTest.Services.Models;

namespace AlbaconTest.Services.Services.Interfaces
{
    public interface IDatastoreService
    {
        Task<IReadOnlyCollection<Document>> GetAll();

        Task<Guid> Insert(Document document);

        Task Update(Document document);

        Task Delete(Guid documentId);

    }
}
