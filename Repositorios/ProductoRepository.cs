using models;
using interfaces;
using Microsoft.Data.Sqlite;

public class ProductoRepository : IProductoRepository
{
    private string _connectionString = "Data Source=DB/tienda.db";

    private SqliteConnection ConnectAndEnsureTable()
    {
        // Basicamente hace la conexion, y se asegura de la existencia de la tabla
        var con = new SqliteConnection(_connectionString);
        con.Open();
        string createTableQuery = "CREATE TABLE IF NOT EXISTS Productos (idProducto INTEGER PRIMARY KEY UNIQUE NOT NULL, Descripcion TEXT(100), precio NUMERIC(10,2))";
        using (SqliteCommand createTableCmd = new SqliteCommand(createTableQuery, con))
        {
            createTableCmd.ExecuteNonQuery();
        }
        return con;

    }

    public int Add(Producto p)
    {
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES(@desc, @prec); SELECT last_insert_rowid();";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@prec", p.Precio);
        int id = Convert.ToInt32(cmd.ExecuteScalar());
        return id;
    }


    public void Update(int id, Producto p)
    {
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "UPDATE Productos SET Descripcion = @desc, Precio = @prec WHERE idProducto = @id";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion);
        cmd.Parameters.AddWithValue("@prec", p.Precio);
        cmd.Parameters.AddWithValue("@id", p.IdProducto);
        cmd.ExecuteNonQuery();
    }
    public List<Producto> GetAll()
    {
        List<Producto> ret = [];
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "SELECT idProducto, Descripcion, Precio FROM Productos";
        using (var cmd = new SqliteCommand(sql, con))
        {
            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Producto p = new Producto(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetDouble(2)
                );
                ret.Add(p);
            }
        }
        return ret;
    }

    public Producto Get(int id)
    {
        string sql = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @id";
        return _Obtener(sql, id, true);
    }
    public Producto Get(string descripcion, bool like=false)
    {
        string sql = $"SELECT idProducto, Descripcion, Precio FROM Productos WHERE Descripcion {(like ? "LIKE %@desc%" : "= @desc")}";
        return _Obtener(sql, descripcion, false);
    }
    private Producto _Obtener(string sql, object arg, bool byId = true)
    {
        using SqliteConnection con = ConnectAndEnsureTable();
        using var cmd = new SqliteCommand(sql, con);

        cmd.Parameters.AddWithValue(byId ? "@id" : "@desc", arg);
        using SqliteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            return new Producto(
                reader.GetInt32(0),
                reader.GetString(reader.GetOrdinal("Descripcion")),
                reader.GetDouble(2)
            );
        }
        return null;
    }

    public void Delete(int id)
    {
        using SqliteConnection con = ConnectAndEnsureTable();
        string sql = "DELETE from Productos where idProducto = @id";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
