
Console.WriteLine("ASD");

Task writeTest = new Task(() => {
    int i = 0;
    while (i<500)
    {
        Console.WriteLine(i);
        i++;
    }

});

writeTest.Start();
Console.ReadKey();
