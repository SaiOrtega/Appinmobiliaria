using System.Data;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoInmuebles
{
    String connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";

    public RepoInmuebles()
    {

    }

    public int Alta(Inmueble inmueble)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO inmueble 
            (direccion,uso,tipo,ambientes,latitud,longitud,precio,superficie,estado,propietario_id) 
            VALUES (@direccion,@uso,@tipo,@ambientes,@latitud,@longitud,@precio,@superficie,@estado,@propietario_Id);
            SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@direccion", inmueble.direccion);
                command.Parameters.AddWithValue("@ambites", inmueble.ambientes);
                command.Parameters.AddWithValue("@latitud", inmueble.latitud);
                command.Parameters.AddWithValue("@longitud", inmueble.longitud);
                command.Parameters.AddWithValue("@precio", inmueble.precio);
                command.Parameters.AddWithValue("@superficie", inmueble.superficie);
                command.Parameters.AddWithValue("@uso", inmueble.uso);
                command.Parameters.AddWithValue("@propietario_Id", inmueble.propietarioId);


                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                inmueble.Id = res;
                conn.Close();

            }
        }
        return res;

    }

    public List<Inmueble> ObtenerTodos()
    {
        List<Inmueble> listaInmuebles = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id";

            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Inmueble i = new Inmueble();
                    i.Id = reader.GetInt32(0);
                    i.direccion = reader.GetString(1);
                    i.uso = reader.GetString(2);
                    i.tipo = reader.GetString(3);
                    i.ambientes = reader.GetInt32(4);
                    i.latitud = reader.GetDecimal(5);
                    i.longitud = reader.GetDecimal(6);
                    i.precio = reader.GetDecimal(7);
                    i.superficie = reader.GetDecimal(8);

                    i.duenio = new Propietario
                    {
                        Id = reader.GetInt32(6),
                        Dni = reader.GetString(7),
                        Nombre = reader.GetString(8),
                        Apellido = reader.GetString(9)
                    };
                    i.estado = reader.GetString(9);
                    i.propietarioId = reader.GetInt32(10);



                    listaInmuebles.Add(i);
                }
                conn.Close();
            }

        }
        return listaInmuebles;

    }

    public Inmueble ObtenerUno(int id)
    {
        Inmueble i = new Inmueble();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = "SELECT * FROM inmueble WHERE id = @id, from inmueble JOIN propietario ON inmueble.propietarioId = propietario.id WHERE inmueble.id = @id";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    i = new Inmueble();
                    {
                        i.Id = reader.GetInt32(0);
                        i.direccion = reader.GetString(1);
                        i.ambientes = reader.GetInt32(2);
                        i.superficie = reader.GetInt32(3);
                        i.precio = reader.GetDecimal(4);

                        i.duenio = new Propietario
                        {
                            Id = reader.GetInt32(5),
                            Dni = reader.GetString(6),
                            Nombre = reader.GetString(7),
                            Apellido = reader.GetString(8),
                            Direccion = reader.GetString(9),
                            Telefono = reader.GetString(10),
                            Email = reader.GetString(11),
                            FechaNacimiento = reader.GetDateTime(12),
                        };
                        i.estado = reader.GetString(5);
                        i.propietarioId = reader.GetInt32(6);
                    }
                }
                conn.Close();
            }
        }
        return i;
    }

    public int Actualizar(Inmueble i)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE inmueble SET direccion = @direccion,ambientes = @ambientes, latitud = @latitud, longitud = @longitud, precio = @precio,superficie = @superficie, estado = @estado, propietario_Id = @propietario_Id WHERE id = @id";

            var command = new MySqlCommand(sql, conn);

            command.Parameters.AddWithValue("@id", i.Id);
            command.Parameters.AddWithValue("@direccion", i.direccion);
            command.Parameters.AddWithValue("@ambientes", i.ambientes);
            command.Parameters.AddWithValue("@latitud", i.latitud);
            command.Parameters.AddWithValue("@longitud", i.longitud);
            command.Parameters.AddWithValue("@precio", i.precio);
            command.Parameters.AddWithValue("@superficie", i.superficie);
            command.Parameters.AddWithValue("@disponible", i.estado);
            command.Parameters.AddWithValue("@propietarioId", i.propietarioId);


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
            var sql = @"DELETE FROM inmueble WHERE id = @id";
            var command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            res = command.ExecuteNonQuery();
            conn.Close();
        }
        return res;
    }

    public void QuitarDisponibilidad(int id)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE inmueble SET disponible = 0 WHERE id = @id";
            var command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }
    }

}