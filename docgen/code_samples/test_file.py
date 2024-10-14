class Calculator:
    """A simple calculator class that performs basic arithmetic operations."""

    def add(self, a, b):
        """Add two numbers and return the result."""
        return a + b

    def subtract(self, a, b):
        """Subtract the second number from the first and return the result."""
        return a - b

    def multiply(self, a, b):
        """Multiply two numbers and return the result."""
        return a * b

    def divide(self, a, b):
        """Divide the first number by the second and return the result."""
        if b == 0:
            raise ValueError("Cannot divide by zero.")
        return a / b

class ScientificCalculator(Calculator):
    """An advanced calculator that performs scientific calculations."""

    def power(self, base, exponent):
        """Calculate the power of a number."""
        return base ** exponent

    def square_root(self, x):
        """Calculate the square root of a number."""
        if x < 0:
            raise ValueError("Cannot take the square root of a negative number.")
        return x ** 0.5

def greet_user(name):
    """Greet the user by their name."""
    return f"Hello, {name}! Welcome to the calculator program."

def calculate_factorial(n):
    """Calculate the factorial of a non-negative integer."""
    if n < 0:
        raise ValueError("Cannot calculate factorial of negative numbers.")
    if n == 0:
        return 1
    else:
        return n * calculate_factorial(n - 1)
