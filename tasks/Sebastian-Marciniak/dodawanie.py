class Podawak:
	def __init__(self, value):
		self.value = value

	def podaj(self):
		return self.value

def dodawanie(a, b):
	return a.podaj() + b.podaj()
