using Microsoft.Data.SqlClient;
using HelpDeskSystemFixed.Data;
using HelpDeskSystemFixed.Models;

namespace HelpDeskSystemFixed.Repositories
{
    public class ChamadoRepository
    {
        private readonly DatabaseContext _context;
        
        public ChamadoRepository()
        {
            _context = new DatabaseContext();
        }
        
        public List<Chamado> GetAll()
        {
            var chamados = new List<Chamado>();
            
            using var connection = _context.GetConnection();
            connection.Open();
            
            var query = @"
                SELECT c.*, 
                       col.Nome as ColaboradorNome,
                       tec.Nome as TecnicoNome
                FROM Chamados c
                INNER JOIN Usuarios col ON c.ColaboradorId = col.Id
                LEFT JOIN Usuarios tec ON c.IdTecnico = tec.Id
                ORDER BY c.DataAbertura DESC";
            
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                chamados.Add(MapChamado(reader));
            }
            
            return chamados;
        }
        
        public bool Update(Chamado chamado)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Open();
                
                var query = @"
                    UPDATE Chamados 
                    SET IdTecnico = @idTecnico, 
                        Status = @status,
                        DataConclusao = @dataConclusao
                    WHERE Id = @id";
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idTecnico", chamado.IdTecnico.HasValue ? chamado.IdTecnico.Value : DBNull.Value);
                command.Parameters.AddWithValue("@status", (int)chamado.Status);
                command.Parameters.AddWithValue("@dataConclusao", chamado.DataConclusao.HasValue ? chamado.DataConclusao.Value : DBNull.Value);
                command.Parameters.AddWithValue("@id", chamado.Id);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool Add(Chamado chamado)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Open();
                
                var query = @"
                    INSERT INTO Chamados (ColaboradorId, IdTecnico, Descricao, Status) 
                    VALUES (@colaboradorId, @idTecnico, @descricao, @status)";
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@colaboradorId", chamado.IdColaborador);
                command.Parameters.AddWithValue("@idTecnico", chamado.IdTecnico.HasValue ? chamado.IdTecnico.Value : DBNull.Value);
                command.Parameters.AddWithValue("@descricao", chamado.Descricao);
                command.Parameters.AddWithValue("@status", (int)chamado.Status);
                
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
                
                var query = "DELETE FROM Chamados WHERE Id = @id";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }
        
        public bool FinalizarChamado(int id, int idTecnico)
        {
            try
            {
                using var connection = _context.GetConnection();
                connection.Open();
                
                var query = @"
                    UPDATE Chamados 
                    SET Status = @status, 
                        IdTecnico = @idTecnico,
                        DataConclusao = GETDATE()
                    WHERE Id = @id AND Status != @status"; // Finaliza apenas se não estiver já concluído
                
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@status", (int)StatusChamado.Concluido);
                command.Parameters.AddWithValue("@idTecnico", idTecnico);
                command.Parameters.AddWithValue("@id", id);
                
                return command.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        public Chamado GetById(int id)
        {
            using var connection = _context.GetConnection();
            connection.Open();
            
            var query = @"
                SELECT c.*, 
                       col.Nome as ColaboradorNome,
                       tec.Nome as TecnicoNome
                FROM Chamados c
                INNER JOIN Usuarios col ON c.ColaboradorId = col.Id
                LEFT JOIN Usuarios tec ON c.IdTecnico = tec.Id
                WHERE c.Id = @id";
            
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return MapChamado(reader);
            }
            
            return null;
        }
        
        private Chamado MapChamado(SqlDataReader reader)
        {
            var idTecnicoOrdinal = reader.GetOrdinal("IdTecnico");
            var dataConclusaoOrdinal = reader.GetOrdinal("DataConclusao");
            var tecnicoNomeOrdinal = reader.GetOrdinal("TecnicoNome");
            
            return new Chamado
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                IdColaborador = reader.GetInt32(reader.GetOrdinal("ColaboradorId")),
                IdTecnico = reader.IsDBNull(idTecnicoOrdinal) ? null : reader.GetInt32(idTecnicoOrdinal),
                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                Status = (StatusChamado)reader.GetInt32(reader.GetOrdinal("Status")),
                DataAbertura = reader.GetDateTime(reader.GetOrdinal("DataAbertura")),
                DataConclusao = reader.IsDBNull(dataConclusaoOrdinal) ? null : reader.GetDateTime(dataConclusaoOrdinal),
                Colaborador = new Usuario 
                { 
                    Id = reader.GetInt32(reader.GetOrdinal("ColaboradorId")), 
                    Nome = reader.GetString(reader.GetOrdinal("ColaboradorNome")) 
                },
                Tecnico = reader.IsDBNull(idTecnicoOrdinal) ? null : new Usuario 
                { 
                    Id = reader.GetInt32(idTecnicoOrdinal), 
                    Nome = reader.GetString(tecnicoNomeOrdinal) 
                }
            };
        }
    }
}
