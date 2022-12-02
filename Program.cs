using CsvHelper;
using System.Globalization;

using (StreamReader streamReader = new StreamReader(@"C:\Users\marie\Downloads\CGLexcercise.csv"))
{
    using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
    {
        csvReader.Context.RegisterClassMap<TransactionMap>();
        List<Transaction> records = csvReader.GetRecords<Transaction>().ToList();

        int units = 0;
        int price = 0;
       
        List<Transaction> sales = getSales(records);
        List<Transaction> purchases = getPurchases(records);

        foreach (Transaction sell in sales)
        {
            units = Math.Abs(sell.Units);
            price = sell.Price;
            getFifo(purchases, units,price, records, sell);

        }
    }
}

void getFifo(List<Transaction> purchases, int units, int price, List<Transaction> records, Transaction sell)
{
    foreach (Transaction purchase in purchases)
    {
        if (purchase.Id > sell.Id)
        {
            return;
        }
        int CGL = (price - purchase.Price) * purchase.Units;

        if (units >= purchase.Units)
        {
            units -= purchase.Units;
            purchase.Units = 0;
        } 
        //kdyby tady bylo and možná? 
        if (units != 0)
        {
            purchases = (from record in records
                         where (record.transaction == "Purchase" && record.Id > purchase.Id
                         && record.Id < sell.Id && record.Units != 0)
                         select record).ToList();
            //tady by bylo lepší mít podmínku, aby se případně rovnou neprováděla následující funkce

            Console.WriteLine((getAnotherStock(purchases, units, price, CGL)));
        }
        else
        {
            Console.WriteLine(CGL);
        }
        
    }
}

int getAnotherStock(IEnumerable<Transaction> purchases, int units, int price, int CGL)
{
    //chybí podmínka
    foreach (Transaction purchase in purchases)
    {
        int originunits = units;
        units -= purchase.Units;
        if (units <= 0)
        {
            units = 0;
            purchase.Units -= originunits;
            CGL += purchase.Units * (price - purchase.Price);
        }
    }
    //tady
    return CGL;
}

List<Transaction> getPurchases(List<Transaction> records)
{
    return (from record in records
            where (record.transaction == "Purchase" && record.Units != 0)
            select record).ToList();
}

List<Transaction> getSales(List<Transaction> records)
{
    return (from record in records
            where (record.transaction == "Sell")
            select record).ToList();
}



//omezit nesting, debuggovat. 

//foreach (var item in purchases)
//{
//    Console.WriteLine($"{item.Id},{item.transaction},{item.Price},{item.Units},{item.DateofTransaction}");
//}
//purchases = (from record in records
//            where (record.transaction == "Purchase" && record.Id > purchase.Id && record.Id <sell.Id)
//            select record);
//není lepší ptát se na to, které nemají x.units =0? 

//Console.WriteLine("sales");
//foreach (var item in sales)
//{
//    Console.WriteLine($"{item.Id},{item.transaction},{item.Price},{item.Units},{item.DateofTransaction}");
//}
//else
//{
//    Console.WriteLine(CGL);
//}
//foreach (var item in purchases)
//{
//    Console.WriteLine($"{item.Id},{item.transaction},{item.Price},{item.Units},{item.DateofTransaction}");
//}
//je chybe někde tady od 40. řádku. 
//ono to totiž nemusí být foreach purchase, ale odečítá se do té doby, dokud není units nula.
//přidat podmínku v případě že units nejsou větší nebo rovno purchase.units
//tady jde dát metoda s argumenty units a puchase units pro odečítání.
// problém je na lince 22, kód bere poslední záznam, protože on má stále 50.