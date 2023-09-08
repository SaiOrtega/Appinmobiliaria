using System.Data;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoPropietarios
{
    string connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";

    public RepoPropietarios()
    {

    }

    public int Alta(Propietario p)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO propietario 
            (dni, nombre, apellido, direccion, telefono, email, nacimiento) 
            VALUES (@dni, @nombre, @apellido, @direccion, @telefono, @email, @nacimiento);
            SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@dni", p.Dni);
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@direccion", p.Direccion);
                command.Parameters.AddWithValue("@telefono", p.Telefono);
                command.Parameters.AddWithValue("@email", p.Email);
                command.Parameters.AddWithValue("@nacimiento", p.FechaNacimiento);
                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                p.Id = res;
                conn.Close();

            }
        }
        return res;

    }

    public List<Propietario> ObtenerTodos()
    {
        List<Propietario> lista = new List<Propietario>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM propietario";
            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Propietario p = new Propietario();
                    p.Id = reader.GetInt32(0);
                    p.Dni = reader.GetString(1);
                    p.Nombre = reader.GetString(2);
                    p.Apellido = reader.GetString(3);
                    p.Direccion = reader.GetString(4);
                    p.Telefono = reader.GetString(5);
                    p.Email = reader.GetString(6);
                    p.FechaNacimiento = reader.GetDateTime(7);
                    lista.Add(p);
                }
                conn.Close();
            }
        }
        return lista;

    }

    public Propietario ObtenerUno(int id)
    {
        Propietario p = new Propietario();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM propietario WHERE id = @id";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    p = new Propietario();
                    {
                        p.Id = reader.GetInt32(0);
                        p.Dni = reader.GetString(1);
                        p.Nombre = reader.GetString(2);
                        p.Apellido = reader.GetString(3);
                        p.Direccion = reader.GetString(4);
                        p.Telefono = reader.GetString(5);
                        p.Email = reader.GetString(6);
                        p.FechaNacimiento = reader.GetDateTime(7);
                    }
                }
                conn.Close();
            }
        }
        return p;
    }

    public int Actualizar(Propietario p)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE inquilino SET dni = @dni, nombre = @nombre, apellido = @apellido, direccion = @direccion, telefono = @telefono, email = @email, nacimiento = @nacimiento WHERE id = @id";

            var command = new MySqlCommand(sql, conn);

            command.Parameters.AddWithValue("@dni", p.Dni);
            command.Parameters.AddWithValue("@nombre", p.Nombre);
            command.Parameters.AddWithValue("@apellido", p.Apellido);
            command.Parameters.AddWithValue("@direccion", p.Direccion);
            command.Parameters.AddWithValue("@telefono", p.Telefono);
            command.Parameters.AddWithValue("@email", p.Email);
            command.Parameters.AddWithValue("@nacimiento", p.FechaNacimiento);
            command.Parameters.AddWithValue("@id", p.Id);
            conn.Open();
            res = command.ExecuteNonQuery();
            conn.Close();
        }
        return res;
    }

    public int Eliminar(int id)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"DELETE FROM propietario WHERE id = @id";
            var command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            res = command.ExecuteNonQuery();
            conn.Close();
        }
        return res;
    }

}