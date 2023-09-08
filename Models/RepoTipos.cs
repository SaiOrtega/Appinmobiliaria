using System.Data;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoTipos
{

    string connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";
    public RepoTipos()
    {
    }
    public List<Tipo> ObtenerTodos()
    {
        List<Tipo> lista = new List<Tipo>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM tipo";
            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Tipo p = new Tipo();
                    p.Id = reader.GetInt32(0);
                    p.tipo = reader.GetString(1);
                    lista.Add(p);
                }
                conn.Close();
            }
        }
        return lista;

    }
}

