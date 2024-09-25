using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Socios.Models;
using System.Data.SqlClient;
using System.Data;

namespace Socios.Services
{
    public interface IDepartamentoRepo
    {
        Task<int> Save(Departamento dto);
        Task<IEnumerable<Departamento>?> GetAll();
        Task<Departamento> GetById(int id);
        Task DeleteById(int id);
        Task<int> Update(Departamento dto);
        Task<int> UpdateSalario(int id, double porcentaje);
    }

    public class DepartamentoRepo : IDepartamentoRepo
    {
        private readonly string? _connectionString;
        public DepartamentoRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task DeleteById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                using var command = new SqlCommand("deleteDepartamento", conn);
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

        public async Task<IEnumerable<Departamento>?> GetAll()
        {
            using (SqlConnection con = new SqlConnection(
            _connectionString))
            {
                try
                {
                    await con.OpenAsync();
                    using var command = new SqlCommand("selectAllDepartamento", con);
                    command.CommandType = CommandType.StoredProcedure;
                    List<Departamento> departamentos = new List<Departamento>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Departamento dto = new Departamento();
                            dto.IdDepartamento = Convert.ToInt32(reader["IdDepartamento"]);
                            dto.Nombre = reader["Nombre"].ToString();
                            departamentos.Add(dto);

                        }
                        return departamentos;
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

        public async Task<Departamento> GetById(int id)
        {
            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                try
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conexion;
                    cmd = new SqlCommand("selectEspecificDepartamento", conexion);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    Departamento departamento = new Departamento();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            departamento.IdDepartamento = Convert.ToInt32(reader["IdDepartamento"]);
                            departamento.Nombre = reader["Nombre"].ToString();
                        }

                    }
                    return departamento;
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

        public async Task<int> Save(Departamento dto)
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                using var command = new SqlCommand("addDepartamento", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@nombre", dto.Nombre);

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

        public async Task<int> Update(Departamento dto)
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                using var command = new SqlCommand("updateDepartamento", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", dto.IdDepartamento);
                command.Parameters.AddWithValue("@nombre", dto.Nombre);
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

        public async Task<int> UpdateSalario(int id, double porcentaje)
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync();
                using var command = new SqlCommand("updateSalarioDepartamento", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@porcentaje", porcentaje);
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