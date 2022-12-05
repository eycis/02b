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
        foreach (var item in purchases)
        {
            Console.WriteLine($"{item.Id},{item.transaction},{item.Price},{item.Units},{item.DateofTransaction}");
        }
        Console.WriteLine("-------------------------------------");
        foreach (var item in sales)
        {
            Console.WriteLine($"{item.Id},{item.transaction},{item.Price},{item.Units},{item.DateofTransaction}");
        }

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
            Console.WriteLine("--------------------------");
            foreach (var item in purchases)
            {
                Console.WriteLine($"{item.Id},{item.transaction},{item.Price},{item.Units},{item.DateofTransaction}");
            }

            

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
            break;
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


