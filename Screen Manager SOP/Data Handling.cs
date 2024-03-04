using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Manager_SOP
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }

        // Constructor and any necessary methods
    }
    public class UserRepository
    {
        private List<User> users = new List<User>();

        public UserRepository()
        {
            // Initialize with some dummy data
            users.Add(new User { Id = 1, FirstName = "Alice", LastName = "Johnson", EmailAddress = "alice@example.com", PhoneNumber = "123-456-7890", Address = "123 Main St", Title = "Developer" });
            users.Add(new User { Id = 2, FirstName = "Bob", LastName = "Smith", EmailAddress = "bob@example.com", PhoneNumber = "098-765-4321", Address = "456 Elm St", Title = "Manager" });
            // Add more users as needed
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        // Method to add a new user
        public void AddUser(string firstName, string lastName, string emailAddress, string phoneNumber, string address, string title)
        {
            User newUser = new User
            {
                Id = users.Max(u => u.Id) + 1, // Assign a new ID based on the highest current ID
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
                Address = address,
                Title = title,
            };

            users.Add(newUser);
        }
    }
}

