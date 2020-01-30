using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Characteristic 
        //TODO: Une Table (id, nom, type (visuel) <-probablement la même chose-> type (valeur), valuePossible)
        //valuePossible (table générique ?) T ou T[]
        //-> probablement une Table de jointure avec Jardin
    {
        public Guid Id { get; set; }
        public bool CaracOne { get; set; }
    }
}
