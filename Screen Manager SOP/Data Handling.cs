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
    }
}

