// ФУНКЦІЇ ДЛЯ РОБОТИ З COOKIE

// Функція для встановлення Cookie
function vstanovyty_cookie(imya, znachennya, godyny) {
    // Створюємо дату закінчення
    var data_zakinchennya = new Date();
    data_zakinchennya.setTime(data_zakinchennya.getTime() + (godyny * 60 * 60 * 1000));
    var expires = "expires=" + data_zakinchennya.toUTCString();

    // Встановлюємо Cookie
    document.cookie = imya + "=" + encodeURIComponent(JSON.stringify(znachennya)) + ";" + expires + ";path=/";
}

// Функція для отримання Cookie
function otrymaty_cookie(imya) {
    var cookies_string = document.cookie;
    var cookies_array = cookies_string.split(';');

    for (var i = 0; i < cookies_array.length; i++) {
        var cookie = cookies_array[i].trim();

        if (cookie.indexOf(imya + '=') === 0) {
            var cookie_value = cookie.substring(imya.length + 1);
            try {
                return JSON.parse(decodeURIComponent(cookie_value));
            } catch (e) {
                return null;
            }
        }
    }

    return null;
}

// Функція для видалення Cookie
function vydalyty_cookie(imya) {
    document.cookie = imya + "=;expires=Thu, 01 Jan 1970 00:00:00 UTC;path=/;";
}

// ФУНКЦІЇ ДЛЯ ВАЛІДАЦІЇ EMAIL

function pereveryty_email(email) {
    // Очищуємо помилку
    document.getElementById('error-email').textContent = '';

    // Перевіряємо чи заповнено
    if (email.trim() === '') {
        document.getElementById('error-email').textContent = 'Email is required';
        return false;
    }

    // Перевіряємо чи містить @
    if (!email.includes('@')) {
        document.getElementById('error-email').textContent = 'Wrong email address';
        return false;
    }

    // Розділяємо email на частини
    var parts = email.split('@');

    // Перевіряємо кількість частин
    if (parts.length !== 2) {
        document.getElementById('error-email').textContent = 'Wrong email address';
        return false;
    }

    var lokalna_chastyna = parts[0];
    var domen = parts[1];

    // Перевіряємо локальну частину (мінімум 3 символи)
    if (lokalna_chastyna.length < 3) {
        document.getElementById('error-email').textContent = 'Wrong email address';
        return false;
    }

    // Перевіряємо символи у локальній частині
    var valid_chars = /^[a-zA-Z0-9._-]+$/;
    if (!valid_chars.test(lokalna_chastyna)) {
        document.getElementById('error-email').textContent = 'Wrong email address';
        return false;
    }

    // Перевіряємо домен
    if (domen.trim() === '') {
        document.getElementById('error-email').textContent = 'Wrong email address';
        return false;
    }

    // Перевіряємо символи у домені
    if (!valid_chars.test(domen)) {
        document.getElementById('error-email').textContent = 'Wrong email address';
        return false;
    }

    return true;
}

// ФУНКЦІЇ ДЛЯ ВАЛІДАЦІЇ ПАРОЛЯ

function pereveryty_password(password) {
    // Очищуємо помилку
    document.getElementById('error-password').textContent = '';

    // Перевіряємо чи заповнено
    if (password.trim() === '') {
        document.getElementById('error-password').textContent = 'Password is required';
        return false;
    }

    // Перевіряємо довжину
    if (password.length < 6) {
        document.getElementById('error-password').textContent = 'Password must be at least 6 characters';
        return false;
    }

    // Перевіряємо наявність малої букви
    if (!/[a-z]/.test(password)) {
        document.getElementById('error-password').textContent = 'Password must contain a lowercase letter';
        return false;
    }

    // Перевіряємо наявність великої букви
    if (!/[A-Z]/.test(password)) {
        document.getElementById('error-password').textContent = 'Password must contain an uppercase letter';
        return false;
    }

    // Перевіряємо наявність цифри
    if (!/[0-9]/.test(password)) {
        document.getElementById('error-password').textContent = 'Password must contain a digit';
        return false;
    }

    return true;
}

// ФУНКЦІЇ ДЛЯ ВАЛІДАЦІЇ ПОВТОРУ ПАРОЛЯ

function pereveryty_repeat_password(password, repeat_password) {
    // Очищуємо помилку
    document.getElementById('error-repeat').textContent = '';

    // Перевіряємо чи заповнено
    if (repeat_password.trim() === '') {
        document.getElementById('error-repeat').textContent = 'Repeat password is required';
        return false;
    }

    // Перевіряємо чи збігаються паролі
    if (password !== repeat_password) {
        document.getElementById('error-repeat').textContent = 'Passwords must match';
        return false;
    }

    return true;
}

// ФУНКЦІЇ ДЛЯ СТОРІНКИ 1: РЕЄСТРАЦІЯ

function vykonaty_signup() {
    // Отримуємо значення з полів
    var email = document.getElementById('input-email').value;
    var password = document.getElementById('input-password').value;
    var repeat_password = document.getElementById('input-repeat-password').value;

    // Валідуємо дані
    var email_valid = pereveryty_email(email);
    var password_valid = pereveryty_password(password);
    var repeat_valid = pereveryty_repeat_password(password, repeat_password);

    // Якщо всі дані правильні
    if (email_valid && password_valid && repeat_valid) {
        // Зберігаємо дані в Cookie
        var registration_data = {
            email: email,
            password: password
        };

        vstanovyty_cookie('registration_data', registration_data, 1);

        // Переходимо на другу сторінку
        perejty_na_profile();
    }
}

// ФУНКЦІЇ ДЛЯ СТОРІНКИ 2: ПРОФІЛЬ

function vykonaty_save() {
    // Отримуємо значення з полів
    var firstname = document.getElementById('input-firstname').value.trim();
    var lastname = document.getElementById('input-lastname').value.trim();
    var year = document.getElementById('input-year').value.trim();
    var gender = document.getElementById('select-gender').value;
    var phone = document.getElementById('input-phone').value.trim();
    var skype = document.getElementById('input-skype').value.trim();

    // Очищуємо всі помилки
    document.getElementById('error-firstname').textContent = '';
    document.getElementById('error-lastname').textContent = '';
    document.getElementById('error-year').textContent = '';
    document.getElementById('error-gender').textContent = '';
    document.getElementById('error-phone').textContent = '';
    document.getElementById('error-skype').textContent = '';

    // Лічильник помилок
    var ye_oshybky = false;

    // Валідуємо ім'я
    if (firstname === '') {
        document.getElementById('error-firstname').textContent = 'First name is required';
        ye_oshybky = true;
    } else if (firstname.length > 20) {
        document.getElementById('error-firstname').textContent = 'First name must be max 20 characters';
        ye_oshybky = true;
    } else if (!/^[a-zA-Z]+$/.test(firstname)) {
        document.getElementById('error-firstname').textContent = 'First name can only contain letters';
        ye_oshybky = true;
    }

    // Валідуємо прізвище
    if (lastname === '') {
        document.getElementById('error-lastname').textContent = 'Last name is required';
        ye_oshybky = true;
    } else if (lastname.length > 20) {
        document.getElementById('error-lastname').textContent = 'Last name must be max 20 characters';
        ye_oshybky = true;
    } else if (!/^[a-zA-Z]+$/.test(lastname)) {
        document.getElementById('error-lastname').textContent = 'Last name can only contain letters';
        ye_oshybky = true;
    }

    // Валідуємо рік
    if (year === '') {
        document.getElementById('error-year').textContent = 'Year of birth is required';
        ye_oshybky = true;
    } else {
        var rік_chyslo = parseInt(year);
        var potochnryy_rik = new Date().getFullYear();

        if (rік_chyslo < 1900 || rік_chyslo > potochnryy_rik) {
            document.getElementById('error-year').textContent = 'Year must be between 1900 and ' + potochnryy_rik;
            ye_oshybky = true;
        }
    }

    // Валідуємо стать
    if (gender === '') {
        document.getElementById('error-gender').textContent = 'Gender is required';
        ye_oshybky = true;
    }

    // Валідуємо номер телефону (необов'язкове поле)
    if (phone !== '') {
        // Лічимо цифри у номері
        var cyfry = phone.replace(/\D/g, '').length;

        // Перевіряємо символи
        if (!/^[\d\s\(\)\-]+$/.test(phone)) {
            document.getElementById('error-phone').textContent = 'Phone can only contain digits, spaces, parentheses and dashes';
            ye_oshybky = true;
        } else if (cyfry < 10 || cyfry > 12) {
            document.getElementById('error-phone').textContent = 'Phone must contain 10-12 digits';
            ye_oshybky = true;
        }
    }

    // Валідуємо Skype (необов'язкове поле)
    if (skype !== '') {
        // Перевіряємо символи
        if (!/^[a-zA-Z0-9._-]+$/.test(skype)) {
            document.getElementById('error-skype').textContent = 'Skype can only contain letters, digits, dash and dot';
            ye_oshybky = true;
        }
    }

    // Якщо немає помилок
    if (!ye_oshybky) {
        // Отримуємо існуючі дані
        var registration_data = otrymaty_cookie('registration_data');

        // Додаємо нові дані
        var profile_data = {
            firstname: firstname,
            lastname: lastname,
            year: year,
            gender: gender,
            phone: phone,
            skype: skype
        };

        // Зберігаємо дані в Cookie
        vstanovyty_cookie('profile_data', profile_data, 1);

        alert('Data saved successfully!');
    }
}

function vyyjty() {
    // Видаляємо дані з Cookie
    vydalyty_cookie('registration_data');
    vydalyty_cookie('profile_data');

    // Переходимо на першу сторінку
    perejty_na_registration();
}

// ФУНКЦІЇ ДЛЯ НАВІГАЦІЇ МІЖ СТОРІНКАМИ

function perejty_na_registration() {
    // Ховаємо профіль
    document.getElementById('storinka-profile').classList.remove('active');

    // Показуємо реєстрацію
    document.getElementById('storinka-registration').classList.add('active');

    // Очищуємо форму реєстрації
    document.getElementById('input-email').value = '';
    document.getElementById('input-password').value = '';
    document.getElementById('input-repeat-password').value = '';

    // Очищуємо помилки
    document.getElementById('error-email').textContent = '';
    document.getElementById('error-password').textContent = '';
    document.getElementById('error-repeat').textContent = '';
}

function perejty_na_profile() {
    // Отримуємо email з Cookie
    var registration_data = otrymaty_cookie('registration_data');

    if (registration_data) {
        // Оновлюємо приміт користувача
        document.getElementById('welcome-text').textContent = 'Hello, ' + registration_data.email + '!';

        // Отримуємо профільні дані (якщо вони є)
        var profile_data = otrymaty_cookie('profile_data');

        if (profile_data) {
            // Заповнюємо форму збереженими даними
            document.getElementById('input-firstname').value = profile_data.firstname;
            document.getElementById('input-lastname').value = profile_data.lastname;
            document.getElementById('input-year').value = profile_data.year;
            document.getElementById('select-gender').value = profile_data.gender;
            document.getElementById('input-phone').value = profile_data.phone;
            document.getElementById('input-skype').value = profile_data.skype;
        } else {
            // Очищуємо форму
            document.getElementById('input-firstname').value = '';
            document.getElementById('input-lastname').value = '';
            document.getElementById('input-year').value = '';
            document.getElementById('select-gender').value = '';
            document.getElementById('input-phone').value = '';
            document.getElementById('input-skype').value = '';
        }

        // Очищуємо помилки
        document.getElementById('error-firstname').textContent = '';
        document.getElementById('error-lastname').textContent = '';
        document.getElementById('error-year').textContent = '';
        document.getElementById('error-gender').textContent = '';
        document.getElementById('error-phone').textContent = '';
        document.getElementById('error-skype').textContent = '';

        // Ховаємо реєстрацію
        document.getElementById('storinka-registration').classList.remove('active');

        // Показуємо профіль
        document.getElementById('storinka-profile').classList.add('active');
    } else {
        // Якщо немає даних, переходимо на реєстрацію
        perejty_na_registration();
    }
}

// ІНІЦІАЛІЗАЦІЯ

document.addEventListener('DOMContentLoaded', function() {
    // Перевіряємо чи користувач уже зареєстрований
    var registration_data = otrymaty_cookie('registration_data');

    if (registration_data) {
        // Переходимо на профіль
        perejty_na_profile();
    } else {
        // Показуємо реєстрацію
        document.getElementById('storinka-registration').classList.add('active');
    }

    // Додаємо обробники подій
    document.getElementById('button-signup').addEventListener('click', vykonaty_signup);
    document.getElementById('button-save').addEventListener('click', vykonaty_save);
    document.getElementById('link-exit').addEventListener('click', function(event) {
        event.preventDefault();
        vyyjty();
    });

    // Додаємо можливість реєстрації через Enter
    document.getElementById('input-repeat-password').addEventListener('keypress', function(event) {
        if (event.key === 'Enter') {
            vykonaty_signup();
        }
    });
});