using CsvHelper.Configuration;


namespace CerealDB.Models
{
    public class CsvFileMap : ClassMap<Cereal>
    {
        public CsvFileMap()
        {
            Map(m=> m.Id).Name("id");
            Map(m => m.Name).Name("name");
            Map(m => m.MFR).Name("mfr");
            Map(m => m.Type).Name("type");
            Map(m => m.Calories).Name("calories");
            Map(m => m.Protein).Name("protein");
            Map(m => m.Fat).Name("fat");
            Map(m => m.Sodium).Name("sodium");
            Map(m => m.Fiber).Name("fiber");
            Map(m => m.Carbo).Name("carbo");
            Map(m => m.Sugars).Name("sugars");
            Map(m => m.Potass).Name("potass");
            Map(m => m.Vitamins).Name("vitamins");
            Map(m => m.Shelf).Name("shelf");
            Map(m => m.Weight).Name("weight");
            Map(m => m.Cups).Name("cups");
            Map(m => m.Rating).Name("rating");
        }
    }
}
