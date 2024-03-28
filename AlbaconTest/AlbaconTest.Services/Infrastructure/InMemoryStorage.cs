using AlbaconTest.Services.Models;

namespace AlbaconTest.Services.Infrastructure
{
    /// <summary>
    /// Úložiště v paměti je řešeno jako singleton
    /// </summary>
    public class InMemoryStorage
    {
        private readonly List<Document> documents;

        public InMemoryStorage()
        {
            documents = new List<Document>();
        }

        internal void Delete(Guid documentId)
        {
            documents.RemoveAll(d => d.Identifier == documentId);
        }

        internal IReadOnlyCollection<Document> GetAll()
        {
            return documents;
        }

        internal void Insert(Document document)
        {
            documents.Add(document);
        }

        internal void Update(Document document)
        {
            var existing = documents.Single(d => d.Identifier == document.Identifier);

            existing.Tags = document.Tags;
            existing.Data = document.Data;
        }
    }
}
