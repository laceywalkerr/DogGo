using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//this pulls information from two different data sources/tables
namespace DogGo.Models.ViewModels
{
    public class OwnerFormViewModel
    {
        //this pulls one owner
        public Owner Owner { get; set; }
        //this pulls list of multiple neighborhoods (for our drop down)
        public List<Neighborhood> Neighborhoods { get; set; }
    }
}