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
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
    public class UserRepository
    {
        private List<User> users = new List<User>();

        public UserRepository()
        {
            
        }

        public List<User> GetAllUsers()
        {
            return users;
        }

        public void AddUser(string firstName, string lastName, string emailAddress, string phoneNumber, string address, string title)
        {
            int newId = users.Any() ? users.Max(u => u.Id) + 1 : 1; // Assign a new ID based on the highest current ID
            User newUser = new User
            {
                Id = newId,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
                Address = address,
                Title = title,
            };

            users.Add(newUser);
        }

        public void RemoveUser(int id)
        {
            // Find the user with the given ID
            User? userToRemove = users.Find(u => u.Id == id);

            // If a user with the given ID is found, remove them from the list
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
            }
        }
    }
}

