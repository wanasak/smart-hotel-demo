using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SmartHotel.Services.Booking.Queries
{
    public class OccupancyQuery
    {
        private readonly string _constr;

        public OccupancyQuery(string constr) => _constr = constr;

        public async Task<(double sunny, double notSunny)> GetRoomOcuppancy(DateTime date, int idRoom) => (await GetRoomOcuppancy(date, idRoom, isSunny: true), 0);

        private async Task<double> GetRoomOcuppancy(DateTime date, int idRoomn, bool isSunny)
        {
            var day = date.Date;

            using (var con = new SqlConnection(_constr))
            {
                await con.OpenAsync();
                var sql = "PredictOccupation";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@date", SqlDbType.Date).Value = day;
                    cmd.Parameters.Add("@idRoom", SqlDbType.Int).Value = idRoomn;
                    cmd.Parameters.Add("@isSunny", SqlDbType.Bit).Value = isSunny;

                    var percent = Convert.ToDouble(await cmd.ExecuteScalarAsync());

                    return percent;
                }
            }
        }
    }
}