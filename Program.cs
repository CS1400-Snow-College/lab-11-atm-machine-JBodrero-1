//  Jonathan Bodrero
//  July 25, 25
//  Lab 10  ATM

string[] readBankFile = File.ReadAllLines("bank.txt");  // Read in bank database
List<(string userName, int PIN, double balance)> customerData = new List<(string userName, int PIN, double balance)>();
// string userName;
// int PIN;
// double balance;

string[] customerTemp;
//(string)

string nameTemp = "";
int PINTemp = 0;
double balanceTemp = 0;

foreach (string customer in readBankFile)
{
    customerTemp = customer.Split(',');
    nameTemp = customerTemp[0];
    PINTemp = Convert.ToInt32(customerTemp[1]);
    balanceTemp = Convert.ToDouble(customerTemp[2]);

    customerData.Add((nameTemp, PINTemp, balanceTemp));

}
// customerData.ForEach(thing => Console.Write($"{thing}, ")); // Cycles through list, printing each item.
// foreach (string customer in readBankFile)

foreach (var customer in customerData)
{
    Console.WriteLine($"{customer.userName} has a PIN of {customer.PIN} with a balance of ${customer.balance}.");
}



/*
for (int i = 0; i < customerData.Count; i++)
{
    Console.WriteLine(customerData[i]);
}
*/


