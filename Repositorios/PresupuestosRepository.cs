
using models;
using interfaces;
using Microsoft.Data.Sqlite;

namespace repositorios;
public class PresupuestosRepository : IPresupuestoRepository
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


    public int Add(Presupuesto p)
    {
        using var con = ConnectAndEnsureTable();
        string sql = "INSERT INTO Presupuestos(NombreDestinatario, FechaCreacion) values (@nom, @fec); SELECT last_insert_rowid()";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@nom", p.NombreDestinatario);
        cmd.Parameters.AddWithValue("@fec", p.FechaCreacion);
        return Convert.ToInt32(cmd.ExecuteNonQuery());
    }

    public bool Update(Presupuesto p)
    {
        using var con = ConnectAndEnsureTable();
        string sql = "UPDATE Presupuestos SET NombreDestinatario = @nom WHERE idPresupuesto = @id";
        using var cmd = new SqliteCommand(sql, con);

        cmd.Parameters.AddWithValue("@nom", p.NombreDestinatario);
        cmd.Parameters.AddWithValue("@id", p.IdPresupuesto);
        //cmd.Parameters.AddWithValue("@fec", p.FechaCreacion);
        cmd.ExecuteNonQuery();
        return true;
    }

    

    public List<Presupuesto> GetAll(bool obtenerDetalles = true)
    {
        List<Presupuesto> ret = [];
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";
        using (var cmd = new SqliteCommand(sql, con))
        {
            using SqliteDataReader reader = cmd.ExecuteReader();
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
        if (obtenerDetalles)
        {
            foreach (var pr in ret)
            {
                pr.Detalle = GetDetails(pr);
            }
        }
        return ret;
    }

    public List<PresupuestoDetalle> GetDetails(Presupuesto presupuesto)
    {
        if (presupuesto == null)
        {
            return [];
        }
        return GetDetails(presupuesto.IdPresupuesto);
        
    }
    public List<PresupuestoDetalle> GetDetails(int idPresupuesto)
    {
        List<PresupuestoDetalle> list = [];

        using var con = ConnectAndEnsureTable();
        string sql = "SELECT pd.id, pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad FROM Productos AS pr, PresupuestosDetalle AS pd WHERE pd.idPresupuesto = @idpres AND pr.idProducto = pd.idProducto";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@idpres", idPresupuesto);
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                list.Add(new PresupuestoDetalle(
                    reader.GetInt32(0),
                    new Producto(
                        reader.GetInt32(1),
                        reader.GetString(2),
                        reader.GetDouble(3)
                    )
                    , idPresupuesto
                    , reader.GetInt32(4)
                )
                );
            }
        }
        return list;
    }

    public Presupuesto Get(int idPresupuesto)
    {
        Presupuesto ret = null;
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "SELECT * from Presupuestos where idPresupuesto = @idpres";
        using (var cmd = new SqliteCommand(sql, con))
        {
            cmd.Parameters.AddWithValue("@idpres", idPresupuesto);
            using SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ret = new Presupuesto(
                         reader.GetInt32(0),
                         reader.GetString(1),
                         reader.GetString(2)
                     );
            }
        }
        if (ret == null)
        {
            return null;
        }
        ret.Detalle = GetDetails(ret);
        return ret;
    }

    public int AddProducto(int idPresupuesto, Producto producto, int cantidad)
    {
        if (producto == null){
            return -1;
        }
        return AddProducto(idPresupuesto, producto.IdProducto, cantidad);
    }
    public int AddProducto(int idPresupuesto, int idProducto, int cantidad){
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "INSERT INTO PresupuestosDetalle (idPresupuesto,idProducto,cantidad) VALUES(@idPre, @idProd, @cant); SELECT last_insert_rowid();";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@idPre", idPresupuesto);
        cmd.Parameters.AddWithValue("@idProd", idProducto);
        cmd.Parameters.AddWithValue("@cant", cantidad);
        return Convert.ToInt32(cmd.ExecuteNonQuery());
    }

    

    public int UpdateDetail(PresupuestoDetalle pd)
    {
        using var con = ConnectAndEnsureTable();

        string sql;
        if (pd.Cantidad == 0)
        {
            sql = "DELETE FROM PresupuestosDetalles WHERE id = @id; SELECT last_insert_rowid()";
        }
        else
        {
            sql = "UPDATE PresupuestosDetalles SET cantidad = @cant WHERE id = @id; SELECT last_insert_rowid()";
        }
        
        using var cmd = new SqliteCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", pd.Id);
        cmd.Parameters.AddWithValue("@cant", pd.Cantidad);
        return Convert.ToInt32(cmd.ExecuteNonQuery());
    }

    public bool Delete(int idPresupuesto)
    {
        using SqliteConnection con = ConnectAndEnsureTable();
    
        string sql = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id;DELETE FROM Presupuestos WHERE idPresupuesto = @id";
        using var cmd = new SqliteCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", idPresupuesto);
        cmd.ExecuteNonQuery();
        return true;
            
    }

}