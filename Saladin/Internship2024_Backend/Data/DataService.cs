using Data.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data
{
    public class DataService
    {
        private readonly ILogger<DataService> _logger;
        private readonly CacheService _cacheService;
        private readonly string _connectionString = "Server=ts-internship.database.windows.net;Database=Saladin;User Id=sa_admin;Password=schimba-MA";

        public DataService(ILogger<DataService> logger, CacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<List<Knight>> GetKights(string queryString, CancellationToken cancellationToken)
        {
            //_cacheService.Read<List<Knight>>(queryString, out List<Knight> knights);
            List<Knight> knights = null;

            if (knights != null)
            {
                return knights;
            }

            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                knights = new List<Knight>();
                while (await reader.ReadAsync(cancellationToken))
                {
                    //while (reader != null)
                    //{
                        Knight knight = new Knight()
                        {
                            KnightId = int.Parse(reader["KnightId"].ToString()),
                            Name = reader["Name"].ToString(),
                            DictionaryKnightTypeName = reader["DictionaryKnightTypeName"].ToString(),
                            LegionName = reader["LegionName"].ToString(),
                            BattleName = reader["BattleName"].ToString(),
                            BattleId = int.TryParse(reader["BattleId"].ToString(), out int battleId) ? battleId: 0,
                            CoinsAwardedPerBattle = int.TryParse(reader["CoinsAwardedPerBattle"].ToString(), out int coinsAwarded) ? coinsAwarded : 0
                        };


                        

                        Console.WriteLine($"Adding knight to list {JsonConvert.SerializeObject(knight)}");
                        knights.Add(knight);
                    //}
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message, ex);
            }

            return _cacheService.Write(queryString, knights);

        }

        public async Task ChangeCoinsAwardedPerBattle(string commandString, CancellationToken cancellationToken)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandString, connection);

            List<Knight> knights = new List<Knight>();
            

            try
            {
                connection.Open();
                int rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message, ex);
            };
        }
    }
}
