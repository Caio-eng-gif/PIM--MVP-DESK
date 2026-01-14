using Microsoft.Data.SqlClient;
using HelpDeskSystemFixed.Data;
using HelpDeskSystemFixed.Models;

namespace HelpDeskSystemFixed.Repositories
{
    public class UsuarioRepository
    {
        private readonly DatabaseContext _context;
        
        public UsuarioRepository()
        {
            _context = new DatabaseContext();
        }
        
        public List<Usuario> GetByTipo(TipoUsuario tipo)
        {
            var usuarios = new List<Usuario>();
            
            using var connection = _context.GetConnection();
            connection.Open();
            
            var query = "SELECT * FROM Usuarios WHERE Tipo = @tipo AND Ativo = 1 ORDER BY Nome";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@tipo", (int)tipo);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                usuarios.Add(MapUsuario(reader));
            }
            
            return usuarios;
        }
        
        public List<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();
            
            using var connection = _context.GetConnection();
            connection.Open();
            
            var query = "SELECT * FROM Usuarios WHERE Ativo = 1 ORDER BY Nome";
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                usuarios.Add(MapUsuario(reader));
            }
            
            return usuarios;
        }
        
        public Usuario GetById(int id)
        {
            using var connection = _context.GetConnection();
            connection.Open();
            
            var query = "SELECT * FROM Usuarios WHERE Id = @id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return MapUsuario(reader);
            }
            
            return null;
        }
        
        public bool Add(Usuario usuario)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Open();
                
                var query = @"
                    INSERT INTO Usuarios (Nome, Email, Tipo, SenhaHash) 
                    VALUES (@nome, @email, @tipo, @senhaHash)";
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", usuario.Nome);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@tipo", (int)usuario.Tipo);
                command.Parameters.AddWithValue("@senhaHash", usuario.SenhaHash);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool Update(Usuario usuario)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Open();
                
                var query = @"
                    UPDATE Usuarios 
                    SET Nome = @nome, Email = @email, Tipo = @tipo 
                    WHERE Id = @id";
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nome", usuario.Nome);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@tipo", (int)usuario.Tipo);
                command.Parameters.AddWithValue("@id", usuario.Id);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool Delete(int id)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Open();
                
                var query = "UPDATE Usuarios SET Ativo = 0 WHERE Id = @id";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool UpdatePassword(int id, string novaSenhaHash)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Open();
                
                var query = "UPDATE Usuarios SET SenhaHash = @senhaHash WHERE Id = @id";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@senhaHash", novaSenhaHash);
                command.Parameters.AddWithValue("@id", id);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        private Usuario MapUsuario(SqlDataReader reader)
        {
            var emailOrdinal = reader.GetOrdinal("Email");
            
            return new Usuario
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Email = reader.IsDBNull(emailOrdinal) ? string.Empty : reader.GetString(emailOrdinal),
                Tipo = (TipoUsuario)reader.GetInt32(reader.GetOrdinal("Tipo")),
                SenhaHash = reader.GetString(reader.GetOrdinal("SenhaHash")),
                DataCriacao = reader.GetDateTime(reader.GetOrdinal("DataCriacao")),
                Ativo = reader.GetBoolean(reader.GetOrdinal("Ativo"))
            };
        }
    }
}
