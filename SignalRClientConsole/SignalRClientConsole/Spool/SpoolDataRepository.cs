using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damienbod.SignalR.IHubSync.Client.Dto;

namespace SignalRClientConsole.Spool
{
    public class SpoolDataRepository
    {
        static SQLiteConnection _mDbConnection;

        public SpoolDataRepository()
        {
            _mDbConnection = new SQLiteConnection("Data Source=SignalRSpool.sqlite;Version=3;");
            _mDbConnection.Open();
        }

        public void AddSignalRMessageDto(SignalRMessageDto message)
        {
            //string sql = "CREATE TABLE \"MessageDto\" (\"String1\" TEXT NOT NULL  DEFAULT none, \"String2\" TEXT NOT NULL  DEFAULT none, \"Int1\" INTEGER NOT NULL  DEFAULT 0, \"Int2\" INTEGER NOT NULL  DEFAULT 0, \"Id\" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL )";
            //SQLiteCommand command = new SQLiteCommand(sql, _mDbConnection);
            //command.ExecuteNonQuery();

            var sql = new StringBuilder();
            sql.AppendFormat("insert into MessageDto (String1, String2, Int1, Int2) values ('{0}', '{1}', {2}, {3})",
                message.String1,
                message.String2,
                message.Int1,
                message.Int2);

            var command = new SQLiteCommand(sql.ToString(), _mDbConnection);
            command.ExecuteNonQuery();
        }

        public void RemoveSignalRMessageDto(int id)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("DELETE FROM MessageDto WHERE Id = {0}",  id);
            var command = new SQLiteCommand(sql.ToString(), _mDbConnection);
            command.ExecuteNonQuery();
        }

        public SignalRMessageDto GetNextSignalRMessageDto()
        {
            const string sql = "select * from MessageDto order by Id LIMIT 1";
            var command = new SQLiteCommand(sql, _mDbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var messageDto = new SignalRMessageDto
                {
                    String1 = reader["String1"].ToString(),
                    String2 = reader["String2"].ToString(),
                    Int1 = Convert.ToInt32(reader["Int1"]),
                    Int2 = Convert.ToInt32(reader["Int2"]),
                    Id = Convert.ToInt32(reader["Id"])
                };

                return messageDto;
            }

            return null;
        }
    }
}
