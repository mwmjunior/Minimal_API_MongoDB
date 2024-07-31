using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId] // define que a prop é o Id do objeto
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)] // define o nome do campo no MongoDb como "_id" e o tipo como "ObjectId"
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        // Lista de referências aos produtos pedidos
        [BsonElement("productIds")]
        public List<string>? ProductIds { get; set; }

        [BsonElement("clientId")]
        public string? ClientId { get; set; }

        // Adiciona um dicionário para atributos adicionais
        public Dictionary<string, string?> AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instanciado um obj da classe Order, o atributo AdditionalAttributes já
        /// virá com um novo dicionário e portanto habilitado para adicionar mais atributos
        /// </summary>
        public Order()
        {
            AdditionalAttributes = new Dictionary<string, string?>();
            ProductIds = new List<string>();
        }
    }
}
