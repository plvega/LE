using BlogDataLibrary.Data;
using BlogDataLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using BlogDataLibrary.Database;

namespace BlogTestUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlData db = GetConnection();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Blog Application!");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Add Post");
                Console.WriteLine("4. List Post");
                Console.WriteLine("5. Show Post Details");
                Console.WriteLine("6. Exit");

                Console.Write("Enter your choice (1-6): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register(db);
                        break;
                    case "2":
                        Authenticate(db);
                        break;
                    case "3":
                        AddPosts(db);
                        break;
                    case "4":
                        ListPosts(db);
                        break;
                    case "5":
                        ShowPostDetails(db);
                        break;
                    case "6":
                        running = false;
                        Console.WriteLine("Exiting the application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        private static UserModel GetCurrentUser(SqlData db)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            UserModel user = db.Authenticate(username, password);

            return user;
        }

        static SqlData GetConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();
            ISqlDataAccess dbAccess = new SqlDataAccess(config);
            SqlData db = new SqlData(dbAccess);

            return db;
        }

        public static void Register(SqlData db)
        {
            Console.WriteLine("Enter new username: ");
            var username = Console.ReadLine();

            Console.WriteLine("Enter new First Name: ");
            var firstName = Console.ReadLine();

            Console.WriteLine("Enter new Last Name: ");
            var lastName = Console.ReadLine();

            Console.WriteLine("Enter new Password: ");
            var password = Console.ReadLine();

            db.Register(username, firstName, lastName, password);

            Console.WriteLine("Registration successful!");
        }

        public static void Authenticate(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("Invalid Credentials.");
            }
            else
            {
                Console.WriteLine($"Welcome, {user.UserName}");
            }
        }

        private static void AddPosts(SqlData db)
        {
            UserModel user = GetCurrentUser(db);

            if (user == null)
            {
                Console.WriteLine("User not authenticated. Unable to add post.");
                return;
            }

            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.WriteLine("Write Body: ");
            string body = Console.ReadLine();

            PostModel post = new PostModel
            {
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
                UserId = user.Id
            };

            db.AddPost(post);

            Console.WriteLine("Post added successfully!");
        }

        private static void ListPosts(SqlData db)
        {
            List<ListPostModel> posts = db.ListPosts();

            if (posts.Count == 0)
            {
                Console.WriteLine("No posts available.");
                return;
            }

            foreach (ListPostModel post in posts)
            {
                Console.WriteLine($"{post.Id}. {post.Title} by {post.UserName} [{post.DateCreated.ToString("yyyy-MM-dd")}]");
                Console.WriteLine(post.Body.Length > 20 ? post.Body.Substring(0, 20) + "..." : post.Body);
                Console.WriteLine();
            }
        }

        private static void ShowPostDetails(SqlData db)
        {
            Console.WriteLine("Enter a post ID: ");
            int id = Int32.Parse(Console.ReadLine());


            ListPostModel post = db.ShowPostDetails(id);
            Console.WriteLine(post.Title);
            Console.WriteLine($"by {post.FirstName} {post.LastName} [{post.UserName}]");

            Console.WriteLine();

            Console.WriteLine(post.Body);

            Console.WriteLine(post.DateCreated.ToString("MMM d yyyy"));
        }

    }
}