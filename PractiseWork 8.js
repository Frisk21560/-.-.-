// ЗАВДАННЯ 1: ЛОГІН

function vykonaty_login_1() {
    // Отримуємо значення з полів
    var login = document.getElementById('input-login-1').value.trim();
    var password = document.getElementById('input-password-1').value.trim();
    var ye_remember = document.getElementById('checkbox-remember-1').checked;

    // Перевіряємо чи заповнені поля
    if (login === '' || password === '') {
        alert('Будь ласка, заповніть логін та пароль!');
        return;
    }

    // Визначаємо текст про запам'ятування
    var zapamytatyy_tekst = ye_remember ? 'запам\'ятав' : 'не запам\'ятав';

    // Створюємо повідомлення
    var povidomlennya = 'Привіт, ' + login + '! Я тебе ' + zapamytatyy_tekst + '.';

    // Виводимо результат
    var rezultat_kontejner = document.getElementById('rezultat-1');
    rezultat_kontejner.innerHTML = '<p>' + povidomlennya + '</p>';
    rezultat_kontejner.classList.add('active');
}

// ЗАВДАННЯ 2: РЕЄСТРАЦІЯ

function vykonaty_signup_2() {
    // Отримуємо значення з полів
    var email = document.getElementById('input-email-2').value.trim();
    var login = document.getElementById('input-login-2').value.trim();
    var password = document.getElementById('input-password-2').value.trim();
    var repeat_password = document.getElementById('input-repeat-password-2').value.trim();

    // Перевіряємо чи заповнені всі поля
    if (email === '' || login === '' || password === '' || repeat_password === '') {
        alert('Будь ласка, заповніть всі поля!');
        return;
    }

    // Перевіряємо чи паролі збігаються
    if (password !== repeat_password) {
        alert('Паролі не збігаються!');
        return;
    }

    // Перевіряємо чи email правильного формату
    if (!email.includes('@')) {
        alert('Будь ласка, введіть правильну пошту!');
        return;
    }

    // Створюємо повідомлення
    var povidomlennya = 'На ' + email + ' надіслано лист із підтвердженням.';

    // Виводимо результат
    var rezultat_kontejner = document.getElementById('rezultat-2');
    rezultat_kontejner.innerHTML = '<p>' + povidomlennya + '</p>';
    rezultat_kontejner.classList.add('active');

    // Очищуємо форму
    document.getElementById('input-email-2').value = '';
    document.getElementById('input-login-2').value = '';
    document.getElementById('input-password-2').value = '';
    document.getElementById('input-repeat-password-2').value = '';
}

// ЗАВДАННЯ 3: ІНФОРМАЦІЯ ПРО КОРИСТУВАЧА

// Дані про міста в країнах
var dani_mist = {
    'Ukraine': ['Київ', 'Львів', 'Одеса', 'Харків'],
    'Canada': ['Toronto', 'Vancouver', 'Montreal', 'Calgary'],
    'USA': ['New York', 'Los Angeles', 'Chicago', 'Houston']
};

function onovyty_misty_3() {
    // Отримуємо вибрану країну
    var vybrana_krajijna = document.getElementById('select-country-3').value;
    var select_mist = document.getElementById('select-city-3');

    // Очищуємо select міст
    select_mist.innerHTML = '<option value="">Виберіть місто</option>';

    // Якщо вибрана країна
    if (vybrana_krajijna && dani_mist[vybrana_krajijna]) {
        // Отримуємо міста для цієї країни
        var misty = dani_mist[vybrana_krajijna];

        // Додаємо міста до select
        misty.forEach(function(misto) {
            var opciya = document.createElement('option');
            opciya.value = misto;
            opciya.textContent = misto;
            select_mist.appendChild(opciya);
        });
    }
}

function zberegty_informaciju_3() {
    // Отримуємо значення з усіх полів
    var firstname = document.getElementById('input-firstname-3').value.trim();
    var lastname = document.getElementById('input-lastname-3').value.trim();
    var birthday = document.getElementById('input-birthday-3').value;
    var gender = document.querySelector('input[name="gender-3"]:checked');
    var country = document.getElementById('select-country-3').value;
    var city = document.getElementById('select-city-3').value;

    // Отримуємо навички
    var vsi_skills = document.querySelectorAll('.skill-checkbox input[type="checkbox"]');
    var vybrani_skills = [];
    vsi_skills.forEach(function(skill) {
        if (skill.checked) {
            vybrani_skills.push(skill.value);
        }
    });

    // Перевіряємо чи заповнені обов'язкові поля
    if (firstname === '' || lastname === '' || birthday === '' || !gender || country === '' || city === '') {
        alert('Будь ласка, заповніть всі обов\'язкові поля!');
        return;
    }

    // Форматуємо дату народження
    var data_obj = new Date(birthday);
    var den = String(data_obj.getDate()).padStart(2, '0');
    var misyac = String(data_obj.getMonth() + 1).padStart(2, '0');
    var rik = data_obj.getFullYear();
    var data_formatovana = den + '.' + misyac + '.' + rik;

    // Форматуємо навички
    var skills_tekst = vybrani_skills.length > 0 ? vybrani_skills.join(', ') : 'Немає';

    // Створюємо таблицю з результатами
    var tablytsya_html = '<table>' +
                        '<tr><th>Firstname</th><td>' + firstname + '</td></tr>' +
                        '<tr><th>Lastname</th><td>' + lastname + '</td></tr>' +
                        '<tr><th>Birthday</th><td>' + data_formatovana + '</td></tr>' +
                        '<tr><th>Gender</th><td>' + gender.value + '</td></tr>' +
                        '<tr><th>Country</th><td>' + country + '</td></tr>' +
                        '<tr><th>City</th><td>' + city + '</td></tr>' +
                        '<tr><th>Skills</th><td>' + skills_tekst + '</td></tr>' +
                        '</table>';

    // Виводимо результат
    var rezultat_kontejner = document.getElementById('rezultat-3');
    rezultat_kontejner.innerHTML = tablytsya_html;
    rezultat_kontejner.classList.add('active');
}

// ЗАВДАННЯ 4: ПАЛІТРА КОЛЬОРІВ

// Масив для зберігання кольорів
var palitra_koloriv = [];

function dodaty_kolir_4() {
    // Отримуємо значення RGB
    var r = document.getElementById('input-r-4').value;
    var g = document.getElementById('input-g-4').value;
    var b = document.getElementById('input-b-4').value;

    // Перевіряємо чи всі значення введені
    if (r === '' || g === '' || b === '') {
        alert('Будь ласка, заповніть всі значення RGB!');
        return;
    }

    // Конвертуємо у числа
    r = parseInt(r);
    g = parseInt(g);
    b = parseInt(b);

    // Перевіряємо чи значення в межах 0-255
    if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255) {
        alert('Значення повинні бути від 0 до 255!');
        return;
    }

    // Створюємо об'єкт кольору
    var novyy_kolir = {
        r: r,
        g: g,
        b: b
    };

    // Додаємо колір до масиву
    palitra_koloriv.push(novyy_kolir);

    // Очищуємо поля введення
    document.getElementById('input-r-4').value = '';
    document.getElementById('input-g-4').value = '';
    document.getElementById('input-b-4').value = '';

    // Оновлюємо відображення палітри
    onovyty_palitru_4();
}

function onovyty_palitru_4() {
    // Отримуємо контейнер
    var kontejner = document.getElementById('paletka-list');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Проходимо по всіх кольорах
    palitra_koloriv.forEach(function(kolir) {
        // Створюємо елемент для кольору
        var elem_koliru = document.createElement('div');
        elem_koliru.className = 'color-item';

        // Створюємо RGB строку
        var rgb_string = 'rgb(' + kolir.r + ', ' + kolir.g + ', ' + kolir.b + ')';

        // Заповнюємо HTML
        elem_koliru.innerHTML = '<div class="color-preview" style="background-color: ' + rgb_string + ';"></div>' +
                               '<div class="color-info">RGB(' + kolir.r + ', ' + kolir.g + ', ' + kolir.b + ')</div>';

        // Додаємо елемент до контейнера
        kontejner.appendChild(elem_koliru);
    });
}

// ЗАВДАННЯ 5: КОНСТРУКТОР ТЕСТУ

// Масив для зберігання питань
var pytannya_test_5 = [];

function dodaty_pytannya_5() {
    // Отримуємо значення з полів
    var pytannya = document.getElementById('input-pytannya-5').value.trim();
    var pravilna_vidpovid = document.getElementById('input-pravilna-5').value.trim();
    var hybna_vidpovid = document.getElementById('input-hybna-5').value.trim();

    // Перевіряємо чи заповнені всі поля
    if (pytannya === '' || pravilna_vidpovid === '' || hybna_vidpovid === '') {
        alert('Будь ласка, заповніть всі поля!');
        return;
    }

    // Перевіряємо чи відповіді однакові
    if (pravilna_vidpovid === hybna_vidpovid) {
        alert('Правильна та неправильна відповіді не повинні бути однаковими!');
        return;
    }

    // Створюємо об'єкт питання
    var nove_pytannya = {
        tekst: pytannya,
        pravilna: pravilna_vidpovid,
        hybna: hybna_vidpovid
    };

    // Додаємо питання до масиву
    pytannya_test_5.push(nove_pytannya);

    // Очищуємо форму
    document.getElementById('input-pytannya-5').value = '';
    document.getElementById('input-pravilna-5').value = '';
    document.getElementById('input-hybna-5').value = '';

    // Оновлюємо відображення питань
    onovyty_pytannya_5();
}

function onovyty_pytannya_5() {
    // Отримуємо контейнер
    var kontejner = document.getElementById('pytannya-list-5');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Якщо нема питань
    if (pytannya_test_5.length === 0) {
        kontejner.innerHTML = '<p>Ще немає доданих питань</p>';
        return;
    }

    // Проходимо по всіх питаннях
    pytannya_test_5.forEach(function(pytannya, index) {
        // Створюємо елемент для питання
        var elem_pytannya = document.createElement('div');
        elem_pytannya.className = 'pytannya-item-5';

        // Заповнюємо HTML
        elem_pytannya.innerHTML = '<div class="pytannya-text-5">' + (index + 1) + '. ' + pytannya.tekst + '</div>' +
                                 '<div class="pytannya-vidpovid-5">Correct answer: ' + pytannya.pravilna + '</div>' +
                                 '<div class="pytannya-vidpovid-5">Wrong answer: ' + pytannya.hybna + '</div>';

        // Додаємо елемент до контейнера
        kontejner.appendChild(elem_pytannya);
    });
}

// НАВІГАЦІЯ

function zminyty_zavdannya(nomer) {
    // Ховаємо всі секції
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

// ІНІЦІАЛІЗАЦІЯ

document.addEventListener('DOMContentLoaded', function() {
    // Навігація
    document.querySelectorAll('.nav-button').forEach(function(knopka) {
        knopka.addEventListener('click', function() {
            var nomer_zavdannya = this.getAttribute('data-zavdannya');
            zminyty_zavdannya(nomer_zavdannya);
        });
    });

    // Завдання 1
    document.getElementById('button-signin-1').addEventListener('click', vykonaty_login_1);

    // Завдання 2
    document.getElementById('button-signup-2').addEventListener('click', vykonaty_signup_2);

    // Завдання 3
    document.getElementById('select-country-3').addEventListener('change', onovyty_misty_3);
    document.getElementById('button-save-3').addEventListener('click', zberegty_informaciju_3);

    // Завдання 4
    document.getElementById('button-add-color-4').addEventListener('click', dodaty_kolir_4);

    // Завдання 5
    document.getElementById('button-add-pytannya-5').addEventListener('click', dodaty_pytannya_5);
});