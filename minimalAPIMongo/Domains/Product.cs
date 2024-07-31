using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Product


    {
        [BsonId]// define que a prop e Id do objeto
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)] // define o nome do campo no MongoDb como "id" e o tipo como "ObjectId"


        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }


        [BsonElement("price")]
        public decimal? Price { get; set; }

        // adiciona um dicionario para atributos adicionais 
        public Dictionary<string, string?> AdditionalAttributes { get; set; }


        /// <summary>
        /// Ao ser instaciado um obj da classe product, o atributo  AdditionalAtrributes ja 
        /// vira com um novo dicionario e portanto habilitado para adicionar mais atributos 
        /// </summary>
        public Product()
        {
             AdditionalAttributes = new Dictionary<string, string?>();
        }

        
    }
}
