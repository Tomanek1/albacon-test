using AlbaconTest.Services.DatastoreLogic;
using AlbaconTest.Services.DatastoreLogic.Interfaces;
using AlbaconTest.Services.Enums;
using AlbaconTest.Services.Infrastructure;
using AlbaconTest.Services.Infrastructure.Models;
using AlbaconTest.Services.Models;
using AlbaconTest.Services.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace AlbaconTest.Services.Services
{
    public class DatastoreService : IDatastoreService
    {
        private readonly IDatastoreLogic datastoreLogic;

        public DatastoreService(IOptions<Option> options, InMemoryStorage inMemoryStorage)
        {            
            var dataStorage = Enum.Parse<DataStorageTargetEnum>(options.Value.DataStorage);

            if (dataStorage.Equals(DataStorageTargetEnum.Database))
                this.datastoreLogic = new DatabaseDatastoreLogic();
            else if (dataStorage.Equals(DataStorageTargetEnum.Disk))
                this.datastoreLogic = new DiskDataStorageLogic();
            else if (dataStorage.Equals(DataStorageTargetEnum.InMemory))
                this.datastoreLogic = new InMemoryDatastoreLogic(inMemoryStorage);
            else
                throw new ArgumentOutOfRangeException(nameof(Option.DataStorage), dataStorage, "Unknown data storage type!");
        }

        public Task Delete(Guid documentId)
        {
            return datastoreLogic.Delete(documentId);
        }

        public Task<IReadOnlyCollection<Document>> GetAll()
        {
            return datastoreLogic.GetAll();
        }

        public Task<Guid> Insert(Document document)
        {
            //Zde bych zkontroloval, že Guid není v modelu zadaný aby ho generoval systém, ale kvůli jednoduchosti debuggování to vynechám

            if(document.Identifier == Guid.Empty)
                document.Identifier = Guid.NewGuid();

            return datastoreLogic.Insert(document);
        }

        public Task Update(Document document)
        {
            return datastoreLogic.Update(document);
        }
    }
}
