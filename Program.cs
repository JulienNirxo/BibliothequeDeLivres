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
            Console.WriteLine("1. Récupérer tous les livres");
            Console.WriteLine("2. Ajouter un livre");
            Console.WriteLine("3. Modifier un livre");
            Console.WriteLine("4. Supprimer un livre");
            Console.WriteLine("5. Quitter");
            Console.Write("Choisissez une option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GetBooks();
                    break;
                case "2":
                    AddBook();
                    break;
                case "3":
                    UpdateBooks();
                    break;
                case "4":
                    DeleteBooks();
                    break;
                case "5":
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

    static void UpdateBooks()
    {
        Console.Write("Id du livre: ");
        var id = Console.ReadLine();
        Console.WriteLine("Vous voulez changer quel élément ?");
        Console.WriteLine("1. Nom du livre");
        Console.WriteLine("2. Auteur");
        Console.WriteLine("3. Description");
        Console.WriteLine("4. Quitter");
        Console.Write("Choisissez une option: ");
        var choice = Console.ReadLine();

        using (OracleConnection con = new OracleConnection(connectionString))
        {
            con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {

                switch (choice)
                {
                    case "1":
                        Console.Write("Modifier le nom: ");
                        string name = Console.ReadLine();
                        cmd.CommandText = "UPDATE Books set name = :name WHERE Id = :Id";
                        cmd.Parameters.Add(new OracleParameter("name", name));
                        cmd.Parameters.Add(new OracleParameter("Id", id));
                        break;
                    case "2":
                        Console.Write("Modifier l'auteur: ");
                        string author = Console.ReadLine();
                        cmd.CommandText = "UPDATE Books set author = :author WHERE Id = :Id";
                        cmd.Parameters.Add(new OracleParameter("author", author));
                        cmd.Parameters.Add(new OracleParameter("Id", id));
                        break;
                    case "3":
                        Console.Write("Modifier la description : ");
                        string description = Console.ReadLine();
                        cmd.CommandText = "UPDATE Books set description = :description WHERE Id = :Id";
                        cmd.Parameters.Add(new OracleParameter("description", description));
                        cmd.Parameters.Add(new OracleParameter("Id", id));

                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Option invalide. Veuillez réessayer.");
                        break;
                }
                cmd.ExecuteNonQuery();
            }
        }


        Console.WriteLine("Livre modifié avec succès !");
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
