using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace minimalAPIMongo.Domains
{
    public class Client
    {
        [BsonId] // define que a prop é o Id do objeto
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)] // define o nome do campo no MongoDb como "_id" e o tipo como "ObjectId"
        public string? Id { get; set; }

        [BsonElement("userId")]
        public string? UserId { get; set; }

        [BsonElement("cpf")]
        public string? CPF { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }

        // adiciona um dicionário para atributos adicionais
        public Dictionary<string, string?> AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instanciado um obj da classe Client, o atributo AdditionalAttributes já
        /// virá com um novo dicionário e portanto habilitado para adicionar mais atributos
        /// </summary>
        public Client()
        {
            AdditionalAttributes = new Dictionary<string, string?>();
        }
    }
}
