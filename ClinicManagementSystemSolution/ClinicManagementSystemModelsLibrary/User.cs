using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModelsLibrary
{
    public class User   //: IComparable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Type { get; set; }

        public User()
        {

        }

        public User TakeLoginDetailsFromUserAndAuthenticate(User user, User[] arr)   
        {
            Boolean isLoginSuccess = false;
            User fooItem = null;

            while (isLoginSuccess == false) {
                Console.WriteLine("Please enter your username");
                Username = Console.ReadLine();
                Console.WriteLine("Please enter your password");
                Password = Console.ReadLine();

                fooItem = Array.Find(arr, item => (item.Username == Username) && (item.Password == Password) && (item.Type == user.Type));

                if (fooItem == null)
                {
                    Console.WriteLine("Login unsuccessful. PLease try again.");
                }
                else
                {
                    if (user.Type == "Doctor")
                        Console.WriteLine("Welcome Dr. " + fooItem.Name);
                    else
                        Console.WriteLine("Welcome Patient " + fooItem.Name);

                    isLoginSuccess = true;
                }
            }
            return fooItem;
        }

        public override string ToString()    //override normal tostring
        {
            return "User ID " + Id +" Username "+ Username +" Name " + Name + " Age " + Age ;
        }

        //public int CompareTo(object obj)
        //{
        //    User u1, u2;
        //    u1 = this;
        //    u2 = (User)obj;
        //    return u1.Id.CompareTo(u2.Id);
        //}
    }
}
