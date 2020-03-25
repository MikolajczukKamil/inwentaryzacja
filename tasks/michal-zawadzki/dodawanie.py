class Liczba:
    def __init__(self, wartosc):
        self.liczba = wartosc

    def Podaj(self):
        return self.liczba


def Suma(a, b):
    return a.Podaj() + b.Podaj()
