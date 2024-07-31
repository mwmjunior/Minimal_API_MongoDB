using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace minimalAPIMongo.Domains
{
    public class User
    {
        [BsonId] // define que a prop é o Id do objeto
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)] // define o nome do campo no MongoDb como "_id" e o tipo como "ObjectId"
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        // adiciona um dicionário para atributos adicionais
        public Dictionary<string, string?> AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instanciado um obj da classe User, o atributo AdditionalAttributes já
        /// virá com um novo dicionário e portanto habilitado para adicionar mais atributos
        /// </summary>
        public User()
        {
            AdditionalAttributes = new Dictionary<string, string?>();
        }
    }
}
