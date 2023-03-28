using CerealDB.Models;
using System;

namespace CerealDB
{
    public class SearchCriteria
    {
        public string? Name { get; set; }
        public char? Mfr { get; set; }
        public char? Type { get; set; }
        public int? Calories { get; set; }
        public int? Protein { get; set; }
        public int? Fat { get; set; }
        public int? Sodium { get; set; }
        public float? Fiber { get; set; }
        public float? Carbo { get; set; }
        public int? Sugars { get; set; }
        public int? Potass { get; set; }
        public int? Vitamins { get; set; }
        public int? Shelf { get; set; }
        public float? Weight { get; set; }
        public float? Cups { get; set; }
        public float? Rating { get; set; }

        public bool MatchCritieria(Cereal cereal)
        {
            if(Name != null && cereal.Name != Name) return false;
            if (Mfr != null && cereal.MFR != Mfr) return false;
            if (Type != null && cereal.Type != Type) return false;
            if (Calories != null && cereal.Calories != Calories) return false;
            if (Protein != null && cereal.Protein != Protein) return false;
            if (Fat != null && cereal.Fat != Fat) return false;
            if(Sodium != null && cereal.Sodium != Sodium)   return false;
            if(Fiber != null &&  cereal.Fiber != Fiber) return false;
            if(Carbo != null && cereal.Carbo != Carbo) return false;
            if(Sugars != null && cereal?.Sugars != Sugars) return false;
            if(Potass != null && cereal.Potass != Potass) return false;
            if(Vitamins != null && cereal.Vitamins != Vitamins) return false;
            if(Shelf != null &&  cereal.Shelf != Shelf) return false;
            if(Weight != null && cereal.Weight != Weight) return false;
            if(Cups != null && cereal.Cups != Cups) return false;
            if(Rating != null && cereal.Rating != Rating) return false;
            return true;
        }
    }




}

