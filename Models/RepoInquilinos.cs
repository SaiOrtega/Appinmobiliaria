using System.Data;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoInquilinos
{
    String connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";
    //cambiar base de datos
    public RepoInquilinos()
    {

    }

    public int Alta(Inquilino i)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO inquilino 
            (dni, nombre, apellido, direccion, telefono, email, nacimiento) 
            VALUES (@dni, @nombre, @apellido, @direccion, @telefono, @email, @nacimiento);
            SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@dni", i.Dni);
                command.Parameters.AddWithValue("@nombre", i.Nombre);
                command.Parameters.AddWithValue("@apellido", i.Apellido);
                command.Parameters.AddWithValue("@direccion", i.Direccion);
                command.Parameters.AddWithValue("@telefono", i.Telefono);
                command.Parameters.AddWithValue("@email", i.Email);
                command.Parameters.AddWithValue("@nacimiento", i.FechaNacimiento);
                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                i.Id = res;
                conn.Close();

            }
        }
        return res;

    }

    public List<Inquilino> ObtenerTodos()
    {
        List<Inquilino> lista = new List<Inquilino>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM inquilino";
            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Inquilino p = new Inquilino();
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

    public Inquilino? ObtenerUno(int id)
    {
        Inquilino p = new Inquilino();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM inquilino WHERE id = @id";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    p = new Inquilino();
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

    public int Actualizar(Inquilino i)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE inquilino SET dni = @dni, nombre = @nombre, apellido = @apellido, direccion = @direccion, telefono = @telefono, email = @email, nacimiento = @nacimiento WHERE id = @id";

            var command = new MySqlCommand(sql, conn);

            command.Parameters.AddWithValue("@dni", i.Dni);
            command.Parameters.AddWithValue("@nombre", i.Nombre);
            command.Parameters.AddWithValue("@apellido", i.Apellido);
            command.Parameters.AddWithValue("@direccion", i.Direccion);
            command.Parameters.AddWithValue("@telefono", i.Telefono);
            command.Parameters.AddWithValue("@email", i.Email);
            command.Parameters.AddWithValue("@nacimiento", i.FechaNacimiento);
            command.Parameters.AddWithValue("@id", i.Id);
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
            var sql = @"DELETE FROM inquilino WHERE id = @id";
            var command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            res = command.ExecuteNonQuery();
            conn.Close();
        }
        return res;
    }

}



