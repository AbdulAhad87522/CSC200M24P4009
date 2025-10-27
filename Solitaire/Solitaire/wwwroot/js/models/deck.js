class deck {
    constructor() {
        this.cards = new customqueue();
        
    }

    initializedeck() {
        const suits = ['hearts', 'diamonds', 'spade', 'clubs'];
        const ranks = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13];
        for (let suit of suits) {
            for (let rank of ranks) {
                this.cards.enqueue(new this.cards(suit, card, false));
            }
        }
    }

    shuffle() {
        let cardsarray = [];
        while (!this.card.isEmpty()) {
            cardsarray.push(this.cards.dequeue());
        }

        for (let i = cardsarray.length - 1; i > 0; i--) {
            const j = math.floor(Math.random() * (i + 1));
            cardsarray[i], cardsarray[j] = cardsarray[j], cardsarray[i];
        }

        for (let card of cardsarray) {
            this.cards.enqueue(card);
        }
    }

    drawcard() {
        return this.cards.isEmpty() ? null : this.cards.dequeue();  
    }

    getsize() {
        return this.cards.size();
    }

}

class GameEngine {
    constructor() {
        this.deck = new deck();

    }
}