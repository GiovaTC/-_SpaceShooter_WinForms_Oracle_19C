using System;
using Oracle.ManagedDataAccess.Client;

namespace SpaceShooter_WinForms_Oracle_19C__
{
    public static class DataAccess
    {
        // cambie esta cadena por la suya .
        private const string ConnectionString = 
            "User Id=system;Password=Tapiero123;Data Source=localhost:1521/orcl;";

        public static void SaveCore(string player, int score)
        {
            using var conn = new OracleConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO game_scores_wf (player, score, played_at) VALUES (:p_player, :p_score, SYSTIMESTAMP)";
            cmd.Parameters.Add(new OracleParameter("p_player", player));
            cmd.Parameters.Add(new OracleParameter("p_score", score));
            
            cmd.ExecuteNonQuery();
        }
    }
}
