    class List{
        constructor(data){
            this.data = data;
            this.next = null;
        }
    }

    class linkedlist
    {
        constructor()
        {
            this.head = null;
            this.tail = null;
            this.length = 0;
        }

        push_front(data){
            let newnode = new List(data)
            if(this.head == null)
            {
                this.head = this.tail = newnode;
                return;
            }
            else{
                newnode.next = this.head;
                this.head = newnode;
            }
            this.length++;
        }

        push_back(data)
        {
            let newnode = new List(data);
            if(this.head == null)
            {
                this.head = this.tail = newnode;
                return;
            }
            else
            {
                this.tail.next = newnode;
                this.tail = newnode;
            }
            this.length++;
        }

        pop_front()
        {
            if(this.head == null)
            {
                console.log("empty list");
                return;
            }
            else{
                let temp = this.head;
                this.head = this.head.next;
                temp.next = null;
                this.length--;
            }
        }

        pop_back()
        {
            if(this.head == null)
            {
                console.log("empty list");
                return;
            }
            else if(this.head === this.tail)
            {
                this.head = this.tail = null;
            }
            else{
                let temp = this.head;
                while(temp.next.next != null)
                {
                    temp = temp.next;
                }
                temp.next = null;
                this.tail = temp;
                this.length--;
            }
        }

        insert(data,pos)
        {
            if(pos < 1)
            {
                console.log("invalid position entered.");
                return;
            }
            else if(pos==1)
            {
                this.push_front(data);
            }
            else{
                let temp = this.head;
                for(let i = 1 ; i < pos - 1 ; i++)
                {
                    if(temp === null)
                    {
                        console.log("invalid position entered.");
                        return;
                    }
                    temp = temp.next;
                }
                if(temp === null)
                    {
                        console.log("invalid position entered.");
                        return;
                    }
                let newnode = new List(data);
                newnode.next = temp.next;
                temp.next = newnode
                this.length++;
            }
        }

        print()
        {
            let temp = this.head;
            while(temp != null)
            {
                console.log(temp.data , " ");
                temp = temp.next;
            }
        }
    }

    // const ll  = new linkedlist();
    // ll.push_front("card a");
    // ll.push_front(6);
    // ll.push_front(8);
    // ll.push_front(2);
    // ll.push_back(99);
    // ll.pop_front();
    // ll.pop_back();
    // ll.insert(23,2)
    // ll.print();





class CustomStack {
    constructor() {
        this.list = new linkedlist();  // Use YOUR linked list
    }
    
    push(data) {
        this.list.push_front(data);  // LIFO - add to front
    }
    
    pop() {
        if (this.isEmpty()) {
            console.log("Stack empty");
            return null;
        }
        let data = this.list.head.data;
        this.list.pop_front();
        return data;
    }
    
    peek() {
        return this.list.head ? this.list.head.data : null;
    }
    
    isEmpty() {
        return this.list.head === null;
    }

    size()
    {
        return this.list.length;
    }
}





class CustomQueue {
    constructor() {
        this.list = new linkedlist();  // Use YOUR linked list
    }
    
    enqueue(data) {
        this.list.push_back(data);  // FIFO - add to back
    }
    
    dequeue() {
        if (this.isEmpty()) {
            console.log("Queue empty");
            return null;
        }
        let data = this.list.head.data;
        this.list.pop_front();
        return data;
    }
    
    front() {
        return this.list.head ? this.list.head.data : null;
    }
    
    isEmpty() {
        return this.list.head === null;
    }
    
    size()
    {
        return this.list.length;
    }
}

// Test Stack
const stack = new CustomStack();
stack.push("Ace of Spades");
stack.push("King of Hearts");
console.log(stack.pop()); // "King of Hearts"
console.log(stack.peek()); // "Ace of Spades"

// Test Queue  
const queue = new CustomQueue();
queue.enqueue("First Card");
queue.enqueue("Second Card");
console.log(queue.dequeue()); // "First Card"
console.log(queue.front()); // "Second Card"