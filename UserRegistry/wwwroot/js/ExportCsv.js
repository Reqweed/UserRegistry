function getTableData() {
    const rows = document.querySelectorAll('tbody.table__body tr');
    const data = [];

    rows.forEach(row => {
        const cells = row.querySelectorAll('td');
        const rowData = {
            number: cells[0].innerText,
            identifier: cells[1].innerText,
            fullName: cells[2].innerText,
            address: cells[3].innerText,
            phoneNumber: cells[4].innerText
        };
        data.push(rowData);
    });
    
    return data;
}
document.querySelector(".navbar__button-export").addEventListener("click", function () {
    fetch('Home/ExportToCsv', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(getTableData())
    })
        .then(response => response.blob())
        .then(blob => {
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = "data.csv";
            link.click();
        });
});