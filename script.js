// Zavdannya 1: Ime koristyvacha
var imya = prompt('Yake vashe imya?');
alert('Pryvit, ' + imya + '!');

// Zavdannya 2: Vick koristyvacha
var rik = prompt('Yak vash rik narodzhennya?');
const potozhniy_rik = 2026;
var vick = potozhniy_rik - rik;
alert('Vashe vick: ' + vick + ' rokiv');

// Zavdannya 3: Perimetr kvadrata
var storona_kvadrata = prompt('Vvedit dovzhynu storony kvadrata (m):');
var perimetr_kvadrata = 4 * storona_kvadrata;
alert('Perimetr kvadrata: ' + perimetr_kvadrata + ' m');

// Zavdannya 4: Ploshcha kola
var radius_kola = prompt('Vvedit radius kola (m):');
var ploshcha_kola = 3.14 * radius_kola * radius_kola;
alert('Ploshcha kola: ' + ploshcha_kola + ' m2');

// Zavdannya 5: Shvydkist rukhu
var vidstan_km = prompt('Vvedit vidstan mizh mistamy (km):');
var chasy = prompt('Za skilky hodyn vy khochete distatsya?');
var shvydkist = vidstan_km / chasy;
alert('Neobhidna shvydkist: ' + shvydkist + ' km/h');

// Zavdannya 6: Konvertor valyut
var dolary = prompt('Vvedit sumu v dolarakh:');
const kurs_dolar = 0.95;
var evro = dolary * kurs_dolar;
alert(dolary + ' USD = ' + evro + ' EUR');

// Zavdannya 7: Fleshka
var obsyag_gb = prompt('Vvedit obsyag fleshky (GB):');
var obsyag_mb = obsyag_gb * 1024;
var rozmir_fajlu_mb = 820;
var kilkist_fajliv = Math.floor(obsyag_mb / rozmir_fajlu_mb);
alert('Na fleshku vmistytesya ' + kilkist_fajliv + ' fajliv po 820 MB');

// Zavdannya 8: Chokoladky
var groshi_gamanets = prompt('Skilky groshej u vashomu gamanzi?');
var vartist_shokoladky = prompt('Yaka vartist odniyei shokoladky?');
var kilkist_shokoladok = Math.floor(groshi_gamanets / vartist_shokoladky);
var zdicha = groshi_gamanets - (kilkist_shokoladok * vartist_shokoladky);
alert('Vy mozhete kupyty ' + kilkist_shokoladok + ' shokoladok. Zdacha: ' + zdicha);

// Zavdannya 9: Palindrom
var trizhnachne_chyslo = prompt('Vvedit trizhnachne chyslo:');
var pershe_tsyfry = Math.floor(trizhnachne_chyslo / 100);
var druhe_tsyfry = Math.floor((trizhnachne_chyslo % 100) / 10);
var tretye_tsyfry = trizhnachne_chyslo % 10;
var palindrom = tretye_tsyfry * 100 + druhe_tsyfry * 10 + pershe_tsyfry;
alert('Palindrom chysla ' + trizhnachne_chyslo + ': ' + palindrom);

// Zavdannya 10: Parne chy neparne
var tsite_chyslo = prompt('Vvedit tsite chyslo:');
var parne = tsite_chyslo % 2 === 0;
if (parne) {
    alert('Parne chyslo');
} else {
    alert('Neparne chyslo');
}