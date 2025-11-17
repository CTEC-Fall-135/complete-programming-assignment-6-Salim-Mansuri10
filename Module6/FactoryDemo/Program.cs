using System;                 // console I/O
using System.Data.Common;     // DbConnection, DbCommand, DbDataReader
using Microsoft.Data.SqlClient; // SqlClientFactory for SQL Server

namespace FactoryDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Factory Model Example\n");

            string provider = "SqlServer";

            // 2. Connection string to the AutoLot2023 database
   
            string connectionString =
                @"Server=np:\\.\pipe\LOCALDB#4E53FCD2\tsql\query;Integrated Security=true;Initial Catalog=AutoLot2023;TrustServerCertificate=True";

            Console.WriteLine($"provider is: {provider}");
            Console.WriteLine($"connection string: {connectionString}\n");

            try
            {
                // 3. Get the provider factory
                DbProviderFactory factory = GetDbProviderFactory(provider);

                // 4. Create and open a database connection
                using (DbConnection connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    Console.WriteLine($"Your connection object is a: {connection.GetType().Name}");

                    // 5. Create a command object to query data
                    using (DbCommand command = factory.CreateCommand())
                    {
                        command.Connection = connection;
                        command.CommandText =
                            "SELECT i.Id, m.Name FROM dbo.Inventory i INNER JOIN dbo.Makes m ON m.Id = i.MakeId";

                        Console.WriteLine($"Your command object is a: {command.GetType().Name}");

                        // 6. Execute the command and get a data reader
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine($"Your data reader object is a: {reader.GetType().Name}\n");
                            Console.WriteLine("***** Current Inventory *****");

                            while (reader.Read())
                            {
                                Console.WriteLine($"-> Car #{reader["Id"]} is a {reader["Name"]}.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // prints the error instead of silently dying
                Console.WriteLine("\n*** ERROR OPENING CONNECTION OR RUNNING QUERY ***");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nPress any key to close this window...");
            Console.ReadKey();
        }

        // Return the SQL Server provider factory
        static DbProviderFactory GetDbProviderFactory(string providerName)
        {
            if (providerName == "SqlServer")
            {
                return SqlClientFactory.Instance;
            }

            throw new NotSupportedException("Only SqlServer is required for this assignment.");
        }
    }
}