import random

class JokeBot:

    def __init__(self, name="JokeBot"):
        self.name = name
        self.dad_jokes = [
            "Why did the scarecrow win an award? Because he was outstanding in his field!",
            "I'm reading a book on anti-gravity. It's impossible to put down!",
            "I used to be a banker, but I lost interest."
        ]
        self.puns = [
            "I once told a chemistry joke... there was no reaction.",
            "I’m on a seafood diet. I see food, and I eat it!",
            "I told my computer I needed a break, and now it won’t stop sending me Kit-Kats."
        ]
        self.one_liners = [
            "I told my boss I needed a raise. I knew he could see I was underpaid… just like the toilet paper.",
            "I'm not lazy, I’m just on power-saving mode.",
            "I would tell you a pizza joke, but it’s a little cheesy."
        ]

    def tell_dad_joke(self):
        """Prints a random dad joke."""
        joke = random.choice(self.dad_jokes)
        print(f"{self.name} says: {joke}")

    def tell_pun(self):
        """Prints a random pun."""
        pun = random.choice(self.puns)
        print(f"{self.name} says: {pun}")

    def tell_one_liner(self):
        """Prints a random one-liner."""
        one_liner = random.choice(self.one_liners)
        print(f"{self.name} says: {one_liner}")

    def roast(self, target="you"):
        """Prints a funny roast directed at the target."""
        roasts = [
            f"{target}, if laughter is the best medicine, your face must be curing the world.",
            f"{target}, you bring everyone so much joy when you leave the room!",
            f"Sorry {target}, but your secrets are safe with me… I wasn’t listening anyway."
        ]
        roast = random.choice(roasts)
        print(f"{self.name} says: {roast}")
