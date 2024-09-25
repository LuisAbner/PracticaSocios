using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Socios.Models;

namespace Socios.Services
{
    public interface IAsociadosRepo
    {
        Task<int> Save(Asociados dto);
        Task<IEnumerable<Asociados>?> GetAll();
        Task<Asociados> GetById(int id);
        Task DeleteById(int id);
        Task<int> Update(Asociados dto);
    }
    public class AsociadosRepo : IAsociadosRepo
    {
        private readonly string? _connectionString;
        public AsociadosRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task DeleteById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                using var command = new SqlCommand("deleteAsociado", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error:" + ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<IEnumerable<Asociados>?> GetAll()
        {
            using (SqlConnection con = new SqlConnection(
            _connectionString))
            {
                try
                {
                    await con.OpenAsync();
                    using var command = new SqlCommand("selectAllAsociados", con);
                    command.CommandType = CommandType.StoredProcedure;
                    List<Asociados> asociados = new List<Asociados>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Asociados asociado = new Asociados();
                            asociado.IdAsociado = Convert.ToInt32(reader["IdAsociado"]);
                            asociado.Nombre = reader["Nombre"].ToString();
                            asociado.Apellido = reader["Apellido"].ToString();
                            asociado.Salario = Convert.ToDouble(reader["Salario"].ToString());
                            asociado.NombreDepartamento = reader["nomDto"].ToString();
                            asociado.DepartamentoId = Convert.ToInt32(reader["IdDepartamento"]);
                            asociados.Add(asociado);

                        }
                        return asociados;
                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error" + ex);
                    return null;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public async Task<Asociados> GetById(int id)
        {
            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                try
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conexion;
                    cmd = new SqlCommand("selectEspecificAsociado", conexion);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    Asociados asociado = new Asociados();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            asociado.IdAsociado = Convert.ToInt32(reader["IdAsociado"]);
                            asociado.Nombre = reader["Nombre"].ToString();
                            asociado.Apellido = reader["Apellido"].ToString();
                            asociado.Salario = Convert.ToDouble(reader["Salario"].ToString());
                            asociado.NombreDepartamento = reader["nomDto"].ToString();
                            asociado.DepartamentoId = Convert.ToInt32(reader["IdDepartamento"]);
                        }

                    }
                    return asociado;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error" + ex);
                    return null;
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        public async Task<int> Save(Asociados dto)
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                using var command = new SqlCommand("addAsociado", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@nombre", dto.Nombre);
                command.Parameters.AddWithValue("@apellido", dto.Apellido);
                command.Parameters.AddWithValue("@salario", dto.Salario);
                command.Parameters.AddWithValue("@departamentoId", dto.DepartamentoId);


                command.Parameters.Add("@EId", SqlDbType.Int);
                command.Parameters["@EId"].Direction = ParameterDirection.Output;
                await command.ExecuteNonQueryAsync();

                int response = (int)command.Parameters["@EId"].Value;

                return response;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error" + ex);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<int> Update(Asociados dto)
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                using var command = new SqlCommand("updateAsociado", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nombre", dto.Nombre);
                command.Parameters.AddWithValue("@apellido", dto.Apellido);
                command.Parameters.AddWithValue("@salario", dto.Salario);
                command.Parameters.AddWithValue("@departamentoId", dto.DepartamentoId);
                command.Parameters.AddWithValue("@Id", dto.IdAsociado);
                await command.ExecuteNonQueryAsync();
                return 1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error:" + ex);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}