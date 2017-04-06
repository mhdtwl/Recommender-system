using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        public int Cust_Id { get; set; }

        [Display(Name = "Phone")]
        public int Phone { get; set; }

        [Display(Name = "Customer Name")]
        public string Cust_Nme { get; set; }

        [Display(Name = "Email")]
        public string E_mail { get; set; }

        [Display(Name = "Account Count")]
        public int Account_count { get; set; }

        [Display(Name = "Starting Date")]
        public DateTime Start_Time { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        public int Admin { get; set; }

        public int Payment { get; set; }

        public Customers() { }
        public Customers(string Cust_Nme, int Phone, string E_mail, int Account_count, DateTime Start_Time, string Password)
        {
            this.Cust_Nme = Cust_Nme;
            this.Phone = Phone;
            this.E_mail = E_mail;
            this.Account_count = Account_count;
            this.Start_Time = Start_Time;
            this.Password = Password;

        }

        public Customers(string Cust_Nme, string E_mail, int Account_count, DateTime Start_Time, string Password)
        {
            this.Cust_Nme = Cust_Nme;

            this.E_mail = E_mail;
            this.Account_count = Account_count;
            this.Start_Time = Start_Time;
            this.Password = Password;

        }


    }
    public class CustomersDBContext : DbContext
    {

        public DbSet<Customers> customer { get; set; }

    }

}