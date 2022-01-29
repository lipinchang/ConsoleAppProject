using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystemModelsLibrary
{
    public class Doctor : User
    {
        public int Experience { get; set; }      //yrs 

        public string Speciality { get; set; }      //choose one category

        public Doctor(int id, string username, string name, int age, string password, int experience, string speciality)
        {
            Id = id;
            Username = username;
            Name = name;
            Age = age;
            Password = password;
            Type = "Doctor";
            Experience = experience;
            Speciality = speciality;
        }

        public Doctor()
        {
            Type = "Doctor";
        }

        public override string ToString()    //override normal tostring
        {
            return "Doctor Name: " + Name + "\nAge: " + Age + "\nExperience: " + Experience + "\nSpeciality: "+Speciality;

        }
    }
}
