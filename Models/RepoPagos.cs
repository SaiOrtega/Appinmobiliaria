using System.Data;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoPagos
{
    string connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";
    public RepoPagos()
    {
    }
    public List<Pago> ObtenerTodos(int? id)
    {
        List<Pago> listaPagos = new List<Pago>();

        if (id != null && id != 0)
        {

            var sql = "SELECT id, importe, fecha_pago, periodoi.apellido, p.apellido FROM pago,inquilino i,propietario p WHERE contrato_id = @id";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago();
                        p.id = reader.GetInt32(0);
                        p.importe = reader.GetDecimal(1);
                        p.fechaPago = reader.GetDateTime(2);
                        p.periodo = reader.GetDateTime(3);
                        p.contratoId = reader.GetInt32(4);

                        listaPagos.Add(p);
                    }
                    conn.Close();
                }
            }

        }
        else
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                var sql = @"SELECT id, importe, fecha_pago, periodo, contrato_id FROM pago";
                using (var command = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pago p = new Pago();
                            p.id = reader.GetInt32(0);
                            p.importe = reader.GetDecimal(1);
                            p.fechaPago = reader.GetDateTime(2);
                            p.periodo = reader.GetDateTime(3);
                            p.contratoId = reader.GetInt32(4);
                            listaPagos.Add(p);
                        }
                    }
                    conn.Close();
                }
            }
        }
        return listaPagos;
    }

    public int Alta(Pago p)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {

            var sql = @"INSERT INTO pago (importe, fecha_pago, periodo, contrato_id) VALUES (@importe, @fechaPago, @periodo, @contrato_id)";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@importe", p.importe);
                command.Parameters.AddWithValue("@fechaPago", p.fechaPago);
                command.Parameters.AddWithValue("@periodo", p.periodo);
                command.Parameters.AddWithValue("@contrato_id", p.contratoId);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
                p.id = res;
            }
        }
        return res;
    }

    public int Actualizar(Pago p)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE pago SET importe = @importe, fecha_pago = @fechaPago, periodo = @periodo WHERE id = @id";
            using (var command = new MySqlCommand(sql, conn))
            {

                command.Parameters.AddWithValue("@importe", p.importe);
                command.Parameters.AddWithValue("@fechaPago", p.fechaPago);
                command.Parameters.AddWithValue("@periodo", p.periodo);
                command.Parameters.AddWithValue("@id", p.id);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    public Pago ObtenerUno(int id)
    {
        Pago p = null;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT id, importe, fecha_pago, periodo, contrato_id FROM pago WHERE id = @id";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        p = new Pago();
                        p.id = reader.GetInt32(0);
                        p.importe = reader.GetDecimal(1);
                        p.fechaPago = reader.GetDateTime(2);
                        p.periodo = reader.GetDateTime(3);
                        p.contratoId = reader.GetInt32(4);
                    }
                }
                conn.Close();

            }
        }
        return p;

    }

    public int Eliminar(int id)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"DELETE FROM pago WHERE id = @id";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

}

