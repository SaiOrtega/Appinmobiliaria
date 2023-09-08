using System.Data;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoUsuarios
{
    String connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";
    //cambiar base de datos
    public RepoUsuarios()
    {

    }

    public int Alta(Usuario usuario)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"INSERT INTO Usuario
					(Nombre, Apellido, Avatar, Email, Clave, Rol) 
					VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);
					SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                if (String.IsNullOrEmpty(usuario.Avatar))
                    command.Parameters.AddWithValue("@avatar", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@avatar", usuario.Avatar);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@clave", usuario.Clave);
                command.Parameters.AddWithValue("@rol", usuario.Rol);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                usuario.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    [Authorize(Policy = "Administrador")]
    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = "DELETE FROM Usuario WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public int Modificacion(Usuario usuario)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE usuario 
					SET Nombre=@nombre, Apellido=@apellido,Email=@email, Avatar=@avatar, Rol=@rol
					WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@avatar", usuario.Avatar);
                command.Parameters.AddWithValue("@clave", usuario.Clave);
                command.Parameters.AddWithValue("@rol", usuario.Rol);
                command.Parameters.AddWithValue("@id", usuario.Id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public IList<Usuario> ObtenerTodos()
    {
        IList<Usuario> res = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"
					SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol
					FROM Usuario";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = reader.GetInt32("Rol"),

                    };
                    if (reader.IsDBNull(reader.GetOrdinal("Avatar")))
                    {
                        usuario.Avatar = null;

                    }
                    else
                    {
                        usuario.Avatar = reader.GetString("Avatar");
                    }
                    res.Add(usuario);
                }
                connection.Close();
            }
        }
        return res;
    }

    public Usuario ObtenerPorId(int id)
    {
        Usuario? usuario = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT 
					Id, Nombre, Apellido, Avatar, Email, Clave, Rol 
					FROM Usuario
					WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),

                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = reader.GetInt32("Rol"),
                    };
                    if (reader.IsDBNull(reader.GetOrdinal("Avatar")))
                    {
                        usuario.Avatar = null;

                    }
                    else
                    {
                        usuario.Avatar = reader.GetString("Avatar");
                    }
                }
                connection.Close();
            }
        }
        return usuario;
    }

    public Usuario ObtenerPorEmail(string email)
    {
        Usuario? usuario = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT
					Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM Usuario
					WHERE Email=@email";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),

                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = reader.GetInt32("Rol"),
                    };
                    if (reader.IsDBNull(reader.GetOrdinal("Avatar")))
                    {
                        usuario.Avatar = null;

                    }
                    else
                    {
                        usuario.Avatar = reader.GetString("Avatar");
                    }
                }
                connection.Close();
            }
        }
        return usuario;
    }

    public void ActualizarContraseña(int id, String Contra)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE usuario SET Clave=@clave WHERE Id=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@clave", Contra);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
    }


}


