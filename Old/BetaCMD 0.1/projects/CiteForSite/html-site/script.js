// Кастомный курсор
const cursor = document.querySelector('.cursor');
const cursorDot = document.querySelector('.cursor-dot');

let mouseX = 0;
let mouseY = 0;
let cursorX = 0;
let cursorY = 0;
let dotX = 0;
let dotY = 0;

document.addEventListener('mousemove', (e) => {
    mouseX = e.clientX;
    mouseY = e.clientY;
});

function animateCursor() {
    const dx = mouseX - cursorX;
    const dy = mouseY - cursorY;
    cursorX += dx * 0.1;
    cursorY += dy * 0.1;

    const dotDx = mouseX - dotX;
    const dotDy = mouseY - dotY;
    dotX += dotDx * 0.15;
    dotY += dotDy * 0.15;

    cursor.style.left = cursorX + 'px';
    cursor.style.top = cursorY + 'px';
    cursorDot.style.left = dotX + 'px';
    cursorDot.style.top = dotY + 'px';

    requestAnimationFrame(animateCursor);
}

animateCursor();

// Плавная прокрутка
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Параллакс для Hero Core
const heroCore = document.querySelector('.hero-core');

document.addEventListener('mousemove', (e) => {
    if (window.innerWidth > 768) {
        const x = (e.clientX / window.innerWidth - 0.5) * 30;
        const y = (e.clientY / window.innerHeight - 0.5) * 30;
        
        if (heroCore) {
            heroCore.style.transform = `translate(${x}px, calc(-50% + ${y}px))`;
        }
    }
});

// 3D эффект для виджетов
const widgets = document.querySelectorAll('.glass-widget');

widgets.forEach(widget => {
    widget.addEventListener('mousemove', (e) => {
        const rect = widget.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;
        
        const centerX = rect.width / 2;
        const centerY = rect.height / 2;
        
        const rotateX = (y - centerY) / 10;
        const rotateY = (centerX - x) / 10;
        
        widget.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) translateY(-5px)`;
    });
    
    widget.addEventListener('mouseleave', () => {
        widget.style.transform = 'perspective(1000px) rotateX(0) rotateY(0) translateY(0)';
    });
});

// Анимация появления элементов при скролле
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -100px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Применяем observer к элементам
const animateElements = document.querySelectorAll('.service-card, .case-card, .pipeline-step');
animateElements.forEach(el => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(30px)';
    el.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
    observer.observe(el);
});

// Анимация логов в терминале
const terminalLogs = document.getElementById('terminal-logs');
const logs = [
    { text: '> Running compilation ons...', delay: 0 },
    { text: "> Running: compiling object builders.NETS from 'sits'", delay: 800 },
    { text: '> compiling code.depoatenty', delay: 1600 },
    { text: '> Build successful: 0 errors, 0 warnings', delay: 2400, success: true },
    { text: '> Optimizing bundle size...', delay: 3200 },
    { text: '> Deploying to staging environment...', delay: 4000, success: true }
];

let logIndex = 0;

function addLog() {
    if (logIndex < logs.length) {
        const log = logs[logIndex];
        setTimeout(() => {
            const logLine = document.createElement('div');
            logLine.className = 'log-line' + (log.success ? ' success' : '');
            logLine.textContent = log.text;
            terminalLogs.appendChild(logLine);
            
            // Автоскролл вниз
            terminalLogs.scrollTop = terminalLogs.scrollHeight;
            
            logIndex++;
            addLog();
        }, log.delay);
    } else {
        // Добавляем мигающий курсор
        setTimeout(() => {
            const cursor = document.createElement('span');
            cursor.style.display = 'inline-block';
            cursor.style.width = '8px';
            cursor.style.height = '16px';
            cursor.style.backgroundColor = 'var(--neon-green)';
            cursor.style.marginLeft = '4px';
            cursor.style.animation = 'blink 1s infinite';
            terminalLogs.appendChild(cursor);
        }, 500);
    }
}

// Запускаем анимацию логов когда терминал виден
const terminalObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting && logIndex === 0) {
            addLog();
        }
    });
}, { threshold: 0.5 });

const terminal = document.querySelector('.terminal');
if (terminal) {
    terminalObserver.observe(terminal);
}

// Анимация графиков
const chartBars = document.querySelectorAll('.bar');
let chartAnimated = false;

const chartObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting && !chartAnimated) {
            chartAnimated = true;
            chartBars.forEach((bar, index) => {
                setTimeout(() => {
                    bar.style.transition = 'height 0.6s ease';
                    const height = bar.style.height;
                    bar.style.height = '0%';
                    setTimeout(() => {
                        bar.style.height = height;
                    }, 50);
                }, index * 100);
            });
        }
    });
}, { threshold: 0.5 });

const chart = document.querySelector('.chart');
if (chart) {
    chartObserver.observe(chart);
}

// Hover эффект для карточек услуг
const serviceCards = document.querySelectorAll('.service-card');

serviceCards.forEach(card => {
    card.addEventListener('mouseenter', () => {
        serviceCards.forEach(otherCard => {
            if (otherCard !== card) {
                otherCard.style.opacity = '0.6';
            }
        });
    });
    
    card.addEventListener('mouseleave', () => {
        serviceCards.forEach(otherCard => {
            otherCard.style.opacity = '1';
        });
    });
});

// Интерактивные tech badges
const techBadges = document.querySelectorAll('.tech-badge');

techBadges.forEach(badge => {
    badge.addEventListener('click', () => {
        const tech = badge.textContent;
        const newLog = document.createElement('div');
        newLog.className = 'log-line success';
        newLog.textContent = `> Filtering logs for ${tech}...`;
        terminalLogs.appendChild(newLog);
        terminalLogs.scrollTop = terminalLogs.scrollHeight;
    });
});

// Форма контакта
const contactForm = document.querySelector('.contact-form');

contactForm.addEventListener('submit', (e) => {
    e.preventDefault();
    
    const submitButton = contactForm.querySelector('.submit-button');
    const originalText = submitButton.textContent;
    
    submitButton.textContent = 'ОТПРАВКА...';
    submitButton.disabled = true;
    
    // Имитация отправки
    setTimeout(() => {
        submitButton.textContent = 'ОТПРАВЛЕНО ✓';
        submitButton.style.background = 'var(--neon-green)';
        
        setTimeout(() => {
            submitButton.textContent = originalText;
            submitButton.disabled = false;
            contactForm.reset();
        }, 2000);
    }, 1500);
});

// Пульсация quantum sphere при вводе в форму
const formInputs = document.querySelectorAll('.form-group input, .form-group textarea');
const quantumSphere = document.querySelector('.quantum-sphere');

formInputs.forEach(input => {
    input.addEventListener('focus', () => {
        if (quantumSphere) {
            quantumSphere.style.boxShadow = '0 0 120px rgba(0, 255, 102, 0.8), inset 0 0 80px rgba(0, 255, 102, 0.6)';
        }
    });
    
    input.addEventListener('blur', () => {
        if (quantumSphere) {
            quantumSphere.style.boxShadow = '0 0 80px rgba(0, 255, 102, 0.6), inset 0 0 60px rgba(0, 255, 102, 0.4)';
        }
    });
});

// Эффект наведения на кнопки
const buttons = document.querySelectorAll('button');

buttons.forEach(button => {
    button.addEventListener('mouseenter', () => {
        cursor.style.transform = 'scale(1.5)';
    });
    
    button.addEventListener('mouseleave', () => {
        cursor.style.transform = 'scale(1)';
    });
});

// Динамическое обновление данных виджета
setInterval(() => {
    const widgetValue = document.querySelector('.widget-value');
    if (widgetValue) {
        const currentValue = parseFloat(widgetValue.textContent);
        const change = (Math.random() - 0.5) * 5;
        const newValue = (currentValue + change).toFixed(1);
        widgetValue.textContent = newValue;
    }
}, 3000);

// Случайное изменение высоты баров графика
setInterval(() => {
    chartBars.forEach(bar => {
        const randomHeight = Math.floor(Math.random() * 60) + 30;
        bar.style.height = randomHeight + '%';
    });
}, 2000);

console.log('🚀 CORETECH-IT Website Loaded');
console.log('💚 Neon Green Theme Active');
