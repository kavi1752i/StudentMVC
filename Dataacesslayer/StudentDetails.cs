using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DataAccessLayer
{
    public class StudentDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nmae is reqiured")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email  is reqiured")]
        [EmailAddress(ErrorMessage = "invalid Email  address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobilenumber  is reqiured")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "enter valid 10 digit number")]
        public string Mobilenumber { get; set; }
       
        public int StateId { get; set; }
        public string StateName { get; set; }
        
        [Required(ErrorMessage = "Gender  is reqiured")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "DOB  is reqiured")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
 
       
    }
    public class State
    {
        public int StateId { get; set; }

        public string StateName { get; set; }
    }
    

}
