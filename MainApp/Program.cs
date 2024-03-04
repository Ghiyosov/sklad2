



using Domein.Models;
using Infrastraction.Services;

var moove = new MooveService();
var sklad = new SkladService();
var product = new ProductService();

while (true)
{
    System.Console.WriteLine("ap - add new product");
    System.Console.WriteLine("ps - shov list of produkt in sklad");
    System.Console.WriteLine("mp - moove product at one sklad to another sklad");
    System.Console.WriteLine("sp - story of product");
    System.Console.WriteLine("us - update sklad");
    string comand = Console.ReadLine();
    comand = comand.ToLower();
    if (comand == "ap")
    {
        System.Console.Write("Name : ");
        string nm = Console.ReadLine();
        System.Console.Write("SkladId : ");
        int im = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Kolichesva : ");
        int kp = Convert.ToInt32(Console.ReadLine());
        var pro = new Product();
        pro.Name = nm;
        pro.SkladId = im;
        pro.Kol = kp;
        product.AddProdact(pro);
    }
    else if (comand == "ps")
    {
        System.Console.WriteLine("Sklad id : ");
        int id = Convert.ToInt32(Console.ReadLine());
        var sk = sklad.GetProdactFromSklad(id);
        foreach (var item in sk)
        {
            System.Console.WriteLine($"name : {item.Name} ; kolichestvo : {item.Kol}");
        }
    }
    else if (comand == "mp")
    {
        System.Console.Write("Product Id : ");
        int pid = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("To Sklad Id : ");
        int sid = Convert.ToInt32(Console.ReadLine());
        string dat =  Convert.ToString(DateTime.Now);
        var mv = new Moove();
        mv.ProductId=pid;
        mv.ToSklad=sid;
        mv.Times=dat;

        moove.AddMoove(mv);
    }
    else if (comand == "sp")
    {
        foreach (var item in moove.MooveInfo())
        {
            System.Console.WriteLine($"{item.Product} mooved at {item.AtSklad} to {item.ToSklad} times : {item.Times}");
        }
     }
     else if (comand == "us")
     {
        moove.EqualizeSklad();
     }
}


