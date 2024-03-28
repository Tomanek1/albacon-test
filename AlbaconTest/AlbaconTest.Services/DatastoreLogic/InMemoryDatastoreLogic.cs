using AlbaconTest.Services.DatastoreLogic.Interfaces;
using AlbaconTest.Services.Infrastructure;
using AlbaconTest.Services.Models;

namespace AlbaconTest.Services.DatastoreLogic
{
    internal class InMemoryDatastoreLogic : IDatastoreLogic
    {
        private readonly InMemoryStorage inMemoryStorage;

        public InMemoryDatastoreLogic(InMemoryStorage inMemoryStorage)
        {
            this.inMemoryStorage = inMemoryStorage;
        }


        public Task Delete(Guid documentId)
        {
            inMemoryStorage.Delete(documentId);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Document>> GetAll()
        {
            return Task.FromResult(inMemoryStorage.GetAll());
        }

        public Task<Guid> Insert(Document document)
        {
            inMemoryStorage.Insert(document);
            return Task.FromResult(document.Identifier);
        }

        public Task Update(Document document)
        {
            inMemoryStorage.Update(document);
            return Task.CompletedTask;
        }
    }
}
