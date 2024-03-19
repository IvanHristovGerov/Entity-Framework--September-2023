//1. Connection string
using System.Data.SqlClient;

const string connectionString = @"Server=DESKTOP-SF8TSQ3\SQLEXPRESS;Database = MinionsDB;Integrated Security=True";




//2. SqlConnection
using SqlConnection sqlConnection = new SqlConnection(connectionString);
sqlConnection.Open();


