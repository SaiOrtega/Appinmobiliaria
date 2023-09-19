using System.Data;
using Microsoft.AspNetCore.Authorization;
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
            (direccion, ambientes, latitud, longitud, precio, superficie, estado, propietario_id, uso, tipo) 
            VALUES (@direccion, @ambientes, @latitud, @longitud, @precio, @superficie, @estado, @propietario_id, @uso, @tipo);
            SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@direccion", inmueble.direccion);
                command.Parameters.AddWithValue("@ambientes", inmueble.ambientes);
                command.Parameters.AddWithValue("@latitud", inmueble.latitud);
                command.Parameters.AddWithValue("@longitud", inmueble.longitud);
                command.Parameters.AddWithValue("@precio", inmueble.precio);
                command.Parameters.AddWithValue("@superficie", inmueble.superficie);
                command.Parameters.AddWithValue("@estado", inmueble.estado);
                command.Parameters.AddWithValue("@propietario_Id", inmueble.propietarioId);
                command.Parameters.AddWithValue("@uso", inmueble.uso);
                command.Parameters.AddWithValue("@tipo", inmueble.tipo);

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
            //var sql = "SELECT * FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id";
            var sql = @"SELECT i.id, i.direccion, i.uso, i.tipo, i.ambientes, i.latitud, i.longitud,i.precio,i.superficie,i.propietario_Id, i.estado, p.nombre, p.apellido FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id";
            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Inmueble i = new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        direccion = reader.GetString(1),
                        uso = reader.GetInt32(2),
                        tipo = reader.GetInt32(3),
                        ambientes = reader.GetInt32(4),
                        latitud = reader.GetDecimal(5),
                        longitud = reader.GetDecimal(6),
                        precio = reader.GetDecimal(7),
                        superficie = reader.GetDecimal(8),
                        propietarioId = reader.GetInt32(9),
                        estado = reader.GetBoolean(10),


                        duenio = new Propietario
                        {
                            Id = reader.GetInt32(9),
                            Nombre = reader.GetString(11),
                            Apellido = reader.GetString(12),
                        }
                    };

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
            // var sql = "SELECT * FROM inmueble WHERE id = @id, from inmueble JOIN propietario ON inmueble.propietarioId = propietario.id WHERE inmueble.id = @id";

            var sql = @"SELECT i.direccion, i.uso, i.tipo, i.ambientes, i.latitud, i.longitud, i.precio,i.superficie, i.propietario_Id, i.estado, p.nombre, p.apellido,p.telefono,p.email FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id WHERE i.id = @id";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    i = new Inmueble
                    {
                        Id = id,
                        direccion = reader.GetString(0),
                        uso = reader.GetInt32(1),
                        tipo = reader.GetInt32(2),
                        ambientes = reader.GetInt32(3),
                        latitud = reader.GetDecimal(4),
                        longitud = reader.GetDecimal(5),
                        precio = reader.GetDecimal(6),
                        superficie = reader.GetDecimal(7),
                        propietarioId = reader.GetInt32(8),
                        estado = reader.GetBoolean(9),

                        duenio = new Propietario
                        {
                            Id = reader.GetInt32(8),
                            Nombre = reader.GetString(10),
                            Apellido = reader.GetString(11),
                            Telefono = reader.GetString(12),
                            Email = reader.GetString(13),
                        }
                    };
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
            var sql = @"UPDATE inmueble SET direccion = @direccion, uso = @uso, tipo = @tipo, ambientes = @ambientes, latitud = @latitud, longitud = @longitud, precio = @precio, superficie = @superficie, estado = @estado, propietario_Id = @propietario_Id WHERE id = @id";


            var command = new MySqlCommand(sql, conn);


            command.Parameters.AddWithValue("@direccion", i.direccion);
            command.Parameters.AddWithValue("@uso", i.uso);
            command.Parameters.AddWithValue("@tipo", i.tipo);
            command.Parameters.AddWithValue("@ambientes", i.ambientes);
            command.Parameters.AddWithValue("@latitud", i.latitud);
            command.Parameters.AddWithValue("@longitud", i.longitud);
            command.Parameters.AddWithValue("@precio", i.precio);
            command.Parameters.AddWithValue("@superficie", i.superficie);
            command.Parameters.AddWithValue("@estado", i.estado);
            command.Parameters.AddWithValue("@propietario_Id", i.propietarioId);
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
            var sql = @"DELETE FROM inmueble WHERE id = @id";

            var command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            conn.Open();
            res = command.ExecuteNonQuery();
            conn.Close();
        }
        return res;
    }

    public void noOfertar(int id)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE inmueble SET estado = 'Ocupado' WHERE id = @id";
            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }


    }

    public List<Inmueble> ObtenerTodosDisponibles()
    {
        List<Inmueble> listaInmuebles = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {

            var sql = @"SELECT i.id, i.direccion, i.uso, i.tipo, i.ambientes, i.latitud, i.longitud,i.precio,i.superficie,i.propietario_Id, i.estado, p.nombre, p.apellido FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id WHERE i.estado = 1 ";
            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Inmueble i = new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        direccion = reader.GetString(1),
                        uso = reader.GetInt32(2),
                        tipo = reader.GetInt32(3),
                        ambientes = reader.GetInt32(4),
                        latitud = reader.GetDecimal(5),
                        longitud = reader.GetDecimal(6),
                        precio = reader.GetDecimal(7),
                        superficie = reader.GetDecimal(8),
                        propietarioId = reader.GetInt32(9),
                        estado = reader.GetBoolean(10),


                        duenio = new Propietario
                        {
                            Id = reader.GetInt32(9),
                            Nombre = reader.GetString(11),
                            Apellido = reader.GetString(12),
                        }
                    };

                    listaInmuebles.Add(i);
                }
                conn.Close();
            }

        }
        return listaInmuebles;

    }


    public List<Inmueble> ObtenerTodosInmueblesPropietario(int id)
    {
        List<Inmueble> listaInmuebles = new List<Inmueble>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT i.id, i.direccion, i.uso, i.tipo, i.ambientes, i.latitud, i.longitud,i.precio,i.superficie,i.propietario_Id, i.estado, p.nombre, p.apellido FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id WHERE i.propietario_Id = @id";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Inmueble i = new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        direccion = reader.GetString(1),
                        uso = reader.GetInt32(2),
                        tipo = reader.GetInt32(3),
                        ambientes = reader.GetInt32(4),
                        latitud = reader.GetDecimal(5),
                        longitud = reader.GetDecimal(6),
                        precio = reader.GetDecimal(7),
                        superficie = reader.GetDecimal(8),
                        propietarioId = reader.GetInt32(9),
                        estado = reader.GetBoolean(10),

                        duenio = new Propietario
                        {
                            Id = reader.GetInt32(9),
                            Nombre = reader.GetString(11),
                            Apellido = reader.GetString(12),
                        }
                    };

                    listaInmuebles.Add(i);
                }
                conn.Close();
            }

        }
        return listaInmuebles;

    }


}



