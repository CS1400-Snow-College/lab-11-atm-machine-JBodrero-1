//  Jonathan Bodrero
//  July 25-28, 25
//  Lab 10  ATM


using System.Diagnostics;   //  Allow Debug.Assert

Console.Clear();
string[] readBankFile = LoadBankCustomers();    //  Load customer data.
List<(string userName, int PIN, double balance)> customerData = new List<(string userName, int PIN, double balance)>();
(string userName, int PIN, double balance) testData = ("testRun", 98765, 1000); // Item for possible testing.

string[] customerTemp;

Stack<double> transStack = new Stack<double>(); // Create a stack to track last transactions.
Stack<double> transStackTest = new Stack<double>(); // Create a stack to track last transactions.

string nameTemp = "";
int PINTemp = 0;
double balanceTemp = 0;
int option = -1;

foreach (string customer in readBankFile)       //  Load all the bank data into customerData List
{
    customerTemp = customer.Split(',');
    nameTemp = customerTemp[0];
    PINTemp = Convert.ToInt32(customerTemp[1]);
    balanceTemp = Convert.ToDouble(customerTemp[2]);
    customerData.Add((nameTemp, PINTemp, balanceTemp));
}

//  Debug.Assert(MakeDeposit(ref testData, 75.00, ref transStackTest) == true, "Make deposit must be positive.");
        //  Debug.Assert(MakeDeposit(ref testData, -100, ref transStack) == false, "Make deposit must be positive.");
//  Debug.Assert(MakeWithdrawl(ref testData, 50.00, ref transStackTest) == true, "Make withdrawal must be positive.");


        //  Debug.Assert(ValidLogin(testData) == ("testRun", 98765, 1000));  // Not sure how to use this test because asks for user input.

/*foreach (var customer in customerData)
{
    Console.WriteLine($"{customer.userName} has a PIN of {customer.PIN} with a balance of ${customer.balance}.");
}*/
(string userName, int PIN, double balance) currentCustomer = ("", -1, 0.0);  //  Create a tuple for current customer

Console.Clear();
Console.WriteLine("Welcome to BadgerBank.");
currentCustomer = ValidLogin(customerData);     //  Check for valid customer login

if (currentCustomer.userName == "" || currentCustomer.PIN < 0)
{
    Console.WriteLine("Login failed. Exiting program.");
    Environment.Exit(0);  // AI helped me do this to terminate the program  
}


do
{
    Console.Clear();
    Console.WriteLine(@"Please pick an option from the list below.
    1.  Check Balance
    2.  Withdraw
    3.  Deposit
    4.  Display last 5 transactions
    5.  Quick Withdraw $40
    6.  Quick Withdraw $100
    7.  End current session");
    Console.Write("Selection: ");

    string userInput = Console.ReadLine();

    while (!Int32.TryParse(userInput, out option) || option < 1 || option > 7)  // AI helped me combine code into one while loop.
    {

        Console.SetCursorPosition(0, 9);
        Console.WriteLine("                                                                                           ");
        Console.SetCursorPosition(0, 9);
        Console.WriteLine($"Oops. \"{userInput}\" is not a valid option. Please enter a number between 1 and 7.");
        Console.SetCursorPosition(0, 8);
        Console.WriteLine("                          ");
        Console.SetCursorPosition(0, 8);
        Console.Write("Selection: ");
        userInput = Console.ReadLine();
    }
    switch (option)
    {
        case 1:
            CheckBalance(ref currentCustomer);
            break;

        case 2:
            double withdrawl = -1;

            MakeWithdrawl(ref currentCustomer, withdrawl, ref transStack);
            break;

        case 3:
            double deposit = -1;
            MakeDeposit(ref currentCustomer, deposit, ref transStack);
            break;

        case 4:
            LastFive(ref currentCustomer, ref transStack);
            break;

        case 5:
            int amount40 = 40;
            QuickWithdrawl(ref currentCustomer, amount40, ref transStack);
            break;

        case 6:
            int amount100 = 100;
            QuickWithdrawl(ref currentCustomer, amount100, ref transStack);
            break;

        case 7:     //  Update current customer to bank file and write out to file.
            Console.WriteLine("Thanks for banking with BadgerBank!  Goodbye.");
            int place = customerData.FindIndex(x => x.userName == currentCustomer.userName);
            customerData.RemoveAt(place);
            customerData.Insert(place, currentCustomer);
            SaveBankCustomers(customerData);
            break;
    }
    /* if (option != 7)
    {
        Console.WriteLine("\nPress Enter to return to the menu...");
        Console.ReadLine();
    }*/
} while (option != 7);


string[] LoadBankCustomers()
{
    string[] Data = File.ReadAllLines("bank.txt");  // Read in bank database
    return Data;
}



static void SaveBankCustomers(List<(string userName, int PIN, double balance)> customerData)
{
    List<string> writeBankFile = new List<string>();

    
    for (int i = 0; i < customerData.Count; i++)
    {
        writeBankFile.Add($"{customerData[i].userName}, {customerData[i].PIN}, {customerData[i].balance}");
    }
            //foreach(string bankFile in writeBankFile)
            //Console.WriteLine(bankFile);
            File.WriteAllLines("bank.txt", writeBankFile);
}

//  Method for validating login
(string userName, int PIN, double balance) ValidLogin(List<(string userName, int PIN, double balance)> customerData)
{

    string inputUser;
    int inputPIN;
    
    Console.Write("Please enter your userName: ");
    inputUser = Console.ReadLine();
    Console.Write("Please enter your PIN: ");
    while (!Int32.TryParse(Console.ReadLine(), out inputPIN))  //  Check if input is an integer
    {
        Console.Write("Oops. That is not a number. Try starting over.");
        return ("", -1, 0.0);
    }
    
    foreach (var user in customerData)          //  Check pin matches username.
    {  if (user.userName == inputUser && user.PIN == inputPIN)
        {
            return user;  // Return the matched tuple
        }
    }

        Console.WriteLine("That is not a valid username / PIN number combination.");
        return ("", -918, 0.0);
        
}

//Method to check balance
static bool CheckBalance(ref (string userName, int PIN, double balance) currentCustomer)
{
    Console.SetCursorPosition(0, 9);    // Clear out any prior warning message.
    Console.WriteLine("                                                                                       ");
    Console.SetCursorPosition(0, 9);
    Console.WriteLine($"Your current balance is: ${currentCustomer.balance:F2}.\nPress any key to continue.");
    Console.ReadKey(true);
    return true;
}


//  Method for making deposits.
static bool MakeDeposit(ref (string userName, int PIN, double balance) currentCustomer, double deposit, ref Stack<double> transStack)
{
    bool isValidDeposit = false;

    while (!isValidDeposit)
    {
        Console.SetCursorPosition(0, 9);    // Clear out any prior warning message.
        Console.WriteLine("                                                                                       ");
        Console.SetCursorPosition(0, 9);
        Console.Write("How much are you depositing? $");
        string input = Console.ReadLine();
        input.Trim('$');
        input.Trim(' ');
        input.Trim(';');

        // Try to parse the input as a double > 0

        isValidDeposit = Double.TryParse(input, out deposit);

        if (!isValidDeposit || (deposit - Math.Round(deposit, 2) != 0)) // Ensure to nearest penny.
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a valid number.\nPress any key to continue.");
            Console.ReadKey(true);
            return false;
        }
        //deposit = Math.Round(deposit, 2);   //  Round input to two places (nearest cent).
        if (deposit <= 0)
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a number greater than 0.00.\nPress any key to continue.");
            Console.ReadKey(true);
            return false;
        }
    }
    transStack.Push(deposit);
    currentCustomer.balance = currentCustomer.balance + deposit;
    Console.SetCursorPosition(0, 9);    // Clear out any prior warning message.
    Console.WriteLine("                                                                                       ");
    Console.SetCursorPosition(0, 9);
    Console.WriteLine($"Your current balance is: ${Math.Round(currentCustomer.balance, 2):F2}.\nPress any key to continue.");
    Console.ReadKey(true);
    return true;
}

//  Methods for making withdrawls.
static bool MakeWithdrawl(ref (string userName, int PIN, double balance) currentCustomer, double withdrawl, ref Stack<double> transStack)
{
    bool isValidWithdrawl = false;

    while (!isValidWithdrawl)
    {
        Console.SetCursorPosition(0, 9);    // Clear out any prior warning message.
        Console.WriteLine("                                                                                       ");
        Console.SetCursorPosition(0, 9);
        Console.Write("How much are you withdrawing? $");
        string input = Console.ReadLine();
        input.Trim('$');
        input.Trim(' ');
        input.Trim(';');

        // Try to parse the input as a double > 0

        isValidWithdrawl = Double.TryParse(input, out withdrawl);
        
        if (!isValidWithdrawl || (withdrawl - Math.Round(withdrawl, 2) != 0))  // Ensure nearest penny.
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a valid number.\nPress any key to continue.");
            Console.ReadKey(true);
            return false;
        }
        if (withdrawl <= 0)
        {
            Console.WriteLine("Oops. That is not a valid amount. Please enter a number greater than $0.00.\nPress any key to continue.");
            Console.ReadKey(true);
            return false;
        }
        if (withdrawl > currentCustomer.balance)
        {
            Console.WriteLine($"Oops. You don't have that much money in your account.  \nYou may withdraw up to ${Math.Round(currentCustomer.balance, 2)}.\nPress any key to continue.");
            Console.ReadKey(true);
            return true;
        }
        
    }
    transStack.Push(-1 * withdrawl);
    currentCustomer.balance = currentCustomer.balance - withdrawl;
    Console.SetCursorPosition(0, 9);    // Clear out any prior warning message.
    Console.WriteLine("                                                                                       ");
    Console.SetCursorPosition(0, 9);
    Console.WriteLine($"Your current balance is: ${Math.Round(currentCustomer.balance, 2):F2}.\nPress any key to continue.");
    Console.ReadKey(true);
    return true;

}

static bool QuickWithdrawl(ref (string userName, int PIN, double balance) currentCustomer, int amount, ref Stack<double> transStack)
{
    if (amount > currentCustomer.balance)
    {
        Console.WriteLine($"Oops. You don't have that much money in your account.  \nYou may withdraw up to ${Math.Round(currentCustomer.balance, 2)}.\nPress any key to continue.");
        Console.ReadKey(true);
        return false;
    }
    currentCustomer.balance = currentCustomer.balance - amount;
    transStack.Push(-1 * amount);
    Console.SetCursorPosition(0, 9);    // Clear out any prior warning message.
    Console.WriteLine("                                                                                       ");
    Console.SetCursorPosition(0, 9);
    Console.WriteLine($"You are doing a quick withdrawl for ${amount}. \n Your new balance is ${Math.Round(currentCustomer.balance, 2):F2}.\nPress any key to continue.");
    Console.ReadKey(true);
    return true;
}

static bool LastFive(ref (string userName, int PIN, double balance) currentCustomer, ref Stack<double>transStack)  // I got a little help from AI on this method.
{
    Console.SetCursorPosition(0, 9);    // Clear out any prior warning message.
    Console.WriteLine("                                                                                       ");
    Console.SetCursorPosition(0, 9);
    Console.WriteLine("Your last transactions during this login were: ");
    Stack<double> tempStack = new Stack<double>(transStack);    //  It appears this copying of stacks reverses order (feedback from AI).
    Stack<double> tempStack2 = new Stack<double>(tempStack);    //  Reverse the order back.

    int tempTrans = 5;
    if (transStack.Count < 5)
    { tempTrans = transStack.Count; }
    for (int i = 0; i < tempTrans; i++)
    {
        double transaction = tempStack2.Pop();
        
        // Display the transaction (positive for deposits, negative for withdrawals)
        if (transaction > 0)
        {
            Console.WriteLine($"Deposited: ${transaction}");
        }
        else
        {
            Console.WriteLine($"Withdrew:  ${-transaction}");
        }
    }
    Console.WriteLine($"Your current balance is: ${Math.Round(currentCustomer.balance, 2):F2}.\nPress any key to continue.");
    Console.ReadKey(true);
    return true;
}