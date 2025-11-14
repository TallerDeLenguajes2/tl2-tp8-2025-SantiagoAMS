using models;
using interfaces;
using Microsoft.Data.Sqlite;
namespace repositorios;
public class UsuarioRepository : IUserRepository
{
    private string _connectionString = "Data Source=DB/tienda.db";

    private SqliteConnection ConnectAndEnsureTable()
    {
        // Basicamente hace la conexion, y se asegura de la existencia de la tabla
        var con = new SqliteConnection(_connectionString);
        con.Open();
        string createTableQuery = @"CREATE TABLE IF NOT EXISTS 
            Usuarios (
                Id     NUMERIC PRIMARY KEY UNIQUE NOT NULL,
                Nombre TEXT NOT NULL,
                User   TEXT    UNIQUE NOT NULL,
                Pass   TEXT    NOT NULL,
                Rol    TEXT)
            ;";
        using (SqliteCommand createTableCmd = new SqliteCommand(createTableQuery, con))
        {
            createTableCmd.ExecuteNonQuery();
        }
        return con;

    }
    public Usuario GetUser(string username, string password)
    {
        
        string sql =  @"SELECT Id, Nombre, User, Pass, Rol
        FROM USUARIOS WHERE User = @user AND PASS = @pass;";
        using var con = ConnectAndEnsureTable();
        using var cmd = new SqliteCommand(sql,con);

        cmd.Parameters.AddWithValue("@user",username);
        cmd.Parameters.AddWithValue("@pass",password);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User= reader.GetString(2),
                Pass= reader.GetString(3),
                Rol= reader.GetString(4)
            };
        }
        return null;
    }
}