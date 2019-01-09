using Dapper;
using FINBANKRECONSERVICE.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINBANKRECONSERVICE
{
    public class Db
    {
        private string connString;
        public Db(string dbConnString)
        {
            this.connString = dbConnString;
        }

        public GenericModel GetSettings(int code)
        {
            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = "sp_GetsysSettings";

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@itemcode", code);

                    return conn.Query<GenericModel>(sql, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Util.LogError("GetSettings", ex);
            }

            return null;
        }
        public GenericModel insertfiledata(DeclarationQueryResponse data)
        {
            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = "sp_insertFiledata";

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@OfficeCode", data.OfficeCode);
                    parameters.Add("@OfficeName", data.OfficeName);

                    return conn.Query<GenericModel>(sql, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Util.LogError("sp_insertFiledata", ex);
            }

            return null;
        }

    }
}
