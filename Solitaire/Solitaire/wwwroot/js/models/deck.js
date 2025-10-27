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
        this.foundation = {};
        this.tableau = [];
        this.stock = new customqueue();
        this.waste = new CustomStack();

        this.initializeGame();
    }

    initializeGame() {
        this.deck.shuffle();
        const suits = ['hearts', 'diamonds', 'clubs', 'spades'];
        for (let suit of suits) {
            this.foundation(suit) = new CustomStack();
        }

        this.tableau = [];
        for (let i = 0; i < 7; i++) {
            this.tableau.push(new linkedlist());
        }

        for (let i = 0; i < 7; i++) {
            for (let j = 0; j <= i; j++) {
                const card = this.deck.drawcard();
                if (i === j) {
                    card.isFaceUp = true;
                }
                this.tableau[i].push_back(card);
            }
            while (this.deck.size > 0) {
                this.stock.enqueue(this.deck.drawcard());
            }
        }
    }

    drawFromStock() {
        if (this.stock.isEmpty()) {
            while (!this.waste.isEmpty()) {
                const card = this.waste.pop();
                card.isFaceUp = false;
                this.stock.enqueue(card);
            }
            return;
        }

        const card = this.stock.dequeue();
        card.isFaceUp = true;
        this.waste.push(card);
    }
}