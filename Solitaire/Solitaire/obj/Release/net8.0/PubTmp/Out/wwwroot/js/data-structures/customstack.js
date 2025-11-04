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

        size() {
            return this.list.length;
        }
    }