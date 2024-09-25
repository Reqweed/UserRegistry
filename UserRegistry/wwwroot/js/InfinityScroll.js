let page = 1;
let errorCount = 0;

const bodyTable = document.body.querySelector('.table__body');
const allTable = document.body.querySelector('.table__container');
const regionSelector = document.body.querySelector('.navbar__select-region');
const seedInput = document.body.querySelector('.navbar__input-seed');
const errorCountSlider = document.body.querySelector('.navbar__range-error');
const errorCountInput = document.body.querySelector('.navbar__input-error');
const randomSeedButton = document.body.querySelector('.navbar__button-seed');

async function updateTable() {
    page = 1;
    bodyTable.innerHTML = "";
    await fetchData(20);
}

async function fetchData(count) {
    const response = await fetch('Home/GenerateFakeUsers', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            region: regionSelector.value,
            errorCount: errorCount,
            seed: seedInput.value,
            page: page,
            count: count
        })
    });

    const data = await response.json();
    addRowsTable(data);
    page++;
}

function addRowsTable(data) {
    const rows = data.map(item => `
        <tr>
            <td class="text-wrap">${item.number}</td>
            <td class="text-wrap">${item.identifier}</td>
            <td class="text-wrap">${item.fullName}</td>
            <td class="text-wrap">${item.address}</td>
            <td class="text-wrap">${item.phoneNumber}</td>
        </tr>`
    ).join('');

    bodyTable.insertAdjacentHTML('beforeend', rows);
}

function generateRandomSeed() {
    seedInput.value = Math.floor(Math.random() * 2147483648);
    updateTable();
}

function updateErrorCount(value) {
    errorCount = Math.max(0, Math.min(value, 1000));
    errorCountSlider.value = errorCount;
    errorCountInput.value = errorCount;
    updateTable();
}

allTable.addEventListener('scroll', () => {
    if (allTable.scrollTop + allTable.clientHeight >= allTable.scrollHeight - 10) {
        fetchData(10);
    }
});

regionSelector.addEventListener('change', () => updateTable());
seedInput.addEventListener('input', () => updateTable());
errorCountSlider.addEventListener('input', () => updateErrorCount(parseFloat(errorCountSlider.value)));
errorCountInput.addEventListener('input', () => updateErrorCount(parseInt(errorCountInput.value)));
randomSeedButton.addEventListener('click', generateRandomSeed);

fetchData(20);
