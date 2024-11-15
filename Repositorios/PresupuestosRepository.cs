using System.Collections.Generic;
using System.Linq;
using tl2_tp6_2024_MiguelAngelBusto.Models;
using Microsoft.Data.Sqlite;

namespace tl2_tp6_2024_MiguelAngelBusto.Repositorios;


public class PresupuestosRepository
{

    private string connectionString = "Data Source=db\\Tienda.db;";

    public void Create(Presupuestos item)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            string insertQuery = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion);";
            SqliteCommand command = new SqliteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@NombreDestinatario", item.NombreDestinatario);
            command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
            connection.Open();
            command.ExecuteNonQuery();
            string insertQuery2 = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto,Cantidad) VALUES (@idPresupuesto,@idProducto,@Cantidad);";
            SqliteCommand command2 = new SqliteCommand(insertQuery2, connection);
            command2.Parameters.AddWithValue("@idPresupuesto", item.IdPresupuesto);
            foreach (PresupuestoDetalle detalle in item.Detalle)
            {
                command2.Parameters.AddWithValue("@idPresupuesto", item.IdPresupuesto);
                command2.Parameters.AddWithValue("@idProducto", detalle.Productos.IdProducto);
                command2.Parameters.AddWithValue("@idCantidad", detalle.Cantidad);
                command2.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public Presupuestos GetByID(int id)
    {
        Presupuestos presupuesto = null;
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            string query = "SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos WHERE idPresupuesto = @IdPresupuesto;";
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@IdPresupuesto", id);

            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    presupuesto = new Presupuestos
                    {
                        IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                        NombreDestinatario = reader["NombreDestinatario"].ToString(),
                        FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                        Detalle = new List<PresupuestoDetalle>() // Inicializa la lista de detalles
                    };
                }
            }
            connection.Close();
        }
        return presupuesto;
    }

    public void AddProcutByID(int idPresupuesto, int idProducto, int cantidad)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            string insertQuery = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad);";
            SqliteCommand command = new SqliteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.Parameters.AddWithValue("@Cantidad", cantidad);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Presupuestos> GetPresupuestos()
        {
            List<Presupuestos> productos = new List<Presupuestos>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string queryString = "SELECT * FROM Presupuestos;";
                var command = new SqliteCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Presupuestos pre = new Presupuestos();
                        pre.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                        pre.NombreDestinatario = reader["NombreDestinatario"].ToString();
                        pre.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);

                        pre.setDetallesPresupuesto(
                            this.GetPresupuestosDetalles(Convert.ToInt32(reader["idPresupuesto"]))
                        );

                        productos.Add(pre);
                     }
                    connection.Close();
                }

            }
            return productos;
        }


    private List<PresupuestoDetalle> GetPresupuestosDetalles(int idPresupuesto) {
        List<PresupuestoDetalle> pdList = new List<PresupuestoDetalle>();

        try
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString)) 
            {
                string queryString = @"SELECT 
                    Productos.idProducto,
                    Productos.Descripcion,
                    Productos.Precio,
                    PresupuestosDetalle.Cantidad
                FROM 
                    Presupuestos
                LEFT JOIN 
                    PresupuestosDetalle ON Presupuestos.idPresupuesto = PresupuestosDetalle.idPresupuesto
                LEFT JOIN 
                    Productos ON PresupuestosDetalle.idProducto = Productos.idProducto
                WHERE 
                    Presupuestos.idPresupuesto = @idPresupuesto;";

                using (SqliteCommand command = new SqliteCommand(queryString, connection)) {
                    command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                    connection.Open();
                    using (SqliteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            PresupuestoDetalle pd = new PresupuestoDetalle();

                            if(reader.IsDBNull(reader.GetOrdinal("idProducto"))) {
                                return pdList;
                            }

                            Productos product = new Productos();
                            product.IdProducto = Convert.ToInt32(reader["idProducto"]);
                            product.Descripcion = reader["Descripcion"].ToString();
                            product.Precio = Convert.ToInt32(reader["Precio"]);

                            pd.Productos = product;
                            pd.Cantidad = Convert.ToInt32(reader["Cantidad"]);

                            pdList.Add(pd);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener detalles de presupuesto: {ex.Message}");
        }
        return pdList;
    }
}

