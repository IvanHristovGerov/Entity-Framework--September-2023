

using _01.Initial_Setup;
using System.Data.SqlClient;


//Server name DESKTOP-SF8TSQ3\SQLEXPRESS
//Database = MinionsDB

//1. Connection string
const string connectionString = @"Server=DESKTOP-SF8TSQ3\SQLEXPRESS;Database = MinionsDB;Integrated Security=True";

//2. SqlConnection
using SqlConnection sqlConnection = new SqlConnection(connectionString);
sqlConnection.Open();

//3. Create SQLCommand
using SqlCommand getVillainsCommand = new SqlCommand(SqlQueries.GetVillainsWithNumberOfMinions, sqlConnection);

//4. Data Reader
using SqlDataReader sqlDataReader = getVillainsCommand.ExecuteReader();

while(sqlDataReader.Read())
{
    Console.WriteLine($"{sqlDataReader["Name"]} - {sqlDataReader["TotalMinions"]}");
}
