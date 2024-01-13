// See https://aka.ms/new-console-template for more information
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using Store;
using System.Security.AccessControl;
ClothingStore store = new ClothingStore("clothing store");

Actions();
void Actions()
{
    Console.WriteLine("hi to the clothing store \n1 -- look stock \n2 -- buy clothes \n3 -- look sales");
    string? answer = Console.ReadLine();
    switch (answer)
    {
        case "1":
            store.Stock();
            break;
        case "2":
            store.Purchase();
            break;
        case "3":
            store.SalesFile();
            break;
        default:
            Console.WriteLine("that not an option");
            break;
    }
    Actions();
}

