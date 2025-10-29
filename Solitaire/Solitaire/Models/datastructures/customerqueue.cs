
//namespace Solitaire.Models.datastructures
//{
//    public class CustomQueue<T>
//    {
//        private CustomLinkedList<T> list; // UPDATED: Changed from CustomLinkedLis to CustomLinkedList

//        public CustomQueue()
//        {
//            list = new CustomLinkedList<T>(); // UPDATED: Changed from CustomLinkedLis to CustomLinkedList
//        }

//        public void Enqueue(T data)
//        {
//            list.PushBack(data);
//        }

//        public T Dequeue()
//        {
//            if (IsEmpty())
//            {
//                Console.WriteLine("Queue empty");
//                return default(T);
//            }
//            T data = list.GetHeadData();
//            list.PopFront();
//            return data;
//        }

//        public T Front()
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

public class CustomQueue<T>
{
    private CustomLinkedList<T> list;

    public CustomQueue()
    {
        list = new CustomLinkedList<T>();
    }

    public void Enqueue(T data)
    {
        list.PushBack(data);
    }

    public T Dequeue()
    {
        if (IsEmpty())
        {
            Console.WriteLine("Queue empty");
            return default(T);
        }
        // ✅ FIXED: Get data AFTER popping
        T data = list.GetHeadData();
        list.PopFront();
        return data;
    }

    public T Front()
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