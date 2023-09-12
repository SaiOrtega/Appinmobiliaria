using System.Data;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;

namespace AppInmobiliaria.Models;

public class RepoContratos
{
    String connectionString = "Server=localhost;Database=imOrtega;User=root;Password=;SslMode=none";

    public RepoContratos()
    {

    }

    public int Alta(Contrato contrato)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO contrato 
            (inmueble_id, inquilino_id, fecha_inicio, fecha_fin, monto_alquiler) 
            VALUES (@inmueble_id, @inquilino_id, @fecha_inicio, @fecha_fin, @monto_alquiler);
            SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, conn))
            {
                // command.CommandType = CommandType.Text;               
                command.Parameters.AddWithValue("@inmueble_id", contrato.InmuebleId);
                command.Parameters.AddWithValue("@inquilino_id", contrato.InquilinoId);
                command.Parameters.AddWithValue("@fecha_inicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@fecha_fin", contrato.FechaFinal);
                command.Parameters.AddWithValue("@monto_alquiler", contrato.MontoMensual);
                conn.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                contrato.Id = res;
                conn.Close();
            }
        }
        return res;
    }


    public List<Contrato> ObtenerTodos()
    {
        List<Contrato> Contratos = new List<Contrato>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            //var sql = "SELECT * FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id";
            var sql =
            @"SELECT c.id, c.inmueble_id, c.inquilino_id, c.fecha_inicio, c.fecha_fin, c.monto_alquiler, inm.Id, inm.direccion,inm.uso,inm.estado,inq.Dni,inq.Nombre, inq.Apellido FROM contrato c JOIN inmueble inm ON c.inmueble_id = inm.id JOIN inquilino inq ON inq.Id = c.inquilino_id ORDER BY c.fecha_fin DESC";

            //SELECT `id`, `inmueble_id`, `inquilino_id`, `fecha_inicio`, `fecha_fin`, `monto_alquiler` contrato
            //SELECT `id`, `dni`, `nombre`, `apellido`, `direccion`, `telefono`, `email`, `nacimiento` propietario o inquilino
            //SELECT `id`, `direccion`, `ambientes`, `latitud`, `longitud`, `precio`, `superficie`, `estado`, `propietario_id`, `uso`, `tipo` FROM `inmueble`

            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Contrato contrato = new Contrato
                            {
                                Id = reader.GetInt32(0),
                                InmuebleId = reader.GetInt32(1),
                                InquilinoId = reader.GetInt32(2),
                                FechaInicio = reader.GetDateTime(3),
                                FechaFinal = reader.GetDateTime(4),
                                MontoMensual = reader.GetDecimal(5),

                                inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(6),
                                    direccion = reader.GetString(7),
                                    uso = reader.GetInt32(8),
                                    estado = reader.GetBoolean(9)
                                },
                                inquilino = new Inquilino
                                {
                                    Dni = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    Apellido = reader.GetString(12)
                                }

                            };
                            Contratos.Add(contrato);
                        }
                    }
                    conn.Close();
                }
            }

        }
        return Contratos;
    }





    public Contrato ObtenerUno(int id)
    {
        Contrato contrato = new Contrato();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql =
             @"SELECT c.id, c.inmueble_id, c.inquilino_id, c.fecha_inicio, c.fecha_fin, c.monto_alquiler,inmu.Id,inmu.direccion,inmu.uso,inmu.estado,inqui.Id,inqui.Dni,inqui.Nombre, inqui.Apellido FROM contrato c JOIN inmueble inmu ON c.inmueble_id = inmu.id JOIN inquilino inqui ON inqui.Id = c.inquilino_id where c.id = @id  ORDER BY c.fecha_fin DESC ";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = command.ExecuteReader();
                {
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            InmuebleId = reader.GetInt32(1),
                            InquilinoId = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
                            FechaFinal = reader.GetDateTime(4),
                            MontoMensual = reader.GetDecimal(5),
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(6),
                                direccion = reader.GetString(7),
                                uso = reader.GetInt32(8),
                                estado = reader.GetBoolean(9)
                            },
                            inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(10),
                                Dni = reader.GetString(11),
                                Nombre = reader.GetString(12),
                                Apellido = reader.GetString(13)
                            }
                        };
                    }

                }
            }
            conn.Close();
        }
        return contrato;
    }

    public int Actualizar(Contrato contrato)
    {
        int res = 0;
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE contrato SET inmueble_id=@InmuebleId,inquilino_id=@InquilinoId,fecha_inicio=@FechaInicio,fecha_fin=@FechaFinal,monto_alquiler=@MontoMensual, where id = @id";

            using (var command = new MySqlCommand(sql, conn))
            {
                command.Parameters.AddWithValue("@id", contrato.Id);
                command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@FechaFinal", contrato.FechaFinal);
                command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);

                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }
        }
        return res;
    }

    [Authorize(Policy = "Administrador")]
    public int EliminarContrato(int id)
    {
        int res = 0;

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql = @"DELETE FROM contrato WHERE id = @id";

            var command = new MySqlCommand(sql, conn);
            {
                command.Parameters.AddWithValue("@id", id);
                conn.Open();
                res = command.ExecuteNonQuery();
                conn.Close();
            }

        }
        return res;
    }

    public List<Contrato> ObtenerTodosVigentes()
    {
        List<Contrato> contratos = new List<Contrato>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            //var sql = "SELECT * FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id";
            var sql =
            @"SELECT c.id, c.inmueble_id, c.inquilino_id, c.fecha_inicio, c.fecha_fin, c.monto_alquiler, inmu.Id, inmu.direccion,inmu.uso,inmu.estado,inqui.Dni,inqui.Nombre, inqui.Apellido FROM contrato c JOIN inmueble inmu ON c.inmueble_id = inmu.id JOIN inquilino inqui ON inqui.Id = c.inquilino_id WHERE YEAR(c.fecha_fin) >= YEAR(NOW()) AND  MONTH(c.fecha_fin) >= MONTH(NOW()) AND DAY(c.fecha_fin) >= DAY(NOW()) ORDER BY fecha_fin ASC";

            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Contrato contrato = new Contrato
                            {
                                Id = reader.GetInt32(0),
                                InmuebleId = reader.GetInt32(1),
                                InquilinoId = reader.GetInt32(2),
                                FechaInicio = reader.GetDateTime(3),
                                FechaFinal = reader.GetDateTime(4),
                                MontoMensual = reader.GetDecimal(5),

                                inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(6),
                                    direccion = reader.GetString(7),
                                    uso = reader.GetInt32(8),
                                    estado = reader.GetBoolean(9)
                                },
                                inquilino = new Inquilino
                                {
                                    Dni = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    Apellido = reader.GetString(12)
                                }

                            };
                            contratos.Add(contrato);
                        }
                    }
                    conn.Close();
                }
            }

        }
        return contratos;
    }


    public List<Contrato> ObtenerTodosVencidos()
    {
        List<Contrato> contratos = new List<Contrato>();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            //var sql = "SELECT * FROM inmueble i JOIN propietario p ON i.propietario_Id = p.id";
            var sql =
            @"SELECT c.id, c.inmueble_id, c.inquilino_id, c.fecha_inicio, c.fecha_fin, c.monto_alquiler, inmu.Id, inmu.direccion,inmu.uso,inmu.estado,inqui.Dni,inqui.Nombre, inqui.Apellido FROM contrato c JOIN inmueble inmu ON c.inmueble_id = inmu.id JOIN inquilino inqui ON inqui.Id = c.inquilino_id WHERE YEAR(c.fecha_fin) <= YEAR(NOW()) AND  MONTH(c.fecha_fin) <= MONTH(NOW()) AND DAY(c.fecha_fin) < DAY(NOW()) ORDER BY fecha_fin ASC";

            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Contrato contrato = new Contrato
                            {
                                Id = reader.GetInt32(0),
                                InmuebleId = reader.GetInt32(1),
                                InquilinoId = reader.GetInt32(2),
                                FechaInicio = reader.GetDateTime(3),
                                FechaFinal = reader.GetDateTime(4),
                                MontoMensual = reader.GetDecimal(5),

                                inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(6),
                                    direccion = reader.GetString(7),
                                    uso = reader.GetInt32(8),
                                    estado = reader.GetBoolean(9)
                                },
                                inquilino = new Inquilino
                                {
                                    Dni = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    Apellido = reader.GetString(12)
                                }

                            };
                            contratos.Add(contrato);
                        }
                    }
                    conn.Close();
                }
            }

        }
        return contratos;
    }

    ///falta validar

    public int verificarPosibilidad(DateTime? inicial, DateTime? final, int? idInm)
    {
        int contador = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            //SELECT `id`, `inmueble_id`, `inquilino_id`, `fecha_inicio`, `fecha_fin`, `monto_alquiler` contrato
            var sql = @"SELECT COUNT(*) FROM contrato 
            WHERE inmueble_id = @inmId  AND ((@FechaInicio BETWEEN contrato.fecha_inicio AND contrato.fecha_fin) OR (@FechaFinal BETWEEN contrato.fecha_inicio AND contrato.fecha_fin))";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@inmId", idInm);
                command.Parameters.AddWithValue("@FechaInicio", inicial);
                command.Parameters.AddWithValue("@FechaFinal", final);

                connection.Open();
                contador = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return contador;


    }
    public int TraerContratoEValido(DateTime? inicial, DateTime? final, int? inmId, int? contratoId)
    {
        int cont = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT COUNT(*) FROM contrato 
            WHERE inmueble_id = @inmId  AND(Id != @contratoId)AND ((@FechaInicio BETWEEN contrato.fecha_inicio AND contrato.fecha_fin) OR (@FechaFinal BETWEEN contrato.fecha_inicio AND contrato.fecha_fin))";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@inmId", inmId);
                command.Parameters.AddWithValue("@FechaInicio", inicial);
                command.Parameters.AddWithValue("@FechaFinal", final);
                command.Parameters.AddWithValue("@contratoId", contratoId);
                connection.Open();
                cont = Convert.ToInt32(command.ExecuteScalar());
            }

        }

        return cont;

    }








}