// ЗАВДАННЯ 1: Введення імені без цифр
const nameInput = document.getElementById('nameInput');

nameInput.addEventListener('input', (e) => {
    e.target.value = e.target.value.replace(/[0-9]/g, '');
});

// ЗАВДАННЯ 2: Модальне вікно
const openModalBtn = document.getElementById('openModalBtn');
const closeModalBtn = document.getElementById('closeModalBtn');
const modal = document.getElementById('modal');

openModalBtn.addEventListener('click', () => {
    modal.classList.add('show');
});

closeModalBtn.addEventListener('click', () => {
    modal.classList.remove('show');
});

window.addEventListener('click', (e) => {
    if (e.target === modal) {
        modal.classList.remove('show');
    }
});

// ЗАВДАННЯ 3: Футбольне поле
const footballField = document.getElementById('footballField');
const ball = document.getElementById('ball');

footballField.addEventListener('click', (e) => {
    const rect = footballField.getBoundingClientRect();
    const fieldX = e.clientX - rect.left;
    const fieldY = e.clientY - rect.top;

    // Обмеження м'яча в межах поля (враховуючи його розмір)
    const ballSize = 100;
    const maxX = rect.width - ballSize / 2;
    const maxY = rect.height - ballSize / 2;
    const minX = ballSize / 2;
    const minY = ballSize / 2;

    const finalX = Math.max(minX, Math.min(maxX, fieldX));
    const finalY = Math.max(minY, Math.min(maxY, fieldY));

    ball.style.left = finalX + 'px';
    ball.style.top = finalY + 'px';
    ball.style.transform = 'translate(-50%, -50%)';
});

// ЗАВДАННЯ 4: Світлофор
const trafficLights = ['red', 'yellow', 'green'];
let currentLightIndex = 0;

function setTrafficLight(index) {
    trafficLights.forEach((color) => {
        const light = document.getElementById(color);
        light.classList.remove('active');
    });
    
    const activeLight = document.getElementById(trafficLights[index]);
    activeLight.classList.add('active');
}

document.getElementById('nextTrafficBtn').addEventListener('click', () => {
    currentLightIndex = (currentLightIndex + 1) % trafficLights.length;
    setTrafficLight(currentLightIndex);
});

// Ініціалізація світлофора
setTrafficLight(0);

// ЗАВДАННЯ 5: Список книг
const bookItems = document.querySelectorAll('.book-item');
let selectedBook = null;

bookItems.forEach((book) => {
    book.addEventListener('click', () => {
        // Видалити активний клас з попередньої книги
        if (selectedBook) {
            selectedBook.classList.remove('active');
        }
        
        // Додати активний клас до нової книги
        book.classList.add('active');
        selectedBook = book;
    });
});

// ЗАВДАННЯ 6: Кнопки з підказками
const tooltipBtns = document.querySelectorAll('.tooltip-btn');

tooltipBtns.forEach((btn) => {
    const tooltip = document.createElement('div');
    tooltip.className = 'tooltip';
    tooltip.textContent = btn.dataset.tooltip;
    btn.appendChild(tooltip);

    btn.addEventListener('mouseenter', () => {
        const btnRect = btn.getBoundingClientRect();
        const tooltipHeight = tooltip.offsetHeight;
        const tooltipTop = btnRect.top - tooltipHeight - 15;

        // Перевірка, чи підказка помішається зверху
        if (tooltipTop < 20) {
            // Якщо не помішається, показувати знизу
            tooltip.classList.add('bottom');
        } else {
            tooltip.classList.remove('bottom');
        }

        tooltip.classList.add('show');
    });

    btn.addEventListener('mouseleave', () => {
        tooltip.classList.remove('show');
    });
});