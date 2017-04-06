using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Recommended_System.Models
{
    [Table("Av_Cust")]
    public class Av_Cust2
    {
        [Key]
        public int Cust_Id { get; set; }

        public int Phone { get; set; }

        [Display(Name = "Customer Name")]
        public string Cust_Nme { get; set; }

        [Display(Name = "Email")]
        public string E_mail { get; set; }

        [Display(Name = "Account Count")]
        public int Account_count { get; set; }

        [Display(Name = "Start Time")]
        public DateTime Start_Time { get; set; }

        public string Password { get; set; }

        [Display(Name = "Subscription Kind")]
        public string Sup_Type { get; set; }

        public Av_Cust2() { }
        public Av_Cust2(string Cust_Nme, int Phone, string E_mail, int Account_count, DateTime Start_Time, string Password,string Sup_Type)
        {
            
            this.Cust_Nme = Cust_Nme;
            this.Phone = Phone;
            this.E_mail = E_mail;
            this.Account_count = Account_count;
            this.Start_Time = Start_Time;
            this.Password = Password;
            this.Sup_Type = Sup_Type;
        }

       

    }
    public class Av_Cust2DBContext : DbContext
    {

        public DbSet<Av_Cust2> av_cust2 { get; set; }

    }

}
