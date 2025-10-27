var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Game}/{action=Index}/{id?}");

app.Run();

//using System;
//using Solitaire.Models.datastructures;

//class Program
//{
//    static void Main(string[] args)
//    {
//        // Test LinkedList
//        CustomLinkedLis<int> ll = new CustomLinkedLis<int>();
//        ll.PushFront(6);
//        ll.PushFront(8);
//        ll.PushFront(2);
//        ll.PushBack(99);
//        ll.PopFront();
//        ll.PopBack();
//        ll.Insert(23, 2);
//        ll.Print();

//        // Test Stack
//        CustomStack<string> stack = new CustomStack<string>();
//        stack.Push("Ace of Spades");
//        stack.Push("King of Hearts");
//        Console.WriteLine(stack.Pop()); // "King of Hearts"
//        Console.WriteLine(stack.Peek()); // "Ace of Spades"

//        // Test Queue  
//        CustomQueue<string> queue = new CustomQueue<string>();
//        queue.Enqueue("First Card");
//        queue.Enqueue("Second Card");
//        Console.WriteLine(queue.Dequeue()); // "First Card"
//        Console.WriteLine(queue.Front()); // "Second Card"


//    }
//}