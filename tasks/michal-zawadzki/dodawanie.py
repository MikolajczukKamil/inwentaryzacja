tablica = [1, 2, 3, 4, 5]


class Liczba:
    def __init__(self, wartosc):
        self.liczba = wartosc

    def Podaj(self):
        return self.liczba


def Suma(a, b):
    return tablica[a.Podaj()] + tablica[b.Podaj()]
