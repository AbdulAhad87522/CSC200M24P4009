//namespace Solitaire.Models.datastructures
//{
//    public class CustomStack<T>
//    {
//        private CustomLinkedList<T> list; // UPDATED: Changed from CustomLinkedLis to CustomLinkedList

//        public CustomStack()
//        {
//            list = new CustomLinkedList<T>(); // UPDATED: Changed from CustomLinkedLis to CustomLinkedList
//        }

//        public void Push(T data)
//        {
//            list.PushFront(data);
//        }

//        public T Pop()
//        {
//            if (IsEmpty())
//            {
//                Console.WriteLine("Stack empty");
//                return default(T);
//            }
//            T data = list.GetHeadData();
//            list.PopFront();
//            return data;
//        }

//        public T Peek()
//        {
//            if (IsEmpty())
//                return default(T);
//            return list.GetHeadData();
//        }

//        public bool IsEmpty()
//        {
//            return list.Length == 0;
//        }

//        public int Size()
//        {
//            return list.Length;
//        }
//    }
//}

using Solitaire.Models.datastructures;

public class CustomStack<T>
{
    private CustomLinkedList<T> list;

    public CustomStack()
    {
        list = new CustomLinkedList<T>();
    }

    public void Push(T data)
    {
        list.PushFront(data);
    }

    public T Pop()
    {
        if (IsEmpty())
        {
            Console.WriteLine("Stack empty");
            return default(T);
        }
        // ✅ FIXED: Get data AFTER popping
        T data = list.GetHeadData();
        list.PopFront();
        return data;
    }

    public T Peek()
    {
        if (IsEmpty())
            return default(T);
        return list.GetHeadData();
    }

    public bool IsEmpty()
    {
        return list.IsEmpty(); // ✅ FIXED: Use IsEmpty() method instead of Length
    }

    public int Size()
    {
        return list.Length;
    }
}