namespace Solitaire.Models.datastructures

{
    public class CustomQueue<T>
    {
        private CustomLinkedLis<T> list;

        public CustomQueue()
        {
            list = new CustomLinkedLis<T>();
        }

        public void Enqueue(T data)
        {
            list.PushBack(data);  // FIFO - add to back
        }



        public T Dequeue()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Queue empty");
                return default(T);
            }
            T data = list.GetHeadData(); // We need to add this helper method
            list.PopFront();
            return data;
        }

        public T Front()
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
