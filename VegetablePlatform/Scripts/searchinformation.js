 let name = document.getElementById('productname');
    document.getElementById("btn-search").addEventListener('click', function (){
        if (name.value.length > 10) {
            alert('不可以輸入超過10個字，請重新搜尋。');
            name.value = '';
            event.preventDefault();
        }
        else if (name.value.length < 2)
        {
            alert('不可以輸入少於2個字，請重新搜尋。');
            name.value = '';
            event.preventDefault();
        }
    });