using MongoDB.Driver;

namespace minimalAPIMongo.Services
{
    public class MongoDbService
    {


        /// <summary>
        /// Armazena a configuracao da aplicacao 
        /// </summary>
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Armazena uma referencia ao MongoDb
        /// </summary>
        private readonly IMongoDatabase _database;


        /// <summary>
        /// Recebe a configuracao da aplicacao como parametro
        /// </summary>
        /// <param name="configuration">objeto configuration</param>

        public MongoDbService(IConfiguration configuration)
        {
             // atribui a configuracao recebida em _configuration
            _configuration = configuration;

            //Obtem a string de conexao  atraves do _configuration
            var connectionString = _configuration.GetConnectionString("DbConnection");


            // cria um objeto MongoUrl que recebe como paramestro a srtrig de conexao 
            var mongoUrl = MongoUrl.Create(connectionString);

            // cria um MongoClient para se conectar ao MongoDb
            var mongoClient = new MongoClient(mongoUrl);

            //obtem a referncia ao banco de dados com o nome especificado atraves da string de conexao 
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
                                                //mongodb:
        }

        // toda vez que o GetDatabase for chamado ele retorna refencias ao banco de dados 
        public IMongoDatabase GetDatabase => _database;

    }
}
