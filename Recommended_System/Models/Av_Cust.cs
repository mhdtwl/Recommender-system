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
    public class Av_Cust
    {
        [Key]
        public int Cust_Id { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public int Phone { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Your Name")]
        public string Cust_Nme { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail")]
        public string E_mail { get; set; }
        [Required]
        [Display(Name = "Shop Count")]
        public int Account_count { get; set; }
        [Required]
        [DataType(DataType.DateTime)]       
        public DateTime Start_Time { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        public string Passwordco { get; set; }
        
        [Display(Name = "Subscription Kind")]
        public SubScription_KindtDBContext subscription_kind { get; set; }

        public Av_Cust() { }
        public Av_Cust(string Cust_Nme, int Phone, string E_mail, int Account_count, DateTime Start_Time, string Password)
        {
            this.Cust_Nme = Cust_Nme;
            this.Phone = Phone;
            this.E_mail = E_mail;
            this.Account_count = Account_count;
            this.Start_Time = Start_Time;
            this.Password = Password;

        }

        public Av_Cust(string Cust_Nme, string E_mail, int Account_count, DateTime Start_Time, string Password)
        {
            this.Cust_Nme = Cust_Nme;

            this.E_mail = E_mail;
            this.Account_count = Account_count;
            this.Start_Time = Start_Time;
            this.Password = Password;

        }


    }
    public class Av_CustDBContext : DbContext
    {

        public DbSet<Av_Cust> av_cust { get; set; }
       
    }

}