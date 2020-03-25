class Podawak {
    constructor(wartosc) {
      this.wartosc = wartosc
    }
  
    Podaj() {
      return this.wartosc;
    }
  }
  

function Iloczyn(a,b){
    return a.Podaj()*b.Podaj();
}