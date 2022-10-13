using MongoDB.Bson;

namespace Application.Common.Interface
{
    public interface IBsonDocumentMapper<T>
    {
        T map(BsonDocument document);
    }
}
