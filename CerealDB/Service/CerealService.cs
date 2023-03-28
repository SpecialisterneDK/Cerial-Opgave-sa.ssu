using CsvHelper;
using CsvHelper.Configuration;
using CerealDB.Models;
using System.Globalization;
using System.Text;

namespace CerealDB.Service;

public class CerealService
{
    static public List<Cereal> LoadCerialCSV()
    {
        //Loads a CSVfile composed of 
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Encoding = Encoding.UTF8, // Our file uses UTF-8 encoding.
            Delimiter = ";" // The delimiter is a coloun.
        };

        using var reader = new StreamReader("C:\\Users\\KOM\\Documents\\Cerial\\cereal_unclean (2).csv");
        using var csv = new CsvReader(reader, config);
        var products = csv.GetRecords<Cereal>().ToList();
        return products;

    }
    
    //Writes the data from the list into a new CSVfile.
    //This was useful when the project used In-memory databases, but unneeded as an SQLDatabase is the system database now.
    public void WriteCsvFile(List<Cereal> filteredProducts)
    {
        using var writer = new StreamWriter("C:\\Users\\KOM\\Documents\\Cerial\\cerealFiltered.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<CsvFileMap>();

        csv.WriteHeader<Cereal>();
        csv.NextRecord();
        csv.WriteRecords(filteredProducts);
    }
}

