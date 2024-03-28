using AlbaconTest.Services.Models;

namespace AlbaconTest.Services.DatastoreLogic.Interfaces
{
    internal interface IDatastoreLogic
    {

        Task<IReadOnlyCollection<Document>> GetAll();

        Task<Guid> Insert(Document document);
        Task Update(Document document);

        Task Delete(Guid documentId);
    }
}
