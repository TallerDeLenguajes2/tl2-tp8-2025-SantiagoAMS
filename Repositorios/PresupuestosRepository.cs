
using models;
using Microsoft.Data.Sqlite;
public class PresupuestosRepository
{
    private string _connectionString = "Data Source=DB/tienda.db";

    private SqliteConnection ConnectAndEnsureTable()
    {
        // Basicamente hace la conexion, y se asegura de la existencia de la tabla
        var con = new SqliteConnection(_connectionString);
        con.Open();
        string createTableQuery = "CREATE TABLE IF NOT EXISTS Presupuestos (idPresupuesto INTEGER PRIMARY KEY NOT NULL UNIQUE, NombreDestinatario TEXT (100), FechaCreacion DATE )";
        using (SqliteCommand createTableCmd = new SqliteCommand(createTableQuery, con))
        {
            createTableCmd.ExecuteNonQuery();
        }
        return con;

    }


    public int Crear(Presupuesto p)
    {
        using (var con = ConnectAndEnsureTable())
        {
            string sql = "INSERT INTO Presupuestos(NombreDestinatario, FechaCreacion) values (@nom, @fec); SELECT last_insert_rowid()";
            using (var cmd = new SqliteCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@nom", p.NombreDestinatario);
                cmd.Parameters.AddWithValue("@fec", p.FechaCreacion);
                return Convert.ToInt32(cmd.ExecuteNonQuery());
            }
        }
    }

    public List<Presupuesto> Listar(bool obtenerDetalles = false)
    {
        List<Presupuesto> ret = [];
        using (SqliteConnection con = ConnectAndEnsureTable())
        {
            string sql = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";
            using (var cmd = new SqliteCommand(sql, con))
            {
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Presupuesto p = new Presupuesto(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)
                        );
                        ret.Add(p);
                    }

                }
            }
            if (obtenerDetalles){
                foreach (var pr in ret)
                {
                    pr.Detalle = ObtenerDetalles(pr);
                }
            }
            return ret;
        }
    }

    public List<PresupuestoDetalle> ObtenerDetalles(Presupuesto presupuesto)
    {
        if (presupuesto == null)
        {
            return [];
        }
        return ObtenerDetalles(presupuesto.IdPresupuesto);
        
    }
    public List<PresupuestoDetalle> ObtenerDetalles(int idPresupuesto)
    {
        List<PresupuestoDetalle> list = [];

        using var con = ConnectAndEnsureTable();
        string sql = "SELECT pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad FROM Productos AS pr, PresupuestosDetalle AS pd WHERE pd.idPresupuesto = @idpres AND pr.idProducto = pd.idProducto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@idpres", idPresupuesto);
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                list.Add(new PresupuestoDetalle(
                    new Producto(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetDouble(2)
                    ), reader.GetInt32(3)
                )
                );
            }
        }
        return list;
    }

    public Presupuesto Obtener(int idPresupuesto)
    {
        Presupuesto ret = null;
        using (SqliteConnection con = ConnectAndEnsureTable())
        {
            string sql = "SELECT * from Presupuestos where idPresupuesto = @idpres";
            using (var cmd = new SqliteCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@idpres", idPresupuesto);
                using (SqliteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ret = new Presupuesto(
                                 reader.GetInt32(0),
                                 reader.GetString(1),
                                 reader.GetString(2)
                             );
                    }
                }
            }
            if (ret == null){
                return null;
            }
            ret.Detalle = ObtenerDetalles(ret);
            return ret;
        }
    }

    public int Agregar(int idPresupuesto, Producto producto, int cantidad)
    {
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "INSERT INTO PresupuestosDetalle (idPresupuesto,idProducto,cantidad) VALUES(@idPre, @idProd, @cant); SELECT last_insert_rowid();";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@idPre", idPresupuesto);
        cmd.Parameters.AddWithValue("@idProd", producto.IdProducto);
        cmd.Parameters.AddWithValue("@cant", cantidad);
        return Convert.ToInt32(cmd.ExecuteNonQuery());
    }

    public bool Eliminar(int idPresupuesto)
    {
        using (SqliteConnection con = ConnectAndEnsureTable())
        {
            string sql = "DELETE FROM Presupuestos WHERE idPresupuesto = @id; DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
            using (var cmd = new SqliteCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@idPre", idPresupuesto);
                cmd.Parameters.AddWithValue("@idPre", idPresupuesto);
                cmd.ExecuteNonQuery();
                return true;
            }
        }
    }

}