using Microsoft.Data.SqlClient;

namespace HelpDeskSystemFixed.Data
{
    public class DatabaseContext
    {
        // String de conexão para o SQL Server local.
        // O usuário deve garantir que o banco 'HelpDeskDB' exista no servidor 'localhost'
        // e que a Autenticação do Windows esteja configurada.
private const string ConnectionString = "Data Source=PE\\SQLEXPRESS;Initial Catalog=HelpDeskDB;Integrated Security=True;TrustServerCertificate=True;";        
        public SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        // Os métodos InitializeDatabase e CreateSampleData foram removidos,
        // pois eram específicos do SQLite e a criação do banco de dados no SQL Server
        // deve ser feita manualmente (ou via script SQL).
    }
}
