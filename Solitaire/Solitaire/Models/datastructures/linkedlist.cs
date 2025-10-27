namespace Solitaire.Models.datastructures
{
    public class ListNode<T>
    {
        public T Data { get; set; }
        public ListNode<T> Next { get; set; }

        public ListNode(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class CustomLinkedLis<T>
    {
        private ListNode<T> head;
        private ListNode<T> tail;
        private int length;

        public CustomLinkedLis()
        {
            head = null;
            tail = null;
            length = 0;
        }

        public int Length => length;

        public void PushFront(T data)
        {
            ListNode<T> newNode = new ListNode<T>(data);
            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                newNode.Next = head;
                head = newNode;
            }
            length++;
        }

        public void PushBack(T data)
        {
            ListNode<T> newNode = new ListNode<T>(data);
            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
            length++;
        }

        public void PopFront()
        {
            if (head == null)
            {
                Console.WriteLine("empty list");
                return;
            }
            else
            {
                ListNode<T> temp = head;
                head = head.Next;
                temp.Next = null;

                // If list becomes empty after pop
                if (head == null)
                    tail = null;

                length--;
            }
        }

        public void PopBack()
        {
            if (head == null)
            {
                Console.WriteLine("empty list");
                return;
            }
            else if (head == tail)
            {
                head = tail = null;
                length--;
            }
            else
            {
                ListNode<T> temp = head;
                while (temp.Next.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = null;
                tail = temp;
                length--;
            }
        }

        public void Insert(T data, int pos)
        {
            if (pos < 1)
            {
                Console.WriteLine("invalid position entered.");
                return;
            }
            else if (pos == 1)
            {
                PushFront(data);
            }
            else
            {
                ListNode<T> temp = head;
                for (int i = 1; i < pos - 1; i++)
                {
                    if (temp == null)
                    {
                        Console.WriteLine("invalid position entered.");
                        return;
                    }
                    temp = temp.Next;
                }
                if (temp == null)
                {
                    Console.WriteLine("invalid position entered.");
                    return;
                }
                ListNode<T> newNode = new ListNode<T>(data);
                newNode.Next = temp.Next;
                temp.Next = newNode;

                // Update tail if inserting at the end
                if (newNode.Next == null)
                    tail = newNode;

                length++;
            }
        }

        public void Print()
        {
            ListNode<T> temp = head;
            while (temp != null)
            {
                Console.Write(temp.Data + " ");
                temp = temp.Next;
            }
            Console.WriteLine();
        }

        public T GetHeadData()
        {
            if (head == null)
                return default(T);
            return head.Data;
        }
    }
}
