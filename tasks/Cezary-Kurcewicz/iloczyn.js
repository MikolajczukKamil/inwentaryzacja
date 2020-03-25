
class Podawak {
    constructor(wartosc) {
      this.wartosc = wartosc
    }
  
    Podaj() {
      return this.wartosc;
    }
  }
  


const tablica=[0,1,2,3];

function Iloczyn(a,b){
    return tablica[a.Podaj()]*tablica[b.Podaj()];

}