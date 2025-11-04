namespace Solitaire.wwwroot.js
{
    class List {
        constructor(data) {
            this.data = data;
            this.next = null;
        }
    }

    class linkedlist {
        constructor() {
            this.head = null;
            this.tail = null;
            this.length = 0;
        }

        push_front(data) {
            let newnode = new List(data)
            if (this.head == null) {
                this.head = this.tail = newnode;
                return;
            }
            else {
                newnode.next = this.head;
                this.head = newnode;
            }
            this.length++;
        }

        push_back(data) {
            let newnode = new List(data);
            if (this.head == null) {
                this.head = this.tail = newnode;
                return;
            }
            else {
                this.tail.next = newnode;
                this.tail = newnode;
            }
            this.length++;
        }

        pop_front() {
            if (this.head == null) {
                console.log("empty list");
                return;
            }
            else {
                let temp = this.head;
                this.head = this.head.next;
                temp.next = null;
                this.length--;
            }
        }

        pop_back() {
            if (this.head == null) {
                console.log("empty list");
                return;
            }
            else if (this.head === this.tail) {
                this.head = this.tail = null;
            }
            else {
                let temp = this.head;
                while (temp.next.next != null) {
                    temp = temp.next;
                }
                temp.next = null;
                this.tail = temp;
                this.length--;
            }
        }

        insert(data, pos) {
            if (pos < 1) {
                console.log("invalid position entered.");
                return;
            }
            else if (pos == 1) {
                this.push_front(data);
            }
            else {
                let temp = this.head;
                for (let i = 1; i < pos - 1; i++) {
                    if (temp === null) {
                        console.log("invalid position entered.");
                        return;
                    }
                    temp = temp.next;
                }
                if (temp === null) {
                    console.log("invalid position entered.");
                    return;
                }
                let newnode = new List(data);
                newnode.next = temp.next;
                temp.next = newnode
                this.length++;
            }
        }

        print() {
            let temp = this.head;
            while (temp != null) {
                console.log(temp.data, " ");
                temp = temp.next;
            }
        }
    }

}
