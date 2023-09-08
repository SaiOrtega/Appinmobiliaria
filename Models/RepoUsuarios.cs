using System.Data;
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
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO usuario
            (nombre,apellido,email,avatar,clave,rolId)           
            VALUES (@nombre,@apellido,@email,@avatar,@clave,@rolId)";
            using (var command = new MySqlCommand(sql, conn))
            {

                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@email", usuario.Email);
                if (String.IsNullOrEmpty(usuario.Avatar))
                {
                    command.Parameters.AddWithValue("@avatar", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@avatar", usuario.Avatar);
                }
                command.Parameters.AddWithValue("@clave", usuario.Clave);
                command.Parameters.AddWithValue("@rolId", usuario.rol);
                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                usuario.Id = res;
                conn.Close();

            }
        }
        return res;

    }

    public List<Usuario> ObtenerTodos()
    {
        List<Usuario> lista = new List<Usuario>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT id,nombre,apellido,email,clave,rolId, avatar FROM usuario";

            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Usuario u = new Usuario();
                    u.Id = reader.GetInt32(0);
                    u.Nombre = reader.GetString(1);
                    u.Apellido = reader.GetString(2);
                    u.Email = reader.GetString(3);
                    u.Clave = reader.GetString(4);
                    u.rol = reader.GetInt32(5);
                    if (reader.IsDBNull(reader.GetOrdinal("Avatar")))
                    {
                        u.Avatar = null;
                    }
                    else
                    {
                        u.Avatar = reader.GetString(6);
                    }

                    lista.Add(u);
                }
                conn.Close();
            }
        }
        return lista;

    }

    public Usuario? ObtenerUno(int id)
    {
        Usuario? u = null;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT id,nombre,apellido,email,clave,rolId, avatar FROM usuario WHERE id=@id";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    u = new Usuario();
                    {
                        u.Id = reader.GetInt32(0);
                        u.Nombre = reader.GetString(1);
                        u.Apellido = reader.GetString(2);
                        u.Email = reader.GetString(3);
                        u.Clave = reader.GetString(4);
                        u.rol = reader.GetInt32(5);
                        if (reader.IsDBNull(reader.GetOrdinal("Avatar")))
                        {
                            u.Avatar = null;
                        }
                        else
                        {
                            u.Avatar = reader.GetString(6);
                        }

                    }
                }
                conn.Close();
            }
        }
        return u;
    }

    public int Actualizar(Usuario u)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE usuario SET nombre = @Nombre, apellido = @Apellido, avatar =@Avatar ,email = @Email,rol WHERE id = @id";

            var command = new MySqlCommand(sql, conn);

            command.Parameters.AddWithValue("@Nombre", u.Nombre);
            command.Parameters.AddWithValue("@Apellido", u.Apellido);
            command.Parameters.AddWithValue("@Avatar", u.Avatar);
            command.Parameters.AddWithValue("@Email", u.Email);
            command.Parameters.AddWithValue("@Clave", u.Clave);
            command.Parameters.AddWithValue("@rol", u.rol);
            command.Parameters.AddWithValue("@id", u.Id);

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
            var sql = @"DELETE FROM usuario WHERE id = @id";
            var command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            res = command.ExecuteNonQuery();
            conn.Close();
        }
        return res;
    }

    public Usuario ObtenerPorEmail(string email)
    {
        Usuario? usuario = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT
					id, nombre, apellido, avatar, email, clave, rol FROM usuario
					WHERE email=@email";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),

                        Email = reader.GetString("email"),
                        Clave = reader.GetString("clave"),
                        rol = reader.GetInt32("rol"),
                    };
                    if (reader.IsDBNull(reader.GetOrdinal("avatar")))
                    {
                        usuario.Avatar = null;

                    }
                    else
                    {
                        usuario.Avatar = reader.GetString("avatar");
                    }
                }
                connection.Close();
            }
        }
        return usuario;
    }

    public void ActualizarContrasenia(int id, String pass)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE usuario SET clave=@clave WHERE id=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@clave", pass);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
    }

}




