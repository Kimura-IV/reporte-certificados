using certificados.models.Context;
using certificados.models.Entitys;
using certificados.services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.dal.DataAccess
{
    public class DashboardDA
    {
        private readonly AppDbContext context;

        public DashboardDA(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public ResponseApp getDataDashboard()
        {
            ResponseApp response = Utils.BadResponse(null);
            try { 
            using (var connection = context.Database.GetDbConnection())
            {
                var result = new List<Dictionary<string, object>>();
                connection.Open();
                using (var command = connection.CreateCommand())
                { 
                    for (int x = 1; x <= 5; x++)
                    { 
                        command.CommandText = $"EXEC GET_DATA_{x}";
                        command.CommandType = CommandType.Text;
                             
                        using (var reader = command.ExecuteReader())
                        {
                            var tempResult = new List<Dictionary<string, object>>();
                                 
                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();
                                     
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                } 
                                tempResult.Add(row);
                            } 
                            result.AddRange(tempResult);
                        }
                    }
                }
                 
                response = Utils.OkResponse(result);
            }
            }
            catch (Exception ex)
            {
                response = Utils.BadResponse($"ERROR AL OBTENER DATOS DEL DASHBOARD: {ex.Message}");
                throw new Exception($"ERROR AL OBTENER DATOS: {ex.Message}");
            }
            return response;
        }

    }
}
