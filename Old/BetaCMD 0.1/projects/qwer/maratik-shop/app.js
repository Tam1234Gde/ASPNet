const defaultProducts = [
  { name: 'Вишнёвый вечер', description: 'Для мягкого настроения и ярких признаний.', price: 2490, color: 'red' },
  { name: 'Мармеладный набор', description: 'Маленький сюрприз для большого удовольствия.', price: 1890, color: 'pink' },
  { name: 'Тёплый секрет', description: 'Нежный спутник для личных ритуалов.', price: 3290, color: 'cream' }
];
const storageKey = 'maratik-products';
const grid = document.querySelector('#productGrid');
const template = document.querySelector('#productTemplate');
const form = document.querySelector('#productForm');
const count = document.querySelector('#catalogCount');
let products;
try { products = JSON.parse(localStorage.getItem(storageKey)) || defaultProducts; } catch { products = defaultProducts; }

function render() {
  grid.replaceChildren();
  count.textContent = `${products.length} ${products.length === 1 ? 'ПОЗИЦИЯ' : 'ПОЗИЦИИ'}`;
  products.forEach((product, index) => {
    const card = template.content.firstElementChild.cloneNode(true);
    card.classList.add(product.color);
    card.style.animationDelay = `${index * 80}ms`;
    card.querySelector('h3').textContent = product.name;
    card.querySelector('.product-description').textContent = product.description;
    card.querySelector('strong').textContent = `${Number(product.price).toLocaleString('ru-RU')} ₽`;
    card.querySelector('.remove').addEventListener('click', () => {
      products.splice(index, 1);
      saveAndRender();
    });
    grid.append(card);
  });
}
function saveAndRender() {
  localStorage.setItem(storageKey, JSON.stringify(products));
  render();
}
form.addEventListener('submit', (event) => {
  event.preventDefault();
  const data = new FormData(form);
  products.push({
    name: data.get('productName') || document.querySelector('#productName').value.trim(),
    description: data.get('productDescription') || document.querySelector('#productDescription').value.trim(),
    price: Number(document.querySelector('#productPrice').value),
    color: document.querySelector('#productColor').value
  });
  saveAndRender();
  form.reset();
  document.querySelector('#catalog').scrollIntoView({ behavior: 'smooth', block: 'start' });
});
document.querySelector('#year').textContent = new Date().getFullYear();
render();
