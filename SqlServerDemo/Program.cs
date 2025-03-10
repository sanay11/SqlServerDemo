// See https://aka.ms/new-console-template for more information

using System;
using System.Data.SqlClient; // Or Microsoft.Data.SqlClient if using that package

namespace SqlServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Define your connection string
            //    Replace YOUR_SERVER_NAME, YOUR_DATABASE_NAME, etc.
            string connectionString =
                "Data Source=LAPTOP-TO21FNM6;Initial Catalog = TestDB;Integrated Security=True";

            // 2. Create the SQL statements
            //    a) Create table statement
            string createTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
                CREATE TABLE Employees
                (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100),
                    Position NVARCHAR(100),
                    Salary DECIMAL(18, 2)
                );
            ";

            //    b) Insert statement (using parameters)
            string insertSql = @"
                INSERT INTO Employees (Name, Position, Salary)
                VALUES (@Name, @Position, @Salary);
            ";

            // 3. Execute the "Create Table" command
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmdCreate = new SqlCommand(createTableSql, conn);

                try
                {
                    conn.Open();
                    cmdCreate.ExecuteNonQuery();
                    Console.WriteLine("Table 'Employees' created (if it did not already exist).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating table: " + ex.Message);
                }
            }

            // 4. Insert sample data
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmdInsert = new SqlCommand(insertSql, conn);

                // Use parameters to avoid SQL injection
                cmdInsert.Parameters.AddWithValue("@Name", "Sanay");
                cmdInsert.Parameters.AddWithValue("@Position", "Software Engineer");
                cmdInsert.Parameters.AddWithValue("@Salary", 30000.00m);

                try
                {
                    conn.Open();
                    int rowsAffected = cmdInsert.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Data inserted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No rows were inserted.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inserting data: " + ex.Message);
                }
            }

            // 5. (Optional) Read data back from the table
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string selectSql = "SELECT Id, Name, Position, Salary FROM Employees;";
                SqlCommand cmdSelect = new SqlCommand(selectSql, conn);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmdSelect.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Reading columns by index or column name
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string position = reader.GetString(2);
                            decimal salary = reader.GetDecimal(3);

                            Console.WriteLine($"Id: {id}, Name: {name}, Position: {position}, Salary: {salary}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading data: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
