// Завдання 1: Масив для зберігання повідомлень форуму
var povidomlennya_forum = [];

// Завдання 2: Питання для тесту
var pytannya_test = [
    {
        tekst: 'Який рік вийшла мова JavaScript?',
        vidpovidi: ['1995', '2000'],
        pravilna_vidpovid: '1995'
    },
    {
        tekst: 'Хто розробив JavaScript?',
        vidpovidi: ['Brendan Eich', 'Guido van Rossum'],
        pravilna_vidpovid: 'Brendan Eich'
    },
    {
        tekst: 'Як правильно оголосити змінну в JavaScript?',
        vidpovidi: ['var zminna;', 'variable zminna;'],
        pravilna_vidpovid: 'var zminna;'
    }
];

// Завдання 4: Масив книг у магазині
var knyzhky_magazyn = [
    { id: 1, nazva: 'Learning JavaScript', tsina: 165 },
    { id: 2, nazva: 'You Don\'t Know JS', tsina: 205 },
    { id: 3, nazva: 'JavaScript and jQuery', tsina: 225 }
];

// Завдання 5: Данні про групи та пари
var dani_grupy = {
    'Група 1': {
        pary: ['1', '2', '3'],
        studenty: ['Студент 1', 'Студент 2', 'Студент 3']
    },
    'Група 2': {
        pary: ['1', '2', '3'],
        studenty: ['Студент 4', 'Студент 5', 'Студент 6']
    }
};

// Масив для зберігання відміток присутніх
var vidmitky_prysutnih = [];

// Завдання 6: Маршрути поїздів та квитки
var marshruty_poizdiv = {
    'Одеса - Львів': { tsina_za_mistsya: 100, mistsya_bud: 28 },
    'Київ - Харків': { tsina_za_mistsya: 120, mistsya_bud: 28 },
    'Москва - Санкт-Петербург': { tsina_za_mistsya: 80, mistsya_bud: 28 }
};

// Масив для зберігання заброньованих квитків
var zabroniovani_kvytky = [];

// Змінна для вибраної книги
var vybrana_knyzhka = null;

// Змінна для вибраних місць
var vybrani_mistsya = [];

// ФУНКЦІЇ ДЛЯ НАВІГАЦІЇ

// Функція для переходу між завданнями
function zminyty_zavdannya(nomer) {
    //Ховаємо всі секції завдань
    var vsi_sektsii = document.querySelectorAll('.zavdannya-section');
    vsi_sektsii.forEach(function(sekciya) {
        sekciya.classList.remove('active');
    });

    // Показуємо вибрану секцію
    document.getElementById('zavdannya-' + nomer).classList.add('active');

    // Оновлюємо активну кнопку навігації
    var vsi_knoply = document.querySelectorAll('.nav-button');
    vsi_knoply.forEach(function(knopka) {
        knopka.classList.remove('active');
    });
    document.querySelector('[data-zavdannya="' + nomer + '"]').classList.add('active');
}

// ЗАВДАННЯ 1: ФОРУМ

// Функція для додавання повідомлення
function dodaty_povidomlennya() {
    // Отримуємо значення з полів вводу
    var imya = document.getElementById('input-imya').value.trim();
    var tekst_povidomlennya = document.getElementById('input-povidomlennya').value.trim();

    // Перевіряємо, чи заповнені поля
    if (imya === '' || tekst_povidomlennya === '') {
        alert('Будь ласка, заповніть всі поля!');
        return;
    }

    // Отримуємо поточний час
    var zara = new Date();
    var hours = String(zara.getHours()).padStart(2, '0');
    var minutes = String(zara.getMinutes()).padStart(2, '0');
    var seconds = String(zara.getSeconds()).padStart(2, '0');
    var day = String(zara.getDate()).padStart(2, '0');
    var month = String(zara.getMonth() + 1).padStart(2, '0');
    var year = zara.getFullYear();
    var chas_formatovannyy = hours + ':' + minutes + ':' + seconds + ' ' + day + '.' + month + '.' + year;

    // Створюємо об'єкт повідомлення
    var nove_povidomlennya = {
        imya: imya,
        tekst: tekst_povidomlennya,
        chas: chas_formatovannyy
    };

    // Додаємо повідомлення до масиву
    povidomlennya_forum.push(nove_povidomlennya);

    // Очищуємо поля вводу
    document.getElementById('input-imya').value = '';
    document.getElementById('input-povidomlennya').value = '';

    // Оновлюємо відображення повідомлень
    onovyty_forum_vidobrazhення();
}

// Функція для оновлення відображення повідомлень
function onovyty_forum_vidobrazhення() {
    var kontejner = document.getElementById('povidomlennya-list');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Проходимо по всіх повідомленнях
    povidomlennya_forum.forEach(function(povidomlennya) {
        // Створюємо елемент для повідомлення
        var elem_povidomlennya = document.createElement('div');
        elem_povidomlennya.className = 'povidomlennya-item';

        // Заповнюємо HTML
        elem_povidomlennya.innerHTML = '<div class="imya">' + povidomlennya.imya + '</div>' +
                                       '<div class="chas">' + povidomlennya.chas + '</div>' +
                                       '<div class="tekst">' + povidomlennya.tekst + '</div>';

        // Додаємо елемент до контейнера
        kontejner.appendChild(elem_povidomlennya);
    });
}

// ЗАВДАННЯ 2: ТЕСТ

// Функція для ініціалізації тесту
function inicijalizuvaty_test() {
    var kontejner = document.getElementById('test-container');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Проходимо по всіх питаннях
    pytannya_test.forEach(function(pytannya, index) {
        // Створюємо елемент для питання
        var elem_pytannya = document.createElement('div');
        elem_pytannya.className = 'pytannya-test';

        // Створюємо текст питання
        var pytannya_tekst = document.createElement('div');
        pytannya_tekst.className = 'pytannya-text';
        pytannya_tekst.textContent = (index + 1) + '. ' + pytannya.tekst;
        elem_pytannya.appendChild(pytannya_tekst);

        // Створюємо контейнер для варіантів
        var varianty_kontejner = document.createElement('div');
        varianty_kontejner.className = 'varianty-vidpovidi';

        // Проходимо по варіантам відповідей
        pytannya.vidpovidi.forEach(function(vidpovid) {
            // Створюємо label для варіанту
            var label = document.createElement('label');

            // Створюємо radio-button
            var radio = document.createElement('input');
            radio.type = 'radio';
            radio.name = 'pytannya-' + index;
            radio.value = vidpovid;

            // Додаємо radio та текст до label
            label.appendChild(radio);
            label.appendChild(document.createTextNode(vidpovid));

            // Додаємо label до контейнера
            varianty_kontejner.appendChild(label);
        });

        // Додаємо варіанти до питання
        elem_pytannya.appendChild(varianty_kontejner);

        // Додаємо питання до контейнера
        kontejner.appendChild(elem_pytannya);
    });
}

// Функція для перевірки тесту
function pereveryty_test() {
    var kilkist_pravilnyh = 0;

    // Проходимо по всіх питаннях
    pytannya_test.forEach(function(pytannya, index) {
        // Отримуємо вибрану відповідь
        var vybrany_radio = document.querySelector('input[name="pytannya-' + index + '"]:checked');

        // Якщо відповідь вибрана
        if (vybrany_radio) {
            // Перевіряємо чи вона правильна
            if (vybrany_radio.value === pytannya.pravilna_vidpovid) {
                kilkist_pravilnyh++;
            }
        }
    });

    // Виводимо результат
    var rezultat_kontejner = document.getElementById('rezultat-test');
    rezultat_kontejner.innerHTML = '<p>Ви відповіли правильно на ' + kilkist_pravilnyh + ' із ' + pytannya_test.length + ' питань</p>';
    rezultat_kontejner.classList.add('active');
}

// ЗАВДАННЯ 3: СТИЛІЗОВАНИЙ ТЕКСТ

// Функція для показу стилізованого тексту
function pokazaty_stylizovannyy_tekst() {
    // Отримуємо текст
    var tekst = document.getElementById('input-tekst-3').value;

    // Якщо текст пустий
    if (tekst.trim() === '') {
        alert('Будь ласка, введіть текст!');
        return;
    }

    // Отримуємо вибрані стилі
    var ye_bold = document.getElementById('chekbox-bold').checked;
    var ye_underline = document.getElementById('chekbox-underline').checked;
    var ye_italic = document.getElementById('chekbox-italic').checked;
    var vyrivnyannya = document.querySelector('input[name="vyrivnyannya"]:checked');

    // Якщо не вибрано жодне вирівнювання
    if (!vyrivnyannya) {
        alert('Будь ласка, виберіть вирівнювання!');
        return;
    }

    var vyrivnyannya_znachennya = vyrivnyannya.value;

    // Створюємо div для результату
    var rezultat_div = document.getElementById('rezultat-3');
    rezultat_div.innerHTML = '';

    // Створюємо елемент для відображення тексту
    var vyvedennyy_tekst = document.createElement('div');
    vyvedennyy_tekst.className = 'vyvedennyy-tekst';
    vyvedennyy_tekst.textContent = tekst;

    // Додаємо стилі
    if (ye_bold) {
        vyvedennyy_tekst.style.fontWeight = 'bold';
    }
    if (ye_underline) {
        vyvedennyy_tekst.style.textDecoration = 'underline';
    }
    if (ye_italic) {
        vyvedennyy_tekst.style.fontStyle = 'italic';
    }

    vyvedennyy_tekst.style.textAlign = vyrivnyannya_znachennya;

    // Додаємо елемент до контейнера
    rezultat_div.appendChild(vyvedennyy_tekst);
}

// ЗАВДАННЯ 4: МАГАЗИН КНИГ

// Функція для ініціалізації магазину книг
function inicijalizuvaty_magazyn() {
    var kontejner = document.getElementById('knyzhky-list');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Проходимо по всіх книжках
    knyzhky_magazyn.forEach(function(knyzhka) {
        // Створюємо картку книги
        var karta_knyzhky = document.createElement('div');
        karta_knyzhky.className = 'knyzhka-card';
        karta_knyzhky.id = 'knyzhka-' + knyzhka.id;

        // Заповнюємо HTML картки
        karta_knyzhky.innerHTML = '<div class="knyzhka-nazva">' + knyzhka.nazva + '</div>' +
                                  '<div class="knyzhka-tsina">Ціна: ' + knyzhka.tsina + ' грн.</div>';

        // Додаємо обробник кліку
        karta_knyzhky.addEventListener('click', function() {
            vybrary_knyzhku(knyzhka.id);
        });

        // Додаємо картку до контейнера
        kontejner.appendChild(karta_knyzhky);
    });
}

// Функція для вибору книги
function vybrary_knyzhku(id_knyzhky) {
    // Видаляємо клас "вибрана" з попередньої книги
    if (vybrana_knyzhka) {
        document.getElementById('knyzhka-' + vybrana_knyzhka).classList.remove('vybrana');
    }

    // Встановлюємо нову вибрану книгу
    vybrana_knyzhka = id_knyzhky;

    // Додаємо клас "вибрана" до нової книги
    document.getElementById('knyzhka-' + id_knyzhky).classList.add('vybrana');
}

// Функція для оформлення замовлення
function oformyty_zamovlennya() {
    // Перевіряємо чи вибрана книга
    if (!vybrana_knyzhka) {
        alert('Будь ласка, виберіть книгу!');
        return;
    }

    // Отримуємо дані з полів
    var kilkist = document.getElementById('input-kilkist').value;
    var imya = document.getElementById('input-imya-4').value.trim();
    var data_dostavky = document.getElementById('input-data-dostavky').value;
    var adresa = document.getElementById('input-adresa').value.trim();
    var komentar = document.getElementById('input-komentar').value.trim();

    // Перевіряємо чи всі поля заповнені
    if (kilkist === '' || imya === '' || data_dostavky === '' || adresa === '') {
        alert('Будь ласка, заповніть всі обов\'язкові поля!');
        return;
    }

    // Знаходимо вибрану книгу
    var knyzhka = knyzhky_magazyn.find(function(k) { return k.id === vybrana_knyzhka; });

    // Форматуємо дату для спеціальної функції
    var data_obj = new Date(data_dostavky);
    var den = String(data_obj.getDate()).padStart(2, '0');
    var misyac = String(data_obj.getMonth() + 1).padStart(2, '0');
    var rik = data_obj.getFullYear();
    var data_formatovana = den + '.' + misyac + '.' + rik;

    // Створюємо повідомлення про замовлення
    var povidomlennya = imya + ', дякуємо за замовлення. ' +
                        knyzhka.nazva + ' (' + kilkist + ' шт.) ' +
                        'буде доставлено ' + data_formatovana + ' за адресою: ' + adresa + '.';

    if (komentar !== '') {
        povidomlennya += ' Ваш коментар: ' + komentar + '.';
    }

    // Виводимо повідомлення
    var rezultat_kontejner = document.getElementById('rezultat-4');
    rezultat_kontejner.innerHTML = '<p>' + povidomlennya + '</p>';
    rezultat_kontejner.style.display = 'block';

    // Очищуємо форму
    document.getElementById('input-kilkist').value = '';
    document.getElementById('input-imya-4').value = '';
    document.getElementById('input-data-dostavky').value = '';
    document.getElementById('input-adresa').value = '';
    document.getElementById('input-komentar').value = '';
}

// ЗАВДАННЯ 5: ВІДМІТКА ПРИСУТНІХ

// Функція для ініціалізації вибору групи та пари
function inicijalizuvaty_vidmitku() {
    var select_grupa = document.getElementById('select-grupa-5');
    // Очищуємо select
    select_grupa.innerHTML = '<option value="">Виберіть групу</option>';

    // Додаємо групи до select
    for (var grupa in dani_grupy) {
        var opciya = document.createElement('option');
        opciya.value = grupa;
        opciya.textContent = grupa;
        select_grupa.appendChild(opciya);
    }
}

// Функція для оновлення пар
function onovyty_pary() {
    var select_grupa = document.getElementById('select-grupa-5');
    var vybrana_grupa = select_grupa.value;
    var select_para = document.getElementById('select-para-5');

    // Очищуємо select пари
    select_para.innerHTML = '<option value="">Виберіть пару</option>';

    // Якщо вибрана група
    if (vybrana_grupa) {
        // Отримуємо пари для цієї групи
        var pary = dani_grupy[vybrana_grupa].pary;

        // Додаємо пари до select
        pary.forEach(function(para) {
            var opciya = document.createElement('option');
            opciya.value = para;
            opciya.textContent = 'Пара ' + para;
            select_para.appendChild(opciya);
        });
    }
}

// Функція для показу форми відмітки
function pokazaty_formu_vidmitky() {
    var vybrana_grupa = document.getElementById('select-grupa-5').value;
    var vybrana_para = document.getElementById('select-para-5').value;

    // Перевіряємо чи вибрані група та пара
    if (!vybrana_grupa || !vybrana_para) {
        alert('Будь ласка, виберіть групу та пару!');
        return;
    }

    // Показуємо форму
    document.getElementById('forma-vidmitky').style.display = 'block';

    // Отримуємо студентів
    var studenty = dani_grupy[vybrana_grupa].studenty;
    var kontejner = document.getElementById('studenty-list');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Проходимо по студентах
    studenty.forEach(function(student, index) {
        // Створюємо елемент чекбокса
        var elem_checkbox = document.createElement('div');
        elem_checkbox.className = 'student-chekbox';

        // Створюємо checkbox
        var checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.id = 'student-checkbox-' + index;
        checkbox.value = student;

        // Створюємо label
        var label = document.createElement('label');
        label.htmlFor = 'student-checkbox-' + index;
        label.textContent = student;

        // Додаємо checkbox та label до елемента
        elem_checkbox.appendChild(checkbox);
        elem_checkbox.appendChild(label);

        // Додаємо елемент до контейнера
        kontejner.appendChild(elem_checkbox);
    });
}

// Функція для збереження відмітки
function zberegty_vidmitku() {
    var vybrana_grupa = document.getElementById('select-grupa-5').value;
    var vybrana_para = document.getElementById('select-para-5').value;
    var tema = document.getElementById('input-tema-5').value.trim();

    // Перевіряємо чи заповнена тема
    if (tema === '') {
        alert('Будь ласка, введіть тему заняття!');
        return;
    }

    // Отримуємо присутніх студентів
    var prysutni_studenty = [];
    var vsi_checkboksy = document.querySelectorAll('#studenty-list input[type="checkbox"]');
    vsi_checkboksy.forEach(function(checkbox) {
        if (checkbox.checked) {
            prysutni_studenty.push(checkbox.value);
        }
    });

    // Створюємо об'єкт для відмітки
    var nova_vidmitka = {
        grupa: vybrana_grupa,
        para: vybrana_para,
        tema: tema,
        prysutni: prysutni_studenty,
        data: nova_data_dlya_vidmitky()
    };

    // Додаємо відмітку до масиву
    vidmitky_prysutnih.push(nova_vidmitka);

    // Оновлюємо відображення
    onovyty_vidmitky_vidobrazhення();

    // Очищуємо форму
    document.getElementById('input-tema-5').value = '';
    vsi_checkboksy.forEach(function(checkbox) {
        checkbox.checked = false;
    });
    document.getElementById('forma-vidmitky').style.display = 'none';

    alert('Відмітка збережена!');
}

// Функція для отримання поточної дати
function nova_data_dlya_vidmitky() {
    var data = new Date();
    var day = String(data.getDate()).padStart(2, '0');
    var month = String(data.getMonth() + 1).padStart(2, '0');
    var year = data.getFullYear();
    return day + '.' + month + '.' + year;
}

// Функція для оновлення відображення відміток
function onovyty_vidmitky_vidobrazhення() {
    var kontejner = document.getElementById('glyan-vidmitky-list');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Якщо немає відміток
    if (vidmitky_prysutnih.length === 0) {
        kontejner.innerHTML = '<p>Ще немає збережених відміток</p>';
        return;
    }

    // Проходимо по відмітках
    vidmitky_prysutnih.forEach(function(vidmitka) {
        // Створюємо елемент для відмітки
        var elem_vidmitka = document.createElement('div');
        elem_vidmitka.className = 'vidmitka-zapys';

        // Форматуємо присутніх студентів
        var prysutni_tekst = vidmitka.prysutni.join(', ');

        // Заповнюємо HTML
        elem_vidmitka.innerHTML = '<strong>' + vidmitka.grupa + ', Пара ' + vidmitka.para + '</strong><br>' +
                                  'Тема: ' + vidmitka.tema + '<br>' +
                                  'Присутні: ' + prysutni_tekst + '<br>' +
                                  'Дата: ' + vidmitka.data;

        // Додаємо елемент до контейнера
        kontejner.appendChild(elem_vidmitka);
    });
}

// ЗАВДАННЯ 6: БРОНІЮВАННЯ КВИТКІВ

// Функція для ініціалізації маршрутів
function inicijalizuvaty_marshruty() {
    var select_marshrutу = document.getElementById('select-marshrutа-6');
    // Очищуємо select
    select_marshrutу.innerHTML = '<option value="">Виберіть напрямок</option>';

    // Додаємо маршрути до select
    for (var marshrutу in marshruty_poizdiv) {
        var opciya = document.createElement('option');
        opciya.value = marshrutу;
        opciya.textContent = marshrutу;
        select_marshrutу.appendChild(opciya);
    }
}

// Функція для пошуку поїзда
function poshukaty_poizd() {
    var vybrannyy_marshrutу = document.getElementById('select-marshrutа-6').value;
    var vybrana_data = document.getElementById('input-data-6').value;

    // Перевіряємо чи вибрані маршрут та дата
    if (!vybrannyy_marshrutу || !vybrana_data) {
        alert('Будь ласка, виберіть маршрут та дату!');
        return;
    }

    // Показуємо сітку місць
    document.getElementById('mistsya-dlya-brony').style.display = 'block';

    // Очищуємо вибрані місця
    vybrani_mistsya = [];

    // Отримуємо дані про маршрут
    var dani_marshrutu = marshruty_poizdiv[vybrannyy_marshrutу];

    // Генеруємо сітку місць
    generuvaty_sitku_mists(dani_marshrutu.mistsya_bud, dani_marshrutu.tsina_za_mistsya);
}

// Функція для генерування сітки місць
function generuvaty_sitku_mists(kilkist_mists, tsina_za_mistsya) {
    var kontejner = document.getElementById('sitka-mists');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Отримуємо вже заброньовані місця
    var zabronovani_mistsya = otrymaty_zabronovani_mistsya();

    // Проходимо по всіх місцях
    for (var i = 1; i <= kilkist_mists; i++) {
        // Створюємо кнопку для місця
        var knopka_mistsya = document.createElement('button');
        knopka_mistsya.type = 'button';
        knopka_mistsya.className = 'mistsya-button';
        knopka_mistsya.textContent = i;
        knopka_mistsya.id = 'mistsya-' + i;

        // Перевіряємо чи місце уже заброньоване
        if (zabronovani_mistsya.includes(i)) {
            knopka_mistsya.classList.add('zbronovane');
            knopka_mistsya.disabled = true;
        } else {
            // Додаємо обробник кліку
            knopka_mistsya.addEventListener('click', function() {
                vybrary_mistsya(i, tsina_za_mistsya);
            });
        }

        // Додаємо кнопку до контейнера
        kontejner.appendChild(knopka_mistsya);
    }

    // Оновлюємо загальну ціну
    onovyty_zagalnu_tsinu(tsina_za_mistsya);
}

// Функція для вибору місця
function vybrary_mistsya(nomer_mistsya, tsina_za_mistsya) {
    var knopka = document.getElementById('mistsya-' + nomer_mistsya);

    // Перевіряємо чи місце вже вибране
    if (vybrani_mistsya.includes(nomer_mistsya)) {
        // Видаляємо місце з вибраних
        vybrani_mistsya = vybrani_mistsya.filter(function(m) { return m !== nomer_mistsya; });
        knopka.classList.remove('vybrane');
    } else {
        // Додаємо місце до вибраних
        vybrani_mistsya.push(nomer_mistsya);
        knopka.classList.add('vybrane');
    }

    // Оновлюємо загальну ціну
    onovyty_zagalnu_tsinu(tsina_za_mistsya);
}

// Функція для оновлення загальної ціни
function onovyty_zagalnu_tsinu(tsina_za_mistsya) {
    var zagalna_tsina = vybrani_mistsya.length * tsina_za_mistsya;
    document.getElementById('zagalna-tsina-6').textContent = zagalna_tsina;
}

// Функція для отримання заброньованих місць
function otrymaty_zabronovani_mistsya() {
    var vybrannyy_marshrutу = document.getElementById('select-marshrutа-6').value;
    var zabronovani_mistsya = [];

    // Проходимо по всіх квитках
    zabroniovani_kvytky.forEach(function(kvytok) {
        // Якщо маршрут збігається
        if (kvytok.marshrutу === vybrannyy_marshrutу) {
            zabronovani_mistsya.push(kvytok.mistsya);
        }
    });

    return zabronovani_mistsya;
}

// Функція для броніювання квитків
function bronuvaty_kvytky() {
    var vybrannyy_marshrutу = document.getElementById('select-marshrutа-6').value;
    var vybrana_data = document.getElementById('input-data-6').value;

    // Перевіряємо чи вибрані місця
    if (vybrani_mistsya.length === 0) {
        alert('Будь ласка, виберіть хоча б одне місце!');
        return;
    }

    // Форматуємо дату
    var data_obj = new Date(vybrana_data);
    var den = String(data_obj.getDate()).padStart(2, '0');
    var misyac = String(data_obj.getMonth() + 1).padStart(2, '0');
    var rik = data_obj.getFullYear();
    var data_formatovana = den + '.' + misyac + '.' + rik;

    // Проходимо по вибраних місцях
    vybrani_mistsya.forEach(function(mistsya) {
        // Створюємо об'єкт для квитка
        var novyy_kvytok = {
            marshrutу: vybrannyy_marshrutу,
            data: data_formatovana,
            mistsya: mistsya
        };

        // Додаємо квиток до масиву
        zabroniovani_kvytky.push(novyy_kvytok);
    });

    // Оновлюємо відображення квитків
    onovyty_kvytky_vidobrazhення();

    // Очищуємо форму
    vybrani_mistsya = [];
    document.getElementById('select-marshrutа-6').value = '';
    document.getElementById('input-data-6').value = '';
    document.getElementById('mistsya-dlya-brony').style.display = 'none';

    alert('Квитки успішно заброньовані!');
}

// Функція для оновлення відображення квитків
function onovyty_kvytky_vidobrazhення() {
    var kontejner = document.getElementById('glyan-kvytky-list');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Якщо немає квитків
    if (zabroniovani_kvytky.length === 0) {
        kontejner.innerHTML = '<p>Ще немає заброньованих квитків</p>';
        return;
    }

    // Створюємо таблицю
    var tablytsya = document.createElement('table');
    tablytsya.className = 'kvytok-table';

    // Створюємо заголовок таблиці
    var zagolovok = document.createElement('thead');
    var ryadok_zagolovka = document.createElement('tr');
    ryadok_zagolovka.innerHTML = '<th>Напрямок</th><th>Дата</th><th>Місце</th>';
    zagolovok.appendChild(ryadok_zagolovka);
    tablytsya.appendChild(zagolovok);

    // Створюємо тіло таблиці
    var tilo = document.createElement('tbody');

    // Проходимо по всіх квитках
    zabroniovani_kvytky.forEach(function(kvytok) {
        // Створюємо рядок таблиці
        var ryadok = document.createElement('tr');
        ryadok.innerHTML = '<td>' + kvytok.marshrutу + '</td>' +
                           '<td>' + kvytok.data + '</td>' +
                           '<td>' + kvytok.mistsya + '</td>';

        // Додаємо рядок до тіла таблиці
        tilo.appendChild(ryadok);
    });

    tablytsya.appendChild(tilo);

    // Додаємо таблицю до контейнера
    kontejner.appendChild(tablytsya);
}

// ІНІЦІАЛІЗАЦІЯ

// Обробник подій для навігації
document.addEventListener('DOMContentLoaded', function() {
    // Навігація
    document.querySelectorAll('.nav-button').forEach(function(knopka) {
        knopka.addEventListener('click', function() {
            var nomer_zavdannya = this.getAttribute('data-zavdannya');
            zminyty_zavdannya(nomer_zavdannya);
        });
    });

    // Завдання 1
    document.getElementById('button-dodaty-1').addEventListener('click', dodaty_povidomlennya);

    // Завдання 2
    inicijalizuvaty_test();
    document.getElementById('button-vidpravyty-test').addEventListener('click', pereveryty_test);

    // Завдання 3
    document.getElementById('button-pokazaty-3').addEventListener('click', pokazaty_stylizovannyy_tekst);

    // Завдання 4
    inicijalizuvaty_magazyn();
    document.getElementById('button-zamovyty').addEventListener('click', oformyty_zamovlennya);

    // Завдання 5
    inicijalizuvaty_vidmitku();
    document.getElementById('select-grupa-5').addEventListener('change', onovyty_pary);
    document.getElementById('button-vybrat-5').addEventListener('click', pokazaty_formu_vidmitky);
    document.getElementById('button-zberegty-5').addEventListener('click', zberegty_vidmitku);

    // Завдання 6
    inicijalizuvaty_marshruty();
    document.getElementById('button-poshuk-6').addEventListener('click', poshukaty_poizd);
    document.getElementById('button-bronuvaty-6').addEventListener('click', bronuvaty_kvytky);
});