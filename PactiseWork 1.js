// Zavdannya 1: Chyslo v 2 stupin
var chyslo = prompt('Vvedit chyslo:');
var v_kvadrati = chyslo * chyslo;
alert('Chyslo ' + chyslo + ' v 2 stupini = ' + v_kvadrati);

// Zavdannya 2: Seredne aryfmetychne
var chyslo1 = prompt('Vvedit pershe chyslo:');
var chyslo2 = prompt('Vvedit druge chyslo:');
var seredne = (chyslo1 + chyslo2) / 2;
alert('Seredne aryfmetychne: ' + seredne);

// Zavdannya 3: Ploshcha kvadrata
var storona = prompt('Vvedit dovzhynu storony kvadrata:');
var ploshcha = storona * storona;
alert('Ploshcha kvadrata: ' + ploshcha);

// Zavdannya 4: Konvertor km u myli
var km = prompt('Vvedit vidstan u kilometrakh:');
const koeficiyent_myli = 0.621371;
var myli = km * koeficiyent_myli;
alert(km + ' km = ' + myli + ' myl');

// Zavdannya 5: Kalkulator
var chyslo_a = prompt('Vvedit pershe chyslo:');
var chyslo_b = prompt('Vvedit druge chyslo:');
var prybutok = chyslo_a + chyslo_b;
var riznytsya = chyslo_a - chyslo_b;
var dobutok = chyslo_a * chyslo_b;
var chastka = chyslo_a / chyslo_b;
alert('Prybutok: ' + prybutok + '\nRiznytsya: ' + riznytsya + '\nDobutok: ' + dobutok + '\nChastka: ' + chastka);

// Zavdannya 6: Linijne rivnyannya a*x+b=0
var a = prompt('Vvedit koeficiyent a:');
var b = prompt('Vvedit koeficiyent b:');
var x = -b / a;
alert('Rishennya rivnyannya: x = ' + x);

// Zavdannya 7: Chas do kincya doby
var godyny = prompt('Vvedit potozhn godyny:');
var khvylynu = prompt('Vvedit potozhn khvylynu:');
var hodynnogo_zalyshheno = 23 - godyny;
var khvylyny_zalyshheno = 60 - khvylynu;
alert('Do kincya doby zalyshheno: ' + hodynnogo_zalyshheno + ' godn ' + khvylyny_zalyshheno + ' khv');

// Zavdannya 8: Druga tsyfra tryzhnachnogo chysla
var tryzhnachne = prompt('Vvedit tryzhnachne chyslo:');
var desyatky = (tryzhnachne % 100) / 10;
alert('Druga tsyfra: ' + desyatky);

// Zavdannya 9: Peremischennya ostannoi tsyfry v pochatok
var pyatyzhnachne = prompt('Vvedit pyatyzhnachne chyslo:');
var ostannya_tsyfra = pyatyzhnachne % 10;
var reshta_chysla = pyatyzhnachne / 10;
var nove_chyslo = ostannya_tsyfra * 10000 + reshta_chysla;
alert('Nove chyslo: ' + nove_chyslo);

// Zavdannya 10: Zarplata pracivnyka
var prodazhi = prompt('Vvedit zagalnu sumu prodazhu za misyac:');
var bazova_zarplata = 250;
var procenty_prodazhu = prodazhi * 0.1;
var zarplata_vsyogo = bazova_zarplata + procenty_prodazhu;
alert('Zarplata pracivnyka: ' + zarplata_vsyogo + ' dolariv');