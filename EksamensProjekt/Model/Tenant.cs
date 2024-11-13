﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Model
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        //constructor with parameters when creating tenant with full details
        public Tenant(string firstName, string lastName, string phoneNumber, string email) 
        { 
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        //constructor for UI filling details gradually 
        public Tenant() { }
    }
}
