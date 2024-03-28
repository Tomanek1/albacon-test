using AlbaconTest.Services.DatastoreLogic.Interfaces;
using AlbaconTest.Services.Models;

namespace AlbaconTest.Services.DatastoreLogic
{
    internal class CloudDatastoreLogic : IDatastoreLogic
    {
        public Task Delete(Guid documentId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Document>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Insert(Document document)
        {
            throw new NotImplementedException();
        }

        public Task Update(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
