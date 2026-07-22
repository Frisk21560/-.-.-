// Масив для зберігання кольорів
var kolory_lista = [];

// ФУНКЦІЇ ДЛЯ РОБОТИ З COOKIE

// Функція для збереження коллекції в Cookie
function zberegty_cookie() {
    // Конвертуємо масив у JSON
    var json_kolory = JSON.stringify(kolory_lista);

    // Створюємо дату та час закінчення (3 години від тепер)
    var data_zakinchennya = new Date();
    data_zakinchennya.setTime(data_zakinchennya.getTime() + (3 * 60 * 60 * 1000));
    var expires = "expires=" + data_zakinchennya.toUTCString();

    // Встановлюємо Cookie
    document.cookie = "kolory=" + json_kolory + ";" + expires + ";path=/";
}

// Функція для завантаження коллекції з Cookie
function zavantazhyty_cookie() {
    // Отримуємо всі cookies
    var cookies_string = document.cookie;

    // Розділяємо cookies на окремі значення
    var cookies_array = cookies_string.split(';');

    // Проходимо по cookies
    for (var i = 0; i < cookies_array.length; i++) {
        var cookie = cookies_array[i].trim();

        // Перевіряємо чи це наш cookie
        if (cookie.indexOf('kolory=') === 0) {
            // Отримуємо значення
            var cookie_value = cookie.substring(7);

            // Декодуємо та парсимо JSON
            try {
                kolory_lista = JSON.parse(decodeURIComponent(cookie_value));
            } catch (e) {
                kolory_lista = [];
            }
            return;
        }
    }

    // Якщо cookie не знайдено
    kolory_lista = [];
}

// ФУНКЦІЇ ДЛЯ ВАЛІДАЦІЇ

// Функція для валідації назви кольору
function pereveryty_imya_koloru(imya) {
    // Очищуємо помилку
    document.getElementById('error-color-name').textContent = '';

    // Перевіряємо чи заповнено поле
    if (imya.trim() === '') {
        document.getElementById('error-color-name').textContent = 'Color name is required';
        return false;
    }

    // Перевіряємо чи містить тільки літери
    if (!/^[a-zA-Z\s-]+$/.test(imya)) {
        document.getElementById('error-color-name').textContent = 'Color can only contain letters';
        return false;
    }

    // Перевіряємо унікальність (не регістрозалежно)
    var imya_lower = imya.toLowerCase();
    for (var i = 0; i < kolory_lista.length; i++) {
        if (kolory_lista[i].imya.toLowerCase() === imya_lower) {
            document.getElementById('error-color-name').textContent = 'This color name already exists';
            return false;
        }
    }

    return true;
}

// Функція для валідації коду RGB
function pereveryty_rgb_kod(kod) {
    // Розділяємо код на частини
    var parts = kod.split(',');

    // Перевіряємо кількість частин
    if (parts.length !== 3) {
        return false;
    }

    // Перевіряємо кожну частину
    for (var i = 0; i < parts.length; i++) {
        var value = parseInt(parts[i].trim());

        // Перевіряємо чи це число
        if (isNaN(value)) {
            return false;
        }

        // Перевіряємо чи число в межах 0-255
        if (value < 0 || value > 255) {
            return false;
        }
    }

    return true;
}

// Функція для валідації коду RGBA
function pereveryty_rgba_kod(kod) {
    // Розділяємо код на частини
    var parts = kod.split(',');

    // Перевіряємо кількість частин
    if (parts.length !== 4) {
        return false;
    }

    // Перевіряємо перші 3 частини
    for (var i = 0; i < 3; i++) {
        var value = parseInt(parts[i].trim());

        // Перевіряємо чи це число
        if (isNaN(value)) {
            return false;
        }

        // Перевіряємо чи число в межах 0-255
        if (value < 0 || value > 255) {
            return false;
        }
    }

    // Перевіряємо четвертку частину (альфа)
    var alpha = parseFloat(parts[3].trim());

    // Перевіряємо чи це число
    if (isNaN(alpha)) {
        return false;
    }

    // Перевіряємо чи число в межах 0-1
    if (alpha < 0 || alpha > 1) {
        return false;
    }

    return true;
}

// Функція для валідації коду HEX
function pereveryty_hex_kod(kod) {
    // Перевіряємо формат HEX (# та 6 символів)
    var hex_pattern = /^#[0-9A-Fa-f]{6}$/;

    return hex_pattern.test(kod);
}

// Функція для валідації коду кольору
function pereveryty_kod_koloru(kod, typ) {
    // Очищуємо помилку
    document.getElementById('error-code').textContent = '';

    // Перевіряємо чи заповнено поле
    if (kod.trim() === '') {
        document.getElementById('error-code').textContent = 'Color code is required';
        return false;
    }

    // Перевіряємо відповідно до типу
    if (typ === 'RGB') {
        if (!pereveryty_rgb_kod(kod)) {
            document.getElementById('error-code').textContent = 'RGB code must match the pattern [0-255], [0-255], [0-255]';
            return false;
        }
    } else if (typ === 'RGBA') {
        if (!pereveryty_rgba_kod(kod)) {
            document.getElementById('error-code').textContent = 'RGBA code must match the pattern [0-255], [0-255], [0-255], [0-1]';
            return false;
        }
    } else if (typ === 'HEX') {
        if (!pereveryty_hex_kod(kod)) {
            document.getElementById('error-code').textContent = 'HEX code must match the pattern #XXXXXX';
            return false;
        }
    }

    return true;
}

// ФУНКЦІЇ ДЛЯ КОНВЕРТУВАННЯ КОЛЬОРІВ

// Функція для конвертування RGB у CSS string
function rgb_do_css(kod) {
    return 'rgb(' + kod + ')';
}

// Функція для конвертування RGBA у CSS string
function rgba_do_css(kod) {
    return 'rgba(' + kod + ')';
}

// Функція для конвертування HEX - він вже CSS string
function hex_do_css(kod) {
    return kod;
}

// ФУНКЦІЇ ДЛЯ РОБОТИ З КОЛЬОРАМИ

// Функція для додавання нового кольору
function dodaty_novyy_kolir() {
    // Отримуємо значення з полів
    var imya = document.getElementById('input-color-name').value.trim();
    var typ = document.getElementById('select-type').value;
    var kod = document.getElementById('input-code').value.trim();

    // Валідуємо дані
    if (!pereveryty_imya_koloru(imya)) {
        return;
    }

    if (!pereveryty_kod_koloru(kod, typ)) {
        return;
    }

    // Створюємо об'єкт кольору
    var novyy_kolir = {
        imya: imya,
        typ: typ,
        kod: kod
    };

    // Додаємо колір до масиву
    kolory_lista.push(novyy_kolir);

    // Зберігаємо у Cookie
    zberegty_cookie();

    // Очищуємо форму
    document.getElementById('input-color-name').value = '';
    document.getElementById('input-code').value = '';
    document.getElementById('select-type').value = 'RGB';

    // Очищуємо помилки
    document.getElementById('error-color-name').textContent = '';
    document.getElementById('error-code').textContent = '';

    // Оновлюємо відображення палітри
    onovyty_palitru();
}

// Функція для оновлення відображення палітри
function onovyty_palitru() {
    // Отримуємо контейнер
    var kontejner = document.getElementById('paletka-list');
    // Очищуємо контейнер
    kontejner.innerHTML = '';

    // Проходимо по всіх кольорах
    kolory_lista.forEach(function(kolir) {
        // Створюємо карту кольору
        var carta_koloru = document.createElement('div');
        carta_koloru.className = 'color-card';

        // Створюємо preview для кольору
        var preview = document.createElement('div');
        preview.className = 'color-preview';

        // Визначаємо CSS стиль для колору
        var css_kolir;
        if (kolir.typ === 'RGB') {
            css_kolir = rgb_do_css(kolir.kod);
        } else if (kolir.typ === 'RGBA') {
            css_kolir = rgba_do_css(kolir.kod);
        } else if (kolir.typ === 'HEX') {
            css_kolir = hex_do_css(kolir.kod);
        }

        preview.style.backgroundColor = css_kolir;

        // Створюємо інформацію про колір
        var info = document.createElement('div');
        info.className = 'color-info';

        // Заповнюємо HTML інформації
        info.innerHTML = '<div class="color-name">' + kolir.imya + '</div>' +
                        '<div class="color-type">' + kolir.typ + '</div>' +
                        '<div class="color-code">' + kolir.kod + '</div>';

        // Додаємо preview та інформацію до карти
        carta_koloru.appendChild(preview);
        carta_koloru.appendChild(info);

        // Додаємо карту до контейнера
        kontejner.appendChild(carta_koloru);
    });
}

// ІНІЦІАЛІЗАЦІЯ

document.addEventListener('DOMContentLoaded', function() {
    // Завантажуємо кольори з Cookie
    zavantazhyty_cookie();

    // Оновлюємо відображення палітри
    onovyty_palitru();

    // Додаємо обробник для кнопки Save
    document.getElementById('button-save').addEventListener('click', dodaty_novyy_kolir);

    // Додаємо обробник для Enter у полі коду
    document.getElementById('input-code').addEventListener('keypress', function(event) {
        if (event.key === 'Enter') {
            dodaty_novyy_kolir();
        }
    });
});