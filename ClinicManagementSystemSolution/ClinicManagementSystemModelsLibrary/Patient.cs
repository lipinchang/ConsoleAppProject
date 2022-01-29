using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModelsLibrary
{
    public class Patient : User
    {
        public string Remarks { get; set; }

        public string Status { get; set; }

        public Patient(int id, string username, string name, int age, string password, string status, string remarks)
        {
            Id = id;
            Username = username;
            Name = name;
            Age = age;
            Password = password;
            Type = "Patient";
            Status = status;
            Remarks = remarks;
        }

        public Patient()
        {
            Type = "Patient";
        }

        public override string ToString() 
        {
            return "User ID: " + Id + "\nUsername: " + Username + "\nName: " + Name + "\nAge: " + Age + "\nStatus: " + Status + "\nRemarks: " +Remarks;

        }

    }
}
