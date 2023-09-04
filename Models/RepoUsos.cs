using System.Data;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoUsos
{
    string connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";

    public RepoUsos()
    {

    }
    public List<Uso> ObtenerTodos()
    {
        List<Uso> lista = new List<Uso>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM uso";
            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Uso p = new Uso();
                    p.Id = reader.GetInt32(0);
                    p.uso = reader.GetString(1);
                    lista.Add(p);
                }
                conn.Close();
            }
        }
        return lista;
    }
}

