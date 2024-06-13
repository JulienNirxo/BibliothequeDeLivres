using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

class Program
{
    static string connectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1. Ajouter un livre");
            Console.WriteLine("2. Récupérer tous les livres");
            Console.WriteLine("3. Supprimer un livre");
            Console.WriteLine("4. Quitter");
            Console.Write("Choisissez une option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    GetBooks();
                    break;
                case "3":
                    DeleteBooks();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Option invalide. Veuillez réessayer.");
                    break;
            }
        }
    }

    static void AddBook()
    {
        Console.Write("Nom du livre: ");
        string name = Console.ReadLine();
        Console.Write("Auteur: ");
        string author = Console.ReadLine();
        Console.Write("Description: ");
        string description = Console.ReadLine();

        using (OracleConnection con = new OracleConnection(connectionString))
        {
            con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Books (Name, Author, Description) VALUES (:name, :author, :description)";
                cmd.Parameters.Add(new OracleParameter("name", name));
                cmd.Parameters.Add(new OracleParameter("author", author));
                cmd.Parameters.Add(new OracleParameter("description", description));

                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Livre ajouté avec succès !");
    }

    static void GetBooks()
    {
        using (OracleConnection con = new OracleConnection(connectionString))
        {
            con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name, Author, Description FROM Books";
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]}, Nom: {reader["Name"]}, Auteur: {reader["Author"]}, Description: {reader["Description"]}");
                    }
                }
            }
        }
    }

    static void DeleteBooks()
    {
        Console.Write("Id du livre: ");
        string id = Console.ReadLine();

        using (OracleConnection con = new OracleConnection(connectionString))
        {
            con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Books WHERE Id = :Id";
                cmd.Parameters.Add(new OracleParameter("Id", id));

                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Livre supprimé avec succès !");
    }
}
