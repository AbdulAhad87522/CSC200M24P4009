namespace Solitaire.Models.datastructures
{
    public class CustomStack<T>
    {
        private CustomLinkedLis<T> list;

        public CustomStack()
        {
            list = new CustomLinkedLis<T>();
        }

        public void Push(T data)
        {
            list.PushFront(data);  // LIFO - add to front
        }

        public T Pop()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Stack empty");
                return default(T);
            }
            T data = list.GetHeadData(); // We need to add this helper method
            list.PopFront();
            return data;
        }

        public T Peek()
        {
            if (IsEmpty())
                return default(T);
            return list.GetHeadData(); // We need to add this helper method
        }

        public bool IsEmpty()
        {
            return list.Length == 0;
        }

        public int Size()
        {
            return list.Length;
        }
    }
}
