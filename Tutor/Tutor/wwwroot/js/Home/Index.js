let index = 1;
const track = document.getElementById('courseTrack');

function updateCarousel() {
    const cards = document.querySelectorAll('.course-card');
    cards.forEach((card, i) => card.classList.toggle('active', i === index));
}

function prevCourse() {
    if (index > 0) {
        index--;
        updateCarousel();
        scrollToCard(index);
    }
}

function nextCourse() {
    const cards = document.querySelectorAll('.course-card');
    if (index < cards.length - 1) {
        index++;
        updateCarousel();
        scrollToCard(index);
    }
}

// Функция для плавного скролла к карточке
function scrollToCard(index) {
    const cards = document.querySelectorAll('.course-card');
    if (cards[index]) {
        track.scrollTo({
            left: cards[index].offsetLeft - track.offsetLeft - track.clientWidth / 2 + cards[index].clientWidth / 2,
            behavior: "smooth"
        });
    }
}

// Определяем активный курс при скролле
track.addEventListener("scroll", () => {
    const cards = document.querySelectorAll('.course-card');
    let minDistance = Infinity;
    let activeIndex = index;

    cards.forEach((card, i) => {
        const cardCenter = card.offsetLeft + card.offsetWidth / 2;
        const trackCenter = track.scrollLeft + track.clientWidth / 2;
        const distance = Math.abs(trackCenter - cardCenter);

        if (distance < minDistance) {
            minDistance = distance;
            activeIndex = i;
        }
    });

    index = activeIndex;
    updateCarousel();
});

// Поддержка свайпа (мобильные устройства)
track.addEventListener("touchstart", (e) => {
    track.dataset.startX = e.touches[0].clientX;
    track.dataset.startScroll = track.scrollLeft;
});

track.addEventListener("touchmove", (e) => {
    let startX = parseFloat(track.dataset.startX);
    let scrollStart = parseFloat(track.dataset.startScroll);
    let moveX = e.touches[0].clientX - startX;
    track.scrollLeft = scrollStart - moveX;
}, { passive: true });

// Поддержка скролла мышью или двумя пальцами на тачпаде
track.addEventListener("wheel", (e) => {
    e.preventDefault();
    track.scrollLeft += e.deltaY;
}, { passive: true });

updateCarousel();