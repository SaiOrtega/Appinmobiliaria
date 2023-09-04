using System.Data;
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
            VALUES (@id, @inmueble_id, @inquilino_id, @fecha_inicio, @fecha_fin, @monto_alquiler);
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
            @"SELECT c.id, c.inmueble_id, c.inquilino_id, c.fecha_inicio, c.fecha_fin, c.monto_alquiler, inmu.Id, inmu.direccion,inmu.uso,inmu.estado,inqui.Dni,inqui.Nombre, inqui.Apellido FROM contrato c JOIN inmueble inmu ON c.inmueble_id = i.id JOIN inquilino inqui ON inqui.Id = c.inquilino_id ORDER BY c.FechaFinal DESC";

            using (var command = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
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
            }
            conn.Clone();
        }
        return Contratos;
    }

    public Contrato ObtenerUno(int id)
    {
        Contrato contrato = new Contrato();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            var sql =
             @"SELECT c.id, c.inmueble_id, c.inquilino_id, c.fecha_inicio, c.fecha_fin, c.monto_alquiler, inmu.Id, inmu.direccion,inmu.uso,inmu.estado,inqui.Dni,inqui.Nombre, inqui.Apellido FROM contrato c JOIN inmueble inmu ON c.inmueble_id = i.id JOIN inquilino inqui ON inqui.Id = c.inquilino_id ORDER BY c.FechaFinal DESC";

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
                            Id = reader.GetInt32(1),
                            InmuebleId = reader.GetInt32(2),
                            InquilinoId = reader.GetInt32(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaFinal = reader.GetDateTime(5),
                            MontoMensual = reader.GetDecimal(6),
                            inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(7),
                                direccion = reader.GetString(8),
                                uso = reader.GetInt32(9),
                                estado = reader.GetBoolean(10)
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



}