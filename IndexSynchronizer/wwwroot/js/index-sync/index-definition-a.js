function spanTest() {
    const indexArea1 = document.getElementById('indexArea1');

    const indexArea2 = document.getElementById('indexArea2');

    var new1 = document.createElement("p");
    new1.textContent = "new 1 success";

    indexArea1.appendChild(new1);

    var new2 = document.createElement("p");
    new2.textContent = "new 2 success";

    indexArea2.appendChild(new2);
}

setTimeout(spanTest, 3000);