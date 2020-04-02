tablica = [0, 1, 2, 3]
class Podawak:
	def __init__(self, value):
		self.value = value

	def podaj(self):
		return self.value

def dodawanie(a, b):
		return tablica[a.podaj()] + tablica[b.podaj()]