namespace CerealDB.Models
{
    using CsvHelper.Configuration.Attributes;
    public class Cereal
    {
        [Name("id")]
        public int Id { get; set; }
        [Name("name")]
        public string? Name { get; set; }
        [Name("mfr")]
        public char MFR { get; set; }
        [Name("type")]
        public char? Type { get; set; }
        [Name("calories")]
        public int Calories { get; set; }
        [Name("protein")]
        public int Protein { get; set; }
        [Name("fat")]
        public int Fat { get; set; }
        [Name("sodium")]
        public int Sodium { get; set; }
        [Name("fiber")]
        public float Fiber { get; set; }
        [Name("carbo")]
        public float Carbo { get; set; }
        [Name("sugars")]
        public int Sugars { get; set; }
        [Name("potass")]
        public int Potass { get; set; }
        [Name("vitamins")]
        public int Vitamins { get; set; }
        [Name("shelf")]
        public int Shelf { get; set; }
        [Name("weight")]
        public float Weight { get; set; }
        [Name("cups")]
        public float Cups { get; set; }
        [Name("rating")]
        public float Rating { get; set; }

    }
}
