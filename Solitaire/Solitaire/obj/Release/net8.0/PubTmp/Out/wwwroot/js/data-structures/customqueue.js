namespace Solitaire.wwwroot.js
{
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

        size() {
            return this.list.length;
        }
    }

}
