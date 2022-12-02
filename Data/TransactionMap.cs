using CsvHelper.Configuration;


public class TransactionMap : ClassMap<Transaction>
{
    public TransactionMap()
    {
        Map(m => m.Id).Name("ID");
        Map(m => m.DateofTransaction).Name("Date of Transaction");
        Map(m => m.transaction).Name("Transaction");
        Map(m => m.Units).Name("Units");
        Map(m => m.Price).Name("Price");
    }
}
