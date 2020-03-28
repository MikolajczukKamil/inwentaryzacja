// JS

class Podawak {
  constructor(wartosc) {
    this.wartosc = wartosc
  }

  Podaj() {
    return this.wartosc;
  }
}

function Suma(a, b) {
  return a.Podaj() + b.Podaj()
}
