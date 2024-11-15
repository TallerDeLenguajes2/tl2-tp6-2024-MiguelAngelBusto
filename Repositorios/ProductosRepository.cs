using System.Collections.Generic;
using System.Linq;
using tl2_tp6_2024_MiguelAngelBusto.Models;
using Microsoft.Data.Sqlite;

namespace tl2_tp6_2024_MiguelAngelBusto.Repositorios;

    public class ProductosRepository
    {
        private string connectionString = "Data Source=db\\Tienda.db;";
        
        public void Create(Productos item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string insertQuery = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio);";
                SqliteCommand command = new SqliteCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Descripcion", item.Descripcion);
                command.Parameters.AddWithValue("@Precio", item.Precio);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Update(int id, Productos item)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string updateQuery = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @IdProducto;";
                SqliteCommand command = new SqliteCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Descripcion", item.Descripcion);
                command.Parameters.AddWithValue("@Precio", item.Precio);
                command.Parameters.AddWithValue("@IdProducto", id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public List<Productos> GetProductos()
        {
            List<Productos> productos = new List<Productos>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string queryString = "SELECT idProducto, Descripcion, Precio FROM Productos;";
                var command = new SqliteCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Productos producto = new Productos();
                        producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                        producto.Descripcion = reader["Descripcion"].ToString();
                        producto.Precio = Convert.ToInt32(reader["Precio"]);
                        productos.Add(producto);
                    }
                    connection.Close();
                }

            }
            return productos;
        }



    public Productos GetByID(int id) {
        Productos producto = null;
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            string query = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @IdProducto;";
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@IdProducto", id);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read()) // Solo lee un registro
                {
                    producto = new Productos
                    {
                        IdProducto = Convert.ToInt32(reader["idProducto"]),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToInt32(reader["Precio"])
                    };
                }
            }
            connection.Close();
        }
        return producto;
    }

        public void Delete(int id)
    {
    try {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            string deleteQuery = "DELETE FROM Productos WHERE idProducto = @IdProducto;";
            string delete2query = "DELETE FROM PresupuestosDetalle WHERE idProducto = @IdProducto;";
            SqliteCommand command = new SqliteCommand(delete2query, connection);
            command.Parameters.AddWithValue("@IdProducto", id);

            connection.Open();
            command.ExecuteNonQuery();
            command = new SqliteCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@IdProducto", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
    {
        throw new InvalidOperationException("No se puede eliminar el producto porque tiene registros dependientes en otras tablas.");
    }    
    }
}
